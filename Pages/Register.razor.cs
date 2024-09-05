using Microsoft.AspNetCore.Components;

namespace EventAppClient.Pages
{
    public class RegisterBase : ComponentBase
    {
        protected RegisterModel registerModel = new RegisterModel();
        protected string errorMessage;
        protected string successMessage;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        protected async Task OnRegister()
        {
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                errorMessage = "Passwords do not match.";
                return;
            }

            var success = await AuthenticationService.RegisterAsync(registerModel);
            if (success)
            {
                // Handle successful registration
                successMessage = "Registration successful! Redirecting to login...";
                NavigationManager.NavigateTo("/login"); // Redirect to login page
            }
            else
            {
                // Handle registration failure
                errorMessage = "Registration failed: Username may already be taken.";
            }
        }
    }
}
