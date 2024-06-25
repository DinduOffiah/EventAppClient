function initializeStripe(secretKey) {
    console.log("Stripe is being initialized with key: ", secretKey);
    var stripe = Stripe(secretKey);

    window.redirectToCheckout = function (sessionId) {
        console.log("Redirecting to checkout with session ID: ", sessionId);
        stripe.redirectToCheckout({ sessionId: sessionId }).then(function (result) {
            if (result.error) {
                console.error(result.error.message);
            }
        });
    };
    console.log("redirectToCheckout function has been defined");
}
