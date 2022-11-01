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
        var response = await _httpClient.GetAsync(semApiUrl + "Event");
        return await response.Content.ReadFromJsonAsync<List<SportEvent>>();
    }
    
    public async Task<bool> CreateEvent(string name, string description, string token)
    {
        var jsonInString = JsonSerializer.Serialize(new
        {   
            name,
            description
        });   
        
        var request = new HttpRequestMessage(HttpMethod.Post, semApiUrl + "Event")
        {
            Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<SportEvent> GetEvent(Guid id)
    {
        var response = await _httpClient.GetAsync(semApiUrl + $"Event/{id}");
        return await response.Content.ReadFromJsonAsync<SportEvent>();
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
    
public class SportEvent 
{
    public Guid Id { get; set; }
    public string Name { get; set; }    
    public string Description { get; set; }
    public string CreatorNickName { get; set; }
    public DateTime CreationDate { get; set; }
}               