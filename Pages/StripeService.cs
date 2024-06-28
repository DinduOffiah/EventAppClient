using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventAppClient.Pages
{
    public class StripeService
    {
        private readonly string _stripeSectretKey = "sk_test_51PV94HP93VNiDzg2VPX0gnU5JLEHtFcL24cNWSNiwhcox8uvW7kvBM4PigSlKwFXiWpkYME4zJzv57ZGqwDtPjNi00H6EhXxnY"; // Your Stripe Publishable Key

        public StripeService()
        {
            StripeConfiguration.ApiKey = _stripeSectretKey;
        }

        public async Task<Session> CreateCheckoutSession(Event evnt)
        {
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
                            UnitAmount = (long)(evnt.TicketPrice * 100), // Amount in cents (convert from dollars)
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = evnt.EventName,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:7087/paymentsuccessful", // URL to redirect after successful payment
                CancelUrl = "https://localhost:7087/paymentcancelled",   // URL to redirect after canceled payment
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }
    }
}
