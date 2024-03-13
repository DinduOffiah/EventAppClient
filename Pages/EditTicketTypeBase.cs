using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EventAppClient.Pages
{
    public class UpdateTicketTypeBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; } // Inject NavigationManager
        [Parameter] public string Id { get; set; }

        protected TicketType ticketType = new TicketType();
        protected bool isSuccess;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetFromJsonAsync<TicketType>($"api/TicketType/{Id}");

            if (response != null)
            {
                ticketType.TicketTypeId = int.Parse(Id);
                ticketType.TicketTypeName = response.TicketTypeName;
            }
        }

        protected async Task UpdateTicketType()
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/TicketType/{ticketType.TicketTypeId}", ticketType);

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                    NavigationManager.NavigateTo("/tickettypelist");
                }
                else
                {
                    error = "Failed to update ticket type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

    }
}

