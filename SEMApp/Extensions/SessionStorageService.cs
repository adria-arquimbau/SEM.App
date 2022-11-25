using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

namespace SEM.App.Extensions;

public static class SessionStorageService
{
    public static async Task SaveItemEncryptedAsync<T>(this ILocalStorageService sessionStorageService, string key, T item)
    {
        var itemJson = JsonSerializer. Serialize(item);
        var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
        var base64Json = Convert.ToBase64String(itemJsonBytes);
        await sessionStorageService.SetItemAsync(key, base64Json);
    }

    public static async Task<T> ReadEncryptedItemAsync<T>(this ILocalStorageService sessionStorageService, string key)
    {
        var base64Json = await sessionStorageService.GetItemAsync<string>(key);
        var itemJsonBytes = Convert.FromBase64String(base64Json);
        var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
        var item = JsonSerializer.Deserialize<T>(itemJson);
        return item;
    }
}