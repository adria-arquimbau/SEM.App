using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SEM.App.Data;

public class EventsService
{
    private readonly HttpClient _httpClient;

    public EventsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<SportEvent>> GetEvents()
    {
       return await _httpClient.GetFromJsonAsync<List<SportEvent>>(ApiService.GetBaseUrl() + "Events");
    }

    public async Task<SportEvent> GetEvent(Guid eventId)
    {
        var response = await _httpClient.GetAsync(ApiService.GetBaseUrl() + $"Events/{eventId}");
        return await response.Content.ReadFromJsonAsync<SportEvent>();
    } 
    
    public async Task<RegisterToAnEventResponse> RegisterToAnEvent(string token, Guid eventId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiService.GetBaseUrl() + $"Events/{eventId}/register");
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

    public async Task<List<SportEventOrganizer>> GetMyEventsAsOrganizer(string token)
    {   
        var request = new HttpRequestMessage(HttpMethod.Get, ApiService.GetBaseUrl() +  "Events/organizer");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        
        return await response.Content.ReadFromJsonAsync<List<SportEventOrganizer>>();
    }
}

public class IAmRegisteredResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegistrationState RegistrationState { get; set; }
}

public class RegisterToAnEventResponse
{
    public string? Message { get; set; }
    public bool SuccessRegistration { get; set; }
}   
    
public class SportEvent 
{   
    public Guid Id { get; set; }
    public Guid CreatorUserId { get; set; }
    public string Name { get; set; }    
    public string Description { get; set; } 
    public string CreatorNickName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int RegistrationsQuantity { get; set; }
    public string Location { get; set; }
    public int MaxRegistrations { get; set; }
    public DateTime OpenRegistrationsDateTime { get; set; }
    public DateTime CloseRegistrationsDateTime { get; set; }
    public Uri ImageUrl { get; set; }
}

public class SportEventOrganizer
{   
    public Guid Id { get; set; }
    public string Name { get; set; }        
    public string Description { get; set; }
    public string CreatorNickName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int TotalRegistrationsQuantity { get; set; }  
    public int AcceptedQuantity { get; set; }  
    public int PreRegisteredQuantity { get; set; }  
    public int CancelledQuantity { get; set; }  
    public string Location { get; set; }
    public int MaxRegistrations { get; set; }   
    public bool IsPublic { get; set; }
    public DateTime OpenRegistrationsDateTime { get; set; }
    public DateTime CloseRegistrationsDateTime { get; set; }    
    public List<RegistrationDto> Registrations { get; set; }
    public Uri ImageUrl { get; set; }
}     

public class RegistrationDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegistrationRole Role { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegistrationState State { get; set; }
    public int? Bib { get; set; }
    public bool RequestingChangeStatus { get; set; }
    public bool RequestingUpdateBib { get; set; }   
    public bool CheckedIn { get; set; }   
}

public enum RegistrationRole    
{   
    Staff,
    Rider,
    Marshal,    
    RiderMarshal
}     

public enum RegistrationState
{
    NonRegistered,
    PreRegistered,
    Accepted,
    Cancelled
}             