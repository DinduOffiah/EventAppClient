using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
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

                foreach (var evnt in events)
                {
                    if (evnt.Limit.HasValue && evnt.Limit.Value <= 0)
                    {
                        evnt.TicketTypeName = "Sold Out";
                    }
                }
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
                var stripeService = new StripeService(); // Initialize StripeService
                var session = await stripeService.CreateCheckoutSession(evnt);

                // Redirect to Stripe Checkout
                NavigationManager.NavigateTo(session.Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating checkout session: {ex.Message}");
            }
        }
    }
}
