
using System.Collections.Generic;
using Moq;
using Solid.Sample;
using Xunit;

namespace Solid.Sample.Tests
{
    public class CheckoutServiceTests
    {
        [Fact]
        public void CalculateTotal_Applies_All_Rules_In_Order()
        {
            var cart = new Cart(new List<CartItem> { new("Item", 100m, 1) }, new Customer("a@b.com", true));

            var rule1 = new Mock<IDiscountRule>();
            rule1.Setup(r => r.Apply(It.IsAny<decimal>(), cart))
                 .Returns<decimal, Cart>((t, c) => t * 0.90m); // -10%

            var rule2 = new Mock<IDiscountRule>();
            rule2.Setup(r => r.Apply(It.IsAny<decimal>(), cart))
                 .Returns<decimal, Cart>((t, c) => t * 0.95m); // -5%

            var svc = new CheckoutService(new[] { rule1.Object, rule2.Object });

            var total = svc.CalculateTotal(cart);

            Assert.Equal(85.5m, total); // 100 * 0.90 * 0.95
        }
    }
}
