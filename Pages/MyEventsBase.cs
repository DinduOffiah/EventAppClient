using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class MyEventsBase : ComponentBase
    {
        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        protected List<Event> events;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserEvents();
        }

        private async Task LoadUserEvents()
        {
            try
            {
                var userId = await AuthenticationService.GetUsernameAsync();
                events = await Http.GetFromJsonAsync<List<Event>>("api/Events/my-events");
                events = events.Where(e => e.CreatedBy == userId).OrderByDescending(e => e.EventId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

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
