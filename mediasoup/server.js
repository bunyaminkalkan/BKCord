/**
 * =============================================
 * MEDIASOUP SUNUCU - İYİLEŞTİRİLMİŞ VERSİYON
 * =============================================
 * Bu dosya, Mediasoup kullanarak bir WebRTC SFU (Selective Forwarding Unit) sunucusu
 * için gerekli olan tüm mantığı içerir.
 *
 * İyileştirmeler:
 * - Kod, 'Room' ve 'Peer' sınıfları kullanılarak nesne yönelimli hale getirildi.
 * - Dağınık global Map'ler yerine tüm durum (state) ilgili oda ve katılımcı
 * nesneleri içinde yönetiliyor.
 * - Kaynak yönetimi (transport, producer, consumer kapatma) otomatikleştirildi
 * ve daha güvenilir hale getirildi.
 * - Sinyalleşme (WebSocket) ihtiyacı yorumlarla belirtildi.
 * - Daha iyi okunabilirlik ve bakım için kod yeniden düzenlendi.
 */

const express = require('express');
const mediasoup = require('mediasoup');
const cors =require('cors');

// --- Express Uygulaması Kurulumu ---
const app = express();
app.use(cors());
app.use(express.json());

// --- Mediasoup Yapılandırması ---
const mediasoupConfig = {
  worker: {
    rtcMinPort: 40000,
    rtcMaxPort: 49999,
    logLevel: 'warn',
    logTags: [
      'info',
      'ice',
      'dtls',
      'rtp',
      'srtp',
      'rtcp',
    ],
  },
  router: {
    mediaCodecs: [
      {
        kind: 'audio',
        mimeType: 'audio/opus',
        clockRate: 48000,
        channels: 2,
      },
      {
        kind: 'video',
        mimeType: 'video/VP8',
        clockRate: 90000,
        parameters: {
          'x-google-start-bitrate': 1000,
        },
      },
    ],
  },
  webRtcTransport: {
    listenIps: [
      {
        ip: '0.0.0.0',
        // DİKKAT: Üretim (production) ortamında buraya sunucunuzun
        // genel (public) IP adresini yazmalısınız.
        announcedIp: process.env.PUBLIC_IP || '127.0.0.1',
      },
    ],
    enableUdp: true,
    enableTcp: true,
    preferUdp: true,
    maxIncomingBitrate: 1500000,
    initialAvailableOutgoingBitrate: 1000000,
  },
};

// --- Global Değişkenler ---
let worker;
const rooms = new Map(); // <roomId, Room>

// =================================================================================
// Sınıflar (Classes) - Kodun Yönetimini Kolaylaştırmak İçin
// =================================================================================

class Room {
  constructor(id, router) {
    this.id = id;
    this.router = router;
    this.peers = new Map(); // <peerId, Peer>
  }

  addPeer(peer) {
    this.peers.set(peer.id, peer);
  }

  getPeer(peerId) {
    return this.peers.get(peerId);
  }

  getPeers() {
    return this.peers;
  }

  close() {
    console.log(`Closing room ${this.id}...`);
    this.peers.forEach(peer => peer.close());
    this.router.close();
  }
}

class Peer {
  constructor(id, roomId) {
    this.id = id;
    this.roomId = roomId;
    this.transports = new Map();
    this.producers = new Map();
    this.consumers = new Map();
  }

  addTransport(transport) {
    this.transports.set(transport.id, transport);
  }

  async connectTransport(transportId, dtlsParameters) {
    const transport = this.transports.get(transportId);
    if (!transport) {
      throw new Error(`Transport with id "${transportId}" not found.`);
    }
    await transport.connect({ dtlsParameters });
  }

  async createProducer({ transportId, rtpParameters, kind }) {
    const transport = this.transports.get(transportId);
    if (!transport) {
      throw new Error(`Transport with id "${transportId}" not found for producing.`);
    }
    const producer = await transport.produce({ kind, rtpParameters });
    this.producers.set(producer.id, producer);

    producer.on('transportclose', () => {
      console.log(`Producer's transport closed ${producer.id}`);
      producer.close();
      this.producers.delete(producer.id);
    });

    return producer;
  }
  
  async createConsumer({ transportId, producerId, rtpCapabilities }) {
    const transport = this.transports.get(transportId);
     if (!transport) {
      throw new Error(`Transport with id "${transportId}" not found for consuming.`);
    }
    const consumer = await transport.consume({
      producerId,
      rtpCapabilities,
      paused: true, // Her zaman duraklatılmış başla, istemci hazır olunca devam ettirsin.
    });
    this.consumers.set(consumer.id, consumer);
    
    consumer.on('transportclose', () => {
      console.log(`Consumer's transport closed ${consumer.id}`);
      this.consumers.delete(consumer.id);
    });
    
    return consumer;
  }

  close() {
    console.log(`Closing peer ${this.id}...`);
    this.transports.forEach(transport => transport.close());
  }
}

// --- Mediasoup Worker Başlatma ---
async function startMediasoup() {
  worker = await mediasoup.createWorker(mediasoupConfig.worker);

  worker.on('died', (error) => {
    console.error('Mediasoup worker has died:', error);
    setTimeout(() => process.exit(1), 2000); // 2 saniye sonra sunucuyu yeniden başlat
  });

  console.log('Mediasoup worker started');
}

// --- Oda Yönetimi Helper Fonksiyonu ---
async function getOrCreateRoom(roomId) {
  let room = rooms.get(roomId);
  if (!room) {
    const router = await worker.createRouter(mediasoupConfig.router);
    room = new Room(roomId, router);
    rooms.set(roomId, room);
    console.log(`Room created: ${roomId}`);
  }
  return room;
}

// =================================================================================
// API Endpoints
// =================================================================================

/**
 * [GET] Sunucunun Router RTP Yeteneklerini alır.
 * Bir istemcinin odaya katılmadan önce alması gereken ilk bilgidir.
 */
app.get('/rooms/:roomId/router-rtp-capabilities', async (req, res) => {
    const { roomId } = req.params;
    try {
        const room = await getOrCreateRoom(roomId);
        res.json(room.router.rtpCapabilities);
    } catch (error) {
        console.error('Get Router RTP Capabilities error:', error);
        res.status(500).json({ success: false, message: error.message });
    }
});


/**
 * [POST] Bir kullanıcıyı odaya dahil eder.
 */
app.post('/rooms/:roomId/join', async (req, res) => {
    const { roomId } = req.params;
    const { userId } = req.body;

    try {
        const room = await getOrCreateRoom(roomId);
        let peer = room.getPeer(userId);
        if (!peer) {
            peer = new Peer(userId, roomId);
            room.addPeer(peer);
        }

        // Odadaki diğer producer'ların ID'lerini yeni kullanıcıya gönder.
        // İstemci bu producer'lar için consumer oluşturabilir.
        const producerIds = [];
        room.getPeers().forEach(p => {
            p.producers.forEach(prod => {
                producerIds.push(prod.id);
            });
        });

        res.json({ success: true, producerIds });
    } catch (error) {
        console.error('Join room error:', error);
        res.status(500).json({ success: false, message: error.message });
    }
});

/**
 * [POST] Bir kullanıcıyı odadan çıkarır ve kaynaklarını temizler.
 */
app.post('/rooms/:roomId/leave', async (req, res) => {
    const { roomId } = req.params;
    const { userId } = req.body;

    try {
        const room = rooms.get(roomId);
        if (!room) {
            return res.json({ success: true }); // Oda zaten yoksa, işlem başarılıdır.
        }

        const peer = room.getPeer(userId);
        if (peer) {
            peer.close();
            room.peers.delete(userId);
        }

        // Oda boşaldıysa, odayı tamamen kapat.
        if (room.getPeers().size === 0) {
            room.close();
            rooms.delete(roomId);
            console.log(`Room deleted: ${roomId}`);
        }

        res.json({ success: true });
    } catch (error) {
        console.error('Leave room error:', error);
        res.status(500).json({ success: false, message: error.message });
    }
});

/**
 * [POST] Belirtilen kullanıcı için bir WebRTC Transport oluşturur.
 */
app.post('/rooms/:roomId/peers/:userId/transports', async (req, res) => {
    const { roomId, userId } = req.params;
    
    try {
        const room = rooms.get(roomId);
        if (!room) throw new Error(`Room with id "${roomId}" not found.`);
        const peer = room.getPeer(userId);
        if (!peer) throw new Error(`Peer with id "${userId}" not found.`);
        
        const transport = await room.router.createWebRtcTransport(mediasoupConfig.webRtcTransport);
        peer.addTransport(transport);
        
        res.json({
            id: transport.id,
            iceParameters: transport.iceParameters,
            iceCandidates: transport.iceCandidates,
            dtlsParameters: transport.dtlsParameters,
        });
    } catch (error) {
        console.error('Create transport error:', error);
        res.status(500).json({ error: error.message });
    }
});

/**
 * [POST] Transport'u, istemcinin DTLS parametreleriyle bağlar.
 */
app.post('/rooms/:roomId/peers/:userId/transports/:transportId/connect', async (req, res) => {
    const { roomId, userId, transportId } = req.params;
    const { dtlsParameters } = req.body;

    try {
        const room = rooms.get(roomId);
        if (!room) throw new Error(`Room with id "${roomId}" not found.`);
        const peer = room.getPeer(userId);
        if (!peer) throw new Error(`Peer with id "${userId}" not found.`);
        
        await peer.connectTransport(transportId, dtlsParameters);
        res.json({ success: true });
    } catch (error) {
        console.error('Connect transport error:', error);
        res.status(500).json({ error: error.message });
    }
});

/**
 * [POST] Medya göndermek için bir Producer oluşturur.
 */
app.post('/rooms/:roomId/peers/:userId/transports/:transportId/produce', async (req, res) => {
    const { roomId, userId, transportId } = req.params;
    const { kind, rtpParameters } = req.body;
    
    try {
        const room = rooms.get(roomId);
        if (!room) throw new Error(`Room with id "${roomId}" not found.`);
        const peer = room.getPeer(userId);
        if (!peer) throw new Error(`Peer with id "${userId}" not found.`);
        
        const producer = await peer.createProducer({ transportId, rtpParameters, kind });
        
        // Sinyalleşme (WebSocket) Notu:
        // Bu noktada, odadaki diğer tüm istemcilere yeni bir producer'ın
        // eklendiğini bir WebSocket olayı ile bildirmeniz gerekir.
        // Örn: socket.to(roomId).emit('new-producer', { userId, producerId: producer.id });
        
        res.json({ id: producer.id });
    } catch (error) {
        console.error('Create producer error:', error);
        res.status(500).json({ error: error.message });
    }
});

/**
 * [POST] Medya almak için bir Consumer oluşturur.
 */
app.post('/rooms/:roomId/peers/:userId/transports/:transportId/consume', async (req, res) => {
    const { roomId, userId, transportId } = req.params;
    const { producerId, rtpCapabilities } = req.body;
    
    try {
        const room = rooms.get(roomId);
        if (!room) throw new Error(`Room with id "${roomId}" not found.`);
        const peer = room.getPeer(userId);
        if (!peer) throw new Error(`Peer with id "${userId}" not found.`);

        if (!room.router.canConsume({ producerId, rtpCapabilities })) {
            return res.status(400).json({ error: 'Cannot consume' });
        }
        
        const consumer = await peer.createConsumer({ transportId, producerId, rtpCapabilities });

        res.json({
            id: consumer.id,
            producerId,
            kind: consumer.kind,
            rtpParameters: consumer.rtpParameters,
            type: consumer.type,
            paused: consumer.paused,
        });
    } catch (error) {
        console.error('Create consumer error:', error);
        res.status(500).json({ error: error.message });
    }
});

/**
 * [POST] Bir consumer'ı devam ettirir.
 */
app.post('/rooms/:roomId/peers/:userId/consumers/:consumerId/resume', async (req, res) => {
    const { roomId, userId, consumerId } = req.params;
    
    try {
        const room = rooms.get(roomId);
        if (!room) throw new Error(`Room with id "${roomId}" not found.`);
        const peer = room.getPeer(userId);
        if (!peer) throw new Error(`Peer with id "${userId}" not found.`);
        
        const consumer = peer.consumers.get(consumerId);
        if (!consumer) throw new Error(`Consumer with id "${consumerId}" not found.`);
        
        await consumer.resume();
        res.json({ success: true });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// --- Sunucuyu Başlatma ---
const PORT = process.env.PORT || 3000;

startMediasoup().then(() => {
    app.listen(PORT, () => {
        console.log(`Mediasoup server running on port ${PORT}`);
    });
}).catch(error => {
    console.error('Failed to start mediasoup server:', error);
    process.exit(1);
});