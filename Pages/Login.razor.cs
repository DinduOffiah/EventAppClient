using Microsoft.AspNetCore.Components;

namespace EventAppClient.Pages
{
    public class LoginBase : ComponentBase
    {
        protected LoginModel loginModel = new LoginModel();
        protected string errorMessage;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        protected async Task OnLogin()
        {
            try
            {
                var token = await AuthenticationService.LoginAsync(loginModel);
                // Handle successful login
                NavigationManager.NavigateTo("/"); // Redirect to homepage or dashboard
            }
            catch (Exception ex)
            {
                // Handle login errors
                errorMessage = "Login failed: Invalid username or password.";
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }
    }
}
