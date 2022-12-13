using System.Text.Json.Serialization;

namespace SEM.App.Data.Models;

public class ManagersDto
{
    public Guid Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ManagerRole Role { get; set; }
    public UserDto User { get; set; }
}

public enum ManagerRole
{
    Basic,
    CheckIn,
    Admin,
    Owner
}