using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class ChangePasswordBase : ComponentBase
    {
        protected ChangePasswordModel changePasswordModel = new ChangePasswordModel();
        protected string errorMessage;
        protected string successMessage;

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async Task OnChangePassword()
        {
            try
            {
                var result = await AuthenticationService.ChangePasswordAsync(changePasswordModel);

                if (result)
                {
                    successMessage = "Password changed successfully!";
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    errorMessage = "Password change failed. Please check your current password.";
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
