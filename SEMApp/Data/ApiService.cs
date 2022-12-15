namespace SEM.App.Data;

public static class ApiService
{
    private const string semApiUrl = "https://sport-management-api.azurewebsites.net/api/";
    private const string localhostUrl = "https://localhost:5001/api/";

    public static string GetBaseUrl()
    {
        return semApiUrl;   
    }
}
