using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class CreateEventTypeBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        protected EventType EventType { get; set; } = new EventType();

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

        protected async Task CreateEventType()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/EventType", EventType);

                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    // For example, navigate to another page
                    NavigationManager.NavigateTo("/eventtypelist");
                }
                else
                {
                    // Handle failure
                    // For example, show an error message
                    ErrorMessage = "Failed to create event type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                // For example, show an error message
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }
    }
}
