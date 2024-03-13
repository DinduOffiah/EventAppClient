using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class DeleteTicketTypeBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Parameter] public string Id { get; set; }

        protected TicketType ticketType;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Http.GetFromJsonAsync<TicketType>($"api/TicketType/{Id}");

                if (response != null)
                {
                    ticketType = response;
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
                var response = await Http.DeleteAsync($"api/TicketType/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    NavigationManager.NavigateTo("/tickettypelist");
                }
                else
                {
                    error = "Failed to delete ticket type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }


        protected void CancelDelete()
        {
            NavigationManager.NavigateTo("/tickettypelist");
        }
    }
}
