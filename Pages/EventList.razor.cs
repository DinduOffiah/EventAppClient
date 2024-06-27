using EventAppClient.Helpers;
using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class EventListBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected List<Event> events;
        protected List<Event> ongoingEvents;
        protected List<Event> upcomingEvents;
        protected string searchTerm = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadEvents();
        }

        private async Task LoadEvents()
        {
            try
            {
                events = await Http.GetFromJsonAsync<List<Event>>("api/Events");
                events = events.OrderByDescending(e => e.EventId).ToList();

                var currentDate = DateTime.Now;

                ongoingEvents = events.Where(e => (e.StartDate <= currentDate) || (e.EventDate <= currentDate)).ToList();
                upcomingEvents = events.Where(e => (e.StartDate != null && e.StartDate > currentDate) || (e.EventDate != null && e.EventDate > currentDate)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected async Task SearchEvents()
        {
            events = await Http.GetFromJsonAsync<List<Event>>($"api/Events?query={searchTerm}");
            events = events.OrderByDescending(e => e.EventId).ToList();
        }

        protected void NavigateToCreateEvent() => NavigationManager.NavigateTo("/createevent");

        protected void NavigateToUpdateEvent(int id) => NavigationManager.NavigateTo($"/updateevent/{id}");

        protected void NavigateToDeleteEvent(int id) => NavigationManager.NavigateTo($"/deleteevent/{id}");

        protected void NavigateToDetailEvent(int id) => NavigationManager.NavigateTo($"/detailevent/{id}");

        protected string GetEventDetailsUrl(int eventId) => $"/detailevent/{eventId}";

        protected async Task BuyTicket(Event evnt)
        {
            try
            {
                var response = await Http.PostAsync("/api/Payment/CreateCheckoutSession",
                    new StringContent(JsonConvert.SerializeObject(evnt), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                var sessionId = sessionData["sessionId"];

                await JsInterop.RedirectToCheckout(JsRuntime, "pk_test_51PV94HP93VNiDzg2xmOIPLeGULmr7EYaniwNdBkiXo9OzU9lSwvXctv2d6W4SEGInEYGhJ3SVYq13uaDySIHBFSm00lJQHcTsv", sessionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating checkout session: {ex.Message}");
            }
        }
    }
}
