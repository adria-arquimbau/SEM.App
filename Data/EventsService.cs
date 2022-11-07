using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SEM.App.Data;

public class EventsService
{
    private readonly HttpClient _httpClient;
    private const string semApiUrl = "https://sport-management-api.azurewebsites.net/api/";

    public EventsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<SportEvent>> GetEvents()
    {
       return await _httpClient.GetFromJsonAsync<List<SportEvent>>(semApiUrl + "Event");
    }
    
    public async Task<List<RegistrationDto>> GetRegistrationsByEventId(Guid eventId, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl + $"Event/{eventId}/registrations");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadFromJsonAsync<List<RegistrationDto>>();        
    }   
    
    public async Task<List<SportEvent>> GetMyRegisteredEvents(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl + $"Event/Registered");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadFromJsonAsync<List<SportEvent>>();        
    }
        
    public async Task<SportEvent> GetEvent(Guid eventId)
    {
        var response = await _httpClient.GetAsync(semApiUrl + $"Event/{eventId}");
        return await response.Content.ReadFromJsonAsync<SportEvent>();
    }   
    
    public async Task<bool> CreateEvent(string name, string description, string location, int maxRegistrations, string token)
    {
        var jsonInString = JsonSerializer.Serialize(new
        {       
            name,
            description,
            location,
            maxRegistrations
        });   
        
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + "Event")
        {
            Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<RegisterToAnEventResponse> RegisterToAnEvent(string token, Guid eventId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + $"Event/{eventId}/register");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadFromJsonAsync<RegisterToAnEventResponse>();
            responseMessage.SuccessRegistration = true;
            return responseMessage;
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var responseMessage = await response.Content.ReadFromJsonAsync<RegisterToAnEventResponse>();
            responseMessage.SuccessRegistration = false;
            return responseMessage;
        }
        
        return new RegisterToAnEventResponse
        {
            Message = "Something went wrong, try it later please.",
            SuccessRegistration = false
        };
    }   
    
    public async Task<string> UnRegisterToAnEvent(string token, Guid eventId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + $"Event/{eventId}/unregister");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.SendAsync(request);
        
        return response.IsSuccessStatusCode ? "Unregistered" : "Something went wrong, try it later";
    }
    
    public async Task<IAmRegisteredResponse> IAmRegistered(Guid eventId, string token)  
    {
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl + $"Event/{eventId}/user-registered");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<IAmRegisteredResponse>();
    }   
    
    public async Task<List<SportEvent>> GetMyEventsAsOrganizer(string token)
    {   
        var request = new HttpRequestMessage(HttpMethod.Get, semApiUrl +  "Event/organizer");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<List<SportEvent>>();
    }
    
    public async Task<bool> DeleteEvent(Guid id, string token)   
    {   
        var request = new HttpRequestMessage(HttpMethod.Delete, semApiUrl +  $"Event/{id}/delete");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}

public class IAmRegisteredResponse
{
    public bool Registered { get; set; }
}

public class RegisterToAnEventResponse
{
    public string? Message { get; set; }
    public bool SuccessRegistration { get; set; }
}
    
public class SportEvent 
{   
    public Guid Id { get; set; }
    public string Name { get; set; }    
    public string Description { get; set; }
    public string CreatorNickName { get; set; }
    public DateTime CreationDate { get; set; }
    public int RegistrationsQuantity { get; set; }
    public string Location { get; set; }
    public int MaxRegistrations { get; set; }
}     

public class RegistrationDto
{
    public string UserName { get; set; }
    public RegistrationRole Role { get; set; }
}

public enum RegistrationRole
{   
    Staff,
    Rider,
    Marshal,    
    RiderMarshal
}            