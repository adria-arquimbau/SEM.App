using System.Text.Json.Serialization;

namespace SEM.App.Data.Models;

public class ManagerDto
{
    public Guid Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ManagerRole Role { get; set; }
    public UserDto User { get; set; }
    public bool Requesting { get; set; }
}

public enum ManagerRole
{
    CheckIn,
    Basic,
    Admin,
    Owner
}