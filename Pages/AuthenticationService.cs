using Blazored.LocalStorage;
using EventAppClient.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly string _tokenKey = "authToken";
    private readonly UserState _userState;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, UserState userState)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _userState = userState;
    }

    public async Task<bool> RegisterAsync(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
        return response.IsSuccessStatusCode;
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsync(_tokenKey, token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _userState.Username = loginModel.UsernameOrEmail;

            return token;
        }
        else
        {
            throw new Exception("Login failed");
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_tokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;

        _userState.Username = null;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(_tokenKey);
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordModel model)
    {
        var token = await _localStorage.GetItemAsync<string>(_tokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", model);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorObject = JsonDocument.Parse(errorContent).RootElement;
            var errorDescription = errorObject[0].GetProperty("description").GetString();
            throw new Exception(errorDescription);
        }
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<string> GetUsernameAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(_tokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null; 
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return usernameClaim?.Value;
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
