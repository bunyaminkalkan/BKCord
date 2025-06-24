namespace BKCordServer.Modules.Identity.HttpApi.Client.DTOs;

public record RegisterDto(string Email, string UserName, string Password);
