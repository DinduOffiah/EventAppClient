using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class DeleteEventBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Parameter] public string Id { get; set; }

        protected Event eventItem;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Http.GetFromJsonAsync<Event>($"api/Events/{Id}");

                if (response != null)
                {
                    eventItem = response;
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

        protected async Task ConfirmDelete()
        {
            try
            {
                var response = await Http.DeleteAsync($"api/Events/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    NavigationManager.NavigateTo("/eventlist");
                }
                else
                {
                    error = "Failed to delete event. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

        protected void CancelDelete()
        {
            NavigationManager.NavigateTo("/eventlist");
        }
    }
}
