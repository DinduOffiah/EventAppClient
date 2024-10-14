using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Pages;
using Microsoft.AspNetCore.Components.Forms;

namespace EventAppClient.Pages
{
    public class UpdateEventTypeBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationService AuthenticationService { get; set; }

        [Parameter] public string Id { get; set; }

        protected EventType eventType = new EventType();
        protected bool isSuccess;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            var token = await AuthenticationService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                // If not authenticated, redirect to login page
                NavigationManager.NavigateTo("/login");
                return;
            }

            var userDetails = await AuthenticationService.GetUserDetailsAsync();
            if (userDetails.Role != "Admin")
            {
                // If not an admin, redirect to access denied page
                NavigationManager.NavigateTo("/accessdenied");
                return;
            }

            var response = await Http.GetFromJsonAsync<EventType>($"api/EventType/{Id}");

            if (response != null)
            {
                eventType.EventTypeId = int.Parse(Id);
                eventType.EventTypeName = response.EventTypeName;
            }
        }

        protected async Task UpdateEventType()
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/EventType/{eventType.EventTypeId}", eventType);

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                    NavigationManager.NavigateTo("/eventtypelist");
                }
                else
                {
                    error = "Failed to update event type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

    }
}
