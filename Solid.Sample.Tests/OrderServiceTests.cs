
using System.Threading.Tasks;
using Moq;
using Solid.Sample;
using Xunit;

namespace Solid.Sample.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task PlaceOrder_Sends_Notification_Once()
        {
            var mock = new Mock<INotifier>();
            var svc = new OrderService(mock.Object);

            await svc.PlaceOrderAsync(new Order { CustomerEmail = "x@y.com" });

            mock.Verify(n => n.SendAsync("x@y.com", It.IsAny<string>()), Times.Once);
        }
    }
}
