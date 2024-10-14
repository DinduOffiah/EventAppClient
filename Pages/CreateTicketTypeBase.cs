using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class CreateTicketTypeBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationService AuthenticationService { get; set; } 

        protected TicketType TicketType { get; set; } = new TicketType();
        protected string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {

            try
            {
                var token = await AuthenticationService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    // If not authenticated, redirect to login page
                    NavigationManager.NavigateTo("/login");
                    return;
                }

                var userDetails = await AuthenticationService.GetUserDetailsAsync();
                if (userDetails.Role != "Admin")
                {
                    // If not an admin, redirect to access denied page
                    NavigationManager.NavigateTo("/accessdenied");
                    return;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log or display an error message)
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        protected async Task CreateTicketType()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/TicketType", TicketType);
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    NavigationManager.NavigateTo("/tickettypelist");
                }
                else
                {
                    // Handle failure
                    ErrorMessage = "Failed to create ticket type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }
    }
}
