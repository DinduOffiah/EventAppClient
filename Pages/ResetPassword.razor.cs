using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class ResetPasswordBase : ComponentBase
    {
        protected ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
        protected string errorMessage;
        protected string successMessage;

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async Task OnResetPassword()
        {
            try
            {
                var result = await AuthenticationService.ResetPasswordAsync(resetPasswordModel);

                if (result)
                {
                    successMessage = "Password has been reset successfully!";
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    errorMessage = "Password reset failed. Please check your token and try again.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
