using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
                events = events.OrderByDescending(e => e.EventId).ToList();
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

        protected void NavigateToDetailEvent(int id)
        {
            NavigationManager.NavigateTo($"/detailevent/{id}");
        }

        protected string GetEventDetailsUrl(int eventId)
        {
            // Construct the URL for the event details page based on the event ID
            return $"/detailevent/{eventId}";
        }
    }
}
