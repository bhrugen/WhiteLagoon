using Stripe.Checkout;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class PaymentService : IPaymentService
    {
        public async Task<Session> CreateSessionAsync(SessionCreateOptions options)
        {
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }
        public SessionCreateOptions CreateSessionOptions(Booking booking, Villa villa, string domain)
        {
            // Calculate the total cost based on the booking and villa details
            long totalAmount = (long)(booking.Nights * villa.Price * 100); // Amount in cents

            // Define the session creation options
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" }, // Payment method type(s) you want to accept
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd", // Set the currency code as needed
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                    },
                    UnitAmount = totalAmount,
                },
                Quantity = 1, // Number of items in the cart
            },
        },
                SuccessUrl = $"{domain}/booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = $"{domain}/booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
                Mode = "payment", // Set the payment mode
                                  // Add any other options you need
            };

            return sessionOptions;
        }

    }

}
