using System.Text.Json;
using Microsoft.JSInterop;

namespace EVS.App.Presentation.Services;

/// <summary>
/// Provides methods for interacting with the browser's Local Storage using JavaScript interop.
/// </summary>
/// <param name="jsRuntime">An instance of <see cref="IJSRuntime"/> for executing JavaScript functions.</param>
/// <param name="logger">An instance of <see cref="ILogger{LocalStorageService}"/> for logging errors.</param>
public class LocalStorageService(IJSRuntime jsRuntime,
    ILogger<LocalStorageService> logger)
{
    /// <summary>
    /// Stores a value in the browser's Local Storage.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to be stored.</typeparam>
    /// <param name="key">The key under which the value will be stored.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
    public async Task SetValue<TValue>(string key, TValue value)
    {
        if(string.IsNullOrEmpty(key)) 
            throw new ArgumentNullException(nameof(key), "Key cannot be empty.");

        
        try
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        catch (Exception e)
        {
           logger.LogError("Cannot save value to local storage - {Key}", key);
        }
    }

    /// <summary>
    /// Retrieves a value from the browser's Local Storage.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to be retrieved.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <returns>
    /// The retrieved value of type <typeparamref name="TValue"/>, or <c>default</c> if retrieval fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
    public async Task<TValue?> GetValueAsync<TValue>(string key)
    {
        if(string.IsNullOrEmpty(key)) 
            throw new ArgumentNullException(nameof(key), "Key cannot be empty.");
        
        try
        {
            var value = await jsRuntime.InvokeAsync<TValue>("localStorage.getItem", key);
            logger.LogError("Retireved info {Value}", value);
            return value;
        }
        catch (Exception e)
        {
            logger.LogError("Cannot get value from local storage - {Key} due to {Exception}", key, e);
            return default;
        }
    }
}