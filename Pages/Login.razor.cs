using Microsoft.AspNetCore.Components;

namespace EventAppClient.Pages
{
    public class LoginBase : ComponentBase
    {
        protected LoginModel loginModel = new LoginModel();
        protected string errorMessage;
        private string returnUrl;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        protected override void OnInitialized()
        {
            // Extract the returnUrl from the query string
            var uri = new Uri(NavigationManager.Uri);
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("returnUrl", out var extractedReturnUrl))
            {
                returnUrl = extractedReturnUrl;
            }
        }

        protected async Task OnLogin()
        {
            try
            {
                var token = await AuthenticationService.LoginAsync(loginModel);

                // Redirect to the ReturnUrl if available; otherwise, redirect to homepage
                NavigationManager.NavigateTo(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
            }
            catch (Exception ex)
            {
                errorMessage = "Login failed: Invalid username or password.";
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }
    }
}
