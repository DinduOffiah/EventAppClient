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

        protected EventType EventType { get; set; } = new EventType();

        protected string ErrorMessage { get; set; }

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
