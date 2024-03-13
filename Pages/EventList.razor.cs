using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class EventListBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<Event> events;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                events = await Http.GetFromJsonAsync<List<Event>>("api/Events");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected void NavigateToCreateEvent()
        {
            NavigationManager.NavigateTo("/createevent");
        }

        protected void NavigateToUpdateEvent(int id)
        {
            NavigationManager.NavigateTo($"/updateevent/{id}");
        }

        protected void NavigateToDeleteEvent(int id)
        {
            NavigationManager.NavigateTo($"/deleteevent/{id}");
        }

    }
}
