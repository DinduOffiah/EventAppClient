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

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

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

        //Selecting TicketTypeNames and EventTypeNames instead of Id
        protected List<EventType> EventTypes { get; set; } = new List<EventType>();
        protected List<TicketType> TicketTypes { get; set; } = new List<TicketType>();
        protected bool IsDataLoaded { get; set; } = false;

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

                // Fetch data from API endpoints
                EventTypes = await Http.GetFromJsonAsync<List<EventType>>("api/EventType");
                TicketTypes = await Http.GetFromJsonAsync<List<TicketType>>("api/TicketType");

                // Ensure lists are not null
                if (EventTypes == null || TicketTypes == null)
                {
                    // Handle null lists (e.g., show an error message)
                    ErrorMessage = "Failed to load event or ticket types.";
                }
                else
                {
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log or display an error message)
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        protected void ToggleMultiDay(ChangeEventArgs e)
        {
            Event.IsMultiDay = (bool)e.Value;
        }


    }
}
