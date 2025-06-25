using BKCordServer.Modules.Identity.HttpApi.Client.DTOs;
using BKCordServer.Modules.Identity.HttpApi.Client.Interfaces;
using System.Net.Http.Json;

namespace BKCordServer.Modules.Identity.HttpApi.Client.Implementation;
public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IdentityClientOptions _options;

    public IdentityApiClient(HttpClient httpClient, IOptions<IdentityClientOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task RegisterAsync(string email, string userName, string password)
    {
        var dto = new RegisterDto(email, userName, password);

        var response = await _httpClient.PostAsJsonAsync("/identity/auth/register", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Register failed: {error}");
        }
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var response = await _httpClient.GetAsync($"/identity/user/getByEmail");

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}