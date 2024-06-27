function BuyTicket(event) {
    // Get the event details (event.EventId, event.TicketPrice, etc.)
    // Initialize Stripe.js with your publishable key
    const stripe = Stripe('pk_test_51PV94HP93VNiDzg2xmOIPLeGULmr7EYaniwNdBkiXo9OzU9lSwvXctv2d6W4SEGInEYGhJ3SVYq13uaDySIHBFSm00lJQHcTsv');

    // Create a Payment Intent on your server
    fetch('/Payment/CreateCheckoutSession', {
        method: 'POST',
        body: JSON.stringify({ eventId: event.EventId }), // Send event ID to server
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            // Redirect to the Stripe checkout page
            return stripe.redirectToCheckout({ sessionId: data.sessionId });
        })
        .catch(error => {
            console.error('Error creating checkout session:', error);
        });
}
