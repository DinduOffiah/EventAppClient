using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using System.Net.Http.Json;

namespace EventAppClient.Pages
{
    public class DeleteEventTypeBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationService AuthenticationService { get; set; }
        [Parameter] public string Id { get; set; }

        protected EventType eventType;
        protected string error;

        protected override async Task OnInitializedAsync()
        {
            try
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
                    eventType = response;
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

        protected async Task ConfirmDelete()
        {
            try
            {
                var response = await Http.DeleteAsync($"api/EventType/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    NavigationManager.NavigateTo("/eventtypelist");
                }
                else
                {
                    error = "Failed to delete event type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }


        protected void CancelDelete()
        {
            NavigationManager.NavigateTo("/eventtypelist");
        }
    }
}
