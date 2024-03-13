using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text;

namespace EventAppClient.Pages
{
    public class CreateEventBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected Event Event { get; set; } = new Event();
        protected string ErrorMessage { get; set; }

        // Method to handle file upload
        protected async Task HandleFileUpload(InputFileChangeEventArgs e)
        {
            var imageFile = e.File;

            if (imageFile != null)
            {
                using var memoryStream = new MemoryStream();
                await imageFile.OpenReadStream().CopyToAsync(memoryStream);
                Event.Image = memoryStream.ToArray();
            }
        }

        // Method to create the event
        protected async Task CreateEvent()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/Events", Event);

                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    NavigationManager.NavigateTo("/eventlist");
                }
                else
                {
                    // Handle failure
                    ErrorMessage = "Failed to create event. Please try again.";
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
