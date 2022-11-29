using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SEM.App.Extensions;

namespace SEM.App.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageServiceService)
    {
        _localStorageService = localStorageServiceService;    
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await _localStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");
            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
            
            if (userSession.ExpiresIn == 0)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
            
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, userSession.UserName),
                new(ClaimTypes.Role, userSession.Role)
            }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, userSession.UserName),
                new(ClaimTypes.Role, userSession.Role)
            }));
            userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
            await _localStorageService.SaveItemEncryptedAsync("UserSession", userSession);
        }
        else
        {
            claimsPrincipal = _anonymous;
            await _localStorageService.RemoveItemAsync("UserSession");
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    
    public async Task<string> GetToken()
    {
        var result = string.Empty;

        try
        {
            var userSession = await _localStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");
            if (DateTime.Now < userSession.ExpiryTimeStamp)
            {
                result = userSession.Token;
            }
        }   
        catch
        {
            // ignored
        }

        return result;
    }
}

public class UserSession
{
    public string UserName { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiryTimeStamp { get; set; }
}