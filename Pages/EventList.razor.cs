﻿using EventAppClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Stripe;
using Stripe.Checkout;
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

        [Inject] 
        IJSRuntime JSRuntime { get; set; }

        protected List<Event> events;
        protected List<Event> ongoingEvents;
        protected List<Event> upcomingEvents;

        private string stripeSecretKey = "sk_test_51PV94HP93VNiDzg2VPX0gnU5JLEHtFcL24cNWSNiwhcox8uvW7kvBM4PigSlKwFXiWpkYME4zJzv57ZGqwDtPjNi00H6EhXxnY";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                events = await Http.GetFromJsonAsync<List<Event>>("api/Events");
                events = events.OrderByDescending(e => e.EventId).ToList();

                // Get the current date
                var currentDate = DateTime.Now;

                // Filter the events into ongoing and upcoming
                ongoingEvents = events.Where(e => (e.StartDate <= currentDate) || (e.EventDate <= currentDate)).ToList();
                upcomingEvents = events.Where(e => (e.StartDate != null && e.StartDate > currentDate) || (e.EventDate != null && e.EventDate > currentDate)).ToList();

                // Initialize Stripe 
                await JSRuntime.InvokeVoidAsync("initializeStripe", stripeSecretKey);
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

        protected async Task BuyTicket(Event evnt)
        {
            StripeConfiguration.ApiKey = "sk_test_51PV94HP93VNiDzg2VPX0gnU5JLEHtFcL24cNWSNiwhcox8uvW7kvBM4PigSlKwFXiWpkYME4zJzv57ZGqwDtPjNi00H6EhXxnY";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(evnt.TicketPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = evnt.EventName,
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = NavigationManager.ToAbsoluteUri("/paymentsuccessful").ToString(),
                CancelUrl = NavigationManager.ToAbsoluteUri("/paymentcancelled").ToString(),
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // Ensure the function exists before calling it
            await JSRuntime.InvokeVoidAsync("eval", "if (typeof redirectToCheckout !== 'undefined') { redirectToCheckout('" + session.Id + "'); } else { console.error('redirectToCheckout is not defined'); }");
        }



    }
}
