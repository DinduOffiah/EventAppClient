using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EventAppClient.Pages
{
    public class EditEventBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Parameter] public string Id { get; set; }

        protected Event evnt = new Event();
        protected bool isSuccess;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Fetch data from API endpoints
                EventTypes = await Http.GetFromJsonAsync<List<EventType>>("api/EventType");
                TicketTypes = await Http.GetFromJsonAsync<List<TicketType>>("api/TicketType");

                // Ensure lists are not null
                if (EventTypes == null || TicketTypes == null)
                {
                    // Handle null lists (e.g., show an error message)
                    error = "Failed to load event or ticket types.";
                }
                else
                {
                    IsDataLoaded = true;
                }

                var response = await Http.GetFromJsonAsync<Event>($"api/Events/{Id}");

                if (response != null)
                {
                    evnt.EventId = int.Parse(Id);
                    evnt.EventName = response.EventName;
                    evnt.Image = response.Image;
                    evnt.EventDate = response.EventDate;
                    evnt.Location = response.Location;
                    evnt.TicketTypeId = response.TicketTypeId;
                    evnt.EventTypeId = response.EventTypeId;
                    evnt.Limit = response.Limit;
                    evnt.StartDate = response.StartDate;
                    evnt.EndDate = response.EndDate;
                    evnt.Description = response.Description;
                }
                else
                {
                    error = "Event not found.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

        protected async Task UpdateEvent()
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/Events/{evnt.EventId}", evnt);

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                    NavigationManager.NavigateTo("/eventlist");
                }
                else
                {
                    error = "Failed to update event. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

        // Method to handle file upload
        protected async Task HandleFileUpload(InputFileChangeEventArgs e)
        {
            var imageFile = e.File;

            if (imageFile != null)
            {
                using var memoryStream = new MemoryStream();
                await imageFile.OpenReadStream().CopyToAsync(memoryStream);
                evnt.Image = memoryStream.ToArray();
            }
        }

        //Selecting TicketTypeNames and EventTypeNames instead of Id
        protected List<EventType> EventTypes { get; set; } = new List<EventType>();
        protected List<TicketType> TicketTypes { get; set; } = new List<TicketType>();
        protected bool IsDataLoaded { get; set; } = false;

        protected void ToggleMultiDay(ChangeEventArgs e)
        {
            evnt.IsMultiDay = (bool)e.Value;
        }
    }

}
