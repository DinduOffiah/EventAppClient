using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EventAppClient.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EventAppClient.Pages
{
    public class UpdateTicketTypeBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationService AuthenticationService { get; set; }
        [Parameter] public string Id { get; set; }

        protected TicketType ticketType = new TicketType();
        protected bool isSuccess;
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
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log or display an error message)
                error = $"An error occurred: {ex.Message}";
            }
        }

        protected async Task UpdateTicketType()
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/TicketType/{ticketType.TicketTypeId}", ticketType);

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                    NavigationManager.NavigateTo("/tickettypelist");
                }
                else
                {
                    error = "Failed to update ticket type. Please try again.";
                }
            }
            catch (Exception ex)
            {
                error = $"An error occurred: {ex.Message}";
            }
        }

    }
}

