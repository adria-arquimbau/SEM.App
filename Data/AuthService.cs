using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SEM.App.Data;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private string semApiUrl = "https://sport-management-api.azurewebsites.net/api/";

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<LoginResponse> LogIn(string email, string password)
    {
        var jsonInString = JsonSerializer.Serialize(new
        {
            email,
            password
        });   
        
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + "Auth/Login")
        {
            Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
        };
        
        var response = await _httpClient.SendAsync(request);
        
        return (await response.Content.ReadFromJsonAsync<LoginResponse>())!;
    }

    public async Task<RegisterResponse> Register(string email, string password, object userName)
    {
        var jsonInString = JsonSerializer.Serialize(new
        {
            email,
            password,
            userName    
        });   
        
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + "Auth/Register")
        {
            Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
        };
        
        var response = await _httpClient.SendAsync(request);
        var registerResponse = (await response.Content.ReadFromJsonAsync<RegisterResponse>())!;
        registerResponse.StatusCode = response.StatusCode;
        return registerResponse;
    }
}

public class RegisterResponse
{   
    public HttpStatusCode StatusCode { get; set; }      
    public string? Message { get; set; }
    
    public RegisterResponseErrors Errors { get; set; }
}

public class RegisterResponseErrors
{
    public RegisterResponseError PasswordRequiresDigit { get; set; }
    public RegisterResponseError PasswordRequiresUpper { get; set; }
    public RegisterResponseError PasswordRequiresNonAlphanumeric { get; set; }
    public RegisterResponseError PasswordTooShort { get; set; }
}

public class RegisterResponseError
{   
    public List<DetailRegisterErrors> Errors { get; set; }  
}

public class DetailRegisterErrors
{
    public string? ErrorMessage { get; set; }
}

public class LoginResponse      
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }    
    public string? Message { get; set; }
}               