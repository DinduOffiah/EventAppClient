using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using Microsoft.JSInterop;

namespace EventAppClient.Pages
{
    public class DetailEventBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Parameter] public string Id { get; set; }

        protected Event evnt = new Event();
        protected string error;
        protected System.Timers.Timer timer;
        protected bool isLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Http.GetFromJsonAsync<Event>($"api/Events/{Id}");

                if (response != null)
                {
                    evnt.EventId = int.Parse(Id);
                    evnt.EventName = response.EventName;
                    evnt.Image = response.Image;
                    evnt.EventDate = response.EventDate;
                    evnt.StartDate = response.StartDate;
                    evnt.EndDate = response.EndDate;
                    evnt.Description = response.Description;
                    evnt.Location = response.Location;
                    evnt.TicketTypeId = response.TicketTypeId;
                    evnt.EventTypeId = response.EventTypeId;
                    evnt.Limit = response.Limit;
                    evnt.EventTypeName = response.EventTypeName;
                    evnt.TicketTypeName = response.TicketTypeName;
                    isLoaded = true;
                }

                else
                {
                    error = "Event not found.";
                }

                StartCountdown(evnt);

            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }


        private void StartCountdown(Event evnt)
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) => UpdateCountdown(evnt);
            timer.Start();
        }

        private void UpdateCountdown(Event evnt)
        {
            var now = DateTime.Now;
            var eventDate = evnt.StartDate ?? evnt.EventDate ?? now;

            if (now >= eventDate)
            {
                evnt.IsOngoing = true;
                evnt.Countdown = "Ongoing";
            }
            else
            {
                var timeRemaining = eventDate - now;
                evnt.Countdown = $"{timeRemaining.Days}d {timeRemaining.Hours}h {timeRemaining.Minutes}m {timeRemaining.Seconds}s";
            }

            InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            timer?.Dispose();
        }

        protected string GetEventDetailsUrl(int eventId)
        {
            return $"/detailevent/{eventId}";
        }

        protected void GoBack()
        {
            JSRuntime.InvokeVoidAsync("window.history.back");
        }
    }
}
