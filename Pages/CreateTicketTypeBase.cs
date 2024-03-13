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

        protected TicketType TicketType { get; set; } = new TicketType();

        protected string ErrorMessage { get; set; }

        protected async Task CreateTicketType()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/TicketType", TicketType);

                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    // For example, navigate to another page
                    NavigationManager.NavigateTo("/tickettypelist");
                }
                else
                {
                    // Handle failure
                    // For example, show an error message
                    ErrorMessage = "Failed to create ticket type. Please try again.";
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

