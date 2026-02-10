
using System; using System.Collections.Generic; using System.Linq; using System.Threading.Tasks;

public record CartItem(string Name, decimal Price, int Qty);
public record Customer(string Email, bool IsGold);
public record Cart(List<CartItem> Items, Customer Customer);

public interface IDiscountRule { decimal Apply(decimal total, Cart cart); }
public class NewYearDiscount : IDiscountRule { public decimal Apply(decimal t, Cart c) => t * 0.90m; }
public class LoyaltyDiscount : IDiscountRule { public decimal Apply(decimal t, Cart c) => c.Customer.IsGold ? t * 0.95m : t; }

public class CheckoutService {
    private readonly IEnumerable<IDiscountRule> _rules;
    public CheckoutService(IEnumerable<IDiscountRule> rules) => _rules = rules;
    public decimal CalculateTotal(Cart cart){ var subtotal = cart.Items.Sum(i => i.Price * i.Qty); return _rules.Aggregate(subtotal, (t,r) => r.Apply(t, cart)); }
}

public class Program {
    public static async Task Main(){
        var cart = new Cart(new List<CartItem>{ new("Oil Filter", 400, 1), new("Engine Oil", 1200, 1) }, new Customer("user@example.com", true));
        var rules = new List<IDiscountRule>{ new NewYearDiscount(), new LoyaltyDiscount() };
        var checkout = new CheckoutService(rules);
        var total = checkout.CalculateTotal(cart);
        Console.WriteLine($"Final Total: {total}");
        await Task.CompletedTask;
    }
}
