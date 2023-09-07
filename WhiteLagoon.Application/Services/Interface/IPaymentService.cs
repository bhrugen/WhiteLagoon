using Stripe.Checkout;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IPaymentService
    {
        SessionCreateOptions CreateSessionOptions(Booking booking, Villa villa, string domain);        
        Task<Session> CreateSessionAsync(SessionCreateOptions options);
        //Task<string> CreatePaymentSessionAsync(decimal totalAmount, string currency, string productName, string successUrl, string cancelUrl);
    }
}
