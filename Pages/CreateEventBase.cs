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
                    // If not authenticated, redirect to login with ReturnUrl set to this page
                    var currentUrl = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).PathAndQuery;
                    NavigationManager.NavigateTo($"/login?returnUrl={currentUrl}");
                    return;
                }

                // Fetch data for event types and ticket types
                EventTypes = await Http.GetFromJsonAsync<List<EventType>>("api/EventType");
                TicketTypes = await Http.GetFromJsonAsync<List<TicketType>>("api/TicketType");

                if (EventTypes == null || TicketTypes == null)
                {
                    ErrorMessage = "Failed to load event or ticket types.";
                }
                else
                {
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

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

        protected async Task CreateEvent()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/Events", Event);

                if (response.IsSuccessStatusCode)
                {
                    // Navigate to the event list page on success
                    NavigationManager.NavigateTo("/eventlist");
                }
                else
                {
                    ErrorMessage = "Failed to create event. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        protected void ToggleMultiDay(ChangeEventArgs e)
        {
            Event.IsMultiDay = (bool)e.Value;
        }
    }
}
