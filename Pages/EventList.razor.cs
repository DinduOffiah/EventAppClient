using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
        protected List<Event> ongoingEvents;
        protected List<Event> upcomingEvents;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                events = await Http.GetFromJsonAsync<List<Event>>("api/Events");
                events = events.OrderByDescending(e => e.EventId).ToList();

                // Get the current date
                var currentDate = DateTime.Now;

                // Filter the events into ongoing and upcoming
                ongoingEvents = events.Where(e => e.StartDate <= currentDate).ToList();
                upcomingEvents = events.Where(e => e.StartDate > currentDate).ToList();
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

        //This for link sharing
        protected string GetEventDetailsUrl(int eventId)
        {
            return $"/detailevent/{eventId}";
        }

        protected string searchTerm = "";
        protected async Task SearchEvents()
        {
            events = await Http.GetFromJsonAsync<List<Event>>($"api/Events?query={searchTerm}");

            events = events.OrderByDescending(e => e.EventId).ToList();
        }
    }
}
