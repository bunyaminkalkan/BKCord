﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BKCordServer.IdentityModule.Options;

public sealed class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string Jwt = nameof(Jwt);
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(Jwt).Bind(options);
    }
}
