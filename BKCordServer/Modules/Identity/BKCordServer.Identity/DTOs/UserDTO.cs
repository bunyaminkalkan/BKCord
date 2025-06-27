using BKCordServer.Identity.Domain.Entities;

namespace BKCordServer.Identity.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public string AvatarUrl { get; set; }

    public UserDTO(User user)
    {
        Id = user.Id;
        Email = user.Email;
        UserName = user.UserName;
        Name = user.Name;
        Middlename = user.Middlename;
        Surname = user.Surname;
        AvatarUrl = user.AvatarUrl;
    }
}
