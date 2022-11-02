using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SEM.App.Data;

public class UsersService
{
    private readonly HttpClient _httpClient;
    private const string semApiUrl = "https://sport-management-api.azurewebsites.net/api/";

    public UsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<UserDto>> GetUsers(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl +  "User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<List<UserDto>>();
    }
    
    public async Task<bool> ConfirmEmail(string token, Guid userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl +  $"User/confirm-email/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}
        
public class UserDto
{   
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? RegistrationDate { get; set; }
}  