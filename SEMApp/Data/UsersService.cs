using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl +  "Users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<List<UserDto>>();
    }
    
    public async Task<bool> ConfirmEmail(string token, Guid userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, semApiUrl +  $"Users/confirm-email/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<UserDto> GetMyUser(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl +  "Users/my-user");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> SetInformation(string name, string secondName, string phone, string address, string city, string country, string postalCode, string token)
    {
        var jsonInString = JsonSerializer.Serialize(new
        {   
            name,   
            secondName,
            phone,
            address,
            city,
            country,
            postalCode
        });   
        
        var request = new HttpRequestMessage(HttpMethod.Put, semApiUrl + "Users")
        {
            Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
        };
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
    public string SecondName { get; set; }
    public string Phone { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public List<string> Roles { get; set; }
}  