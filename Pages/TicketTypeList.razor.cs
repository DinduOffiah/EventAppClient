using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class TicketTypeListBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<TicketType> ticketTypes;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ticketTypes = await Http.GetFromJsonAsync<List<TicketType>>("api/TicketType");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected void NavigateToCreateTicketType()
        {
            NavigationManager.NavigateTo("/createtickettype");
        }

        protected void NavigateToUpdateTicketType(int id)
        {
            NavigationManager.NavigateTo($"/updatetickettype/{id}");
        }

        protected void NavigateToDeleteTicketType(int id)
        {
            NavigationManager.NavigateTo($"/deletetickettype/{id}");
        }

    }
}
