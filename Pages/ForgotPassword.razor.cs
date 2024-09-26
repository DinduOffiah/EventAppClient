using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class ForgotPasswordBase : ComponentBase
    {
        protected ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
        protected string errorMessage;
        protected string successMessage;

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        protected async Task OnForgotPassword()
        {
            try
            {
                var result = await AuthenticationService.ForgotPasswordAsync(forgotPasswordModel.Email);

                if (result)
                {
                    successMessage = "Password reset token has been sent to your email.";
                }
                else
                {
                    errorMessage = "Failed to send password reset token. Please check your email.";
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
