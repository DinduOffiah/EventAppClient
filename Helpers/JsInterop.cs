using Microsoft.JSInterop;

namespace EventAppClient.Helpers
{
    public class JsInterop
    {
        public static ValueTask RedirectToCheckout(IJSRuntime jsRuntime, string publicKey, string sessionId)
        {
            return jsRuntime.InvokeVoidAsync("JsInterop.redirectToCheckout", publicKey, sessionId);
        }
    }
}
