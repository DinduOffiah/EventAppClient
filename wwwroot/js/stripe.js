// wwwroot/js/stripe.js
function initializeStripe(publishableKey) {
    var stripe = Stripe(publishableKey);

    window.redirectToCheckout = function (sessionId) {
        stripe.redirectToCheckout({ sessionId: sessionId });
    };
}
