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
    }

}
