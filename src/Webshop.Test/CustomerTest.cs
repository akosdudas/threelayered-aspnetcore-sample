using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Webshop.BL;
using Webshop.DAL;

namespace Webshop.Test
{
    [TestClass] // this class contains test code
    public class CustomerTest
    {
        [TestMethod] // this is a test instance
        public async Task TestDeleteNonExistingCustomer()
        {
            // Unit test: Arrange, Act, Assert

            // repository is mocked with a replacement object for this test as this test verifies the business logic and not the database/repository
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            // mock setup: calling GetCustomer always returns null
            customerRepositoryMock.Setup(repo => repo.GetCustomerOrNull(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(repo => repo.ListCustomerOrders(It.IsAny<int>())).ReturnsAsync(Array.Empty<object>());

            // instantiate the business layer class with the mocks
            var vm = new CustomerManager(customerRepositoryMock.Object, orderRepositoryMock.Object);

            // calling delete must return false
            Assert.IsFalse(await vm.TryDeleteCustomer(123));
        }
    }
}
