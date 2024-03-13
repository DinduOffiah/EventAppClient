using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class EventTypeListBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<EventType> eventTypes;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                eventTypes = await Http.GetFromJsonAsync<List<EventType>>("api/EventType");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected void NavigateToCreateEventType()
        {
            NavigationManager.NavigateTo("/createeventtype");
        }

        protected void NavigateToUpdateEventType(int id)
        {
            NavigationManager.NavigateTo($"/updateeventtype/{id}");
        }

        protected void NavigateToDeleteEventType(int id)
        {
            NavigationManager.NavigateTo($"/deleteeventtype/{id}");
        }

    }
}
