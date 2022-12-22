namespace SEM.App.Data;

public static class ApiService
{
    private const string semApiUrl = "https://sport-management-api.azurewebsites.net/";
    private const string localhostUrl = "https://localhost:5001/";

    public static string GetBaseApiCallUrl()
    {
        return  semApiUrl + "api/";   
    }
    
    public static string GetBaseUrl()
    {
        return semApiUrl;      
    }
}
