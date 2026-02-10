
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solid.Sample
{
    // DIP example
    public interface INotifier
    {
        Task SendAsync(string to, string msg);
    }

    public class EmailNotifier : INotifier
    {
        public Task SendAsync(string to, string msg)
        {
            // SMTP logic in real app
            Console.WriteLine($"Email to {to}: {msg}");
            return Task.CompletedTask;
        }
    }

    public class Order
    {
        public required string CustomerEmail { get; set; }
    }

    public class OrderService
    {
        private readonly INotifier _notifier;
        public OrderService(INotifier notifier) => _notifier = notifier;

        public async Task PlaceOrderAsync(Order order)
        {
            // business logic...
            await _notifier.SendAsync(order.CustomerEmail, "Order placed");
        }
    }

    // OCP example
    public record CartItem(string Name, decimal Price, int Qty);
    public record Customer(string Email, bool IsGold);
    public record Cart(List<CartItem> Items, Customer Customer);

    public interface IDiscountRule
    {
        decimal Apply(decimal total, Cart cart);
    }

    public class NewYearDiscount : IDiscountRule
    {
        public decimal Apply(decimal total, Cart cart) => total * 0.90m;
    }

    public class LoyaltyDiscount : IDiscountRule
    {
        public decimal Apply(decimal total, Cart cart) => cart.Customer.IsGold ? total * 0.95m : total;
    }

    public class CheckoutService
    {
        private readonly IEnumerable<IDiscountRule> _rules;
        public CheckoutService(IEnumerable<IDiscountRule> rules) => _rules = rules;

        public decimal CalculateTotal(Cart cart)
        {
            var subtotal = cart.Items.Sum(i => i.Price * i.Qty);
            return _rules.Aggregate(subtotal, (t, r) => r.Apply(t, cart));
        }
    }
}
