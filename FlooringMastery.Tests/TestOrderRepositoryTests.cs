using FlooringMastery.BLL;
using FlooringMastery.Data;
using FlooringMastery.Models.Responses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Tests
{
    [TestFixture]
    public class TestOrderRepositoryTests
    {
        private string _testData = @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Test\";
        private string _seedData = @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Seed\";

        [TestCase("06012013", true)]
        public void CanDisplayOrders(string orderDate, bool expectedOutcome)
        {
            string orderInfo = "Orders_06012013.txt";

            if (File.Exists(_testData + orderInfo))
            {
                File.Delete(_testData + orderInfo);
            }

            File.Copy((_seedData + orderInfo), (_testData + orderInfo));

            TestOrderRepository repository = new TestOrderRepository(_testData);

            OrderManager manager = OrderManagerFactory.Create();

            DisplayOrdersResponse response = manager.DisplayOrders(orderDate);

            Assert.AreEqual(expectedOutcome, response.Success);
        }
        
        [TestCase("08012022", "First Add Order Customer LLC", "Michigan", "Tile", "50", true)] // Will be removed in Remove Test
        public void CanAddOrder(string orderDate, string customerName, string state, string productType, decimal area, bool expectedOutcome)
        {
            string orderInfo1 = "Orders_08012022.txt";
            
            if (File.Exists(_testData + orderInfo1))
            {
                File.Delete(_testData + orderInfo1);
            }
            
            TestOrderRepository repository = new TestOrderRepository(_testData);

            OrderManager manager = OrderManagerFactory.Create();

            AddOrderResponse response = manager.AddOrder(orderDate, customerName, state, productType, area);

            Assert.AreEqual(expectedOutcome, response.Success);
        }

        [TestCase("08012022", "Second Add Order Customer Inc", "Ohio", "Wood", "100", true)]
        public void CanAddSecondOrder(string orderDate, string customerName, string state, string productType, decimal area, bool expectedOutcome)
        {
            TestOrderRepository repository = new TestOrderRepository(_testData);

            OrderManager manager = OrderManagerFactory.Create();

            AddOrderResponse response = manager.AddOrder(orderDate, customerName, state, productType, area);

            Assert.AreEqual(expectedOutcome, response.Success);
        }

        [TestCase("08012022", "2", "Edited Order Customer Inc", "Michigan", "Wood", "100", true)]
        public void CanEditOrder(string orderDate, string orderNumber, string customerName, string state, string productType, decimal area, bool expectedOutcome)
        {
            TestOrderRepository repository = new TestOrderRepository(_testData);

            OrderManager manager = OrderManagerFactory.Create();

            DisplayOrdersResponse displayResponse = manager.DisplayOrders(orderDate);

            EditOrderResponse editResponse = new EditOrderResponse();

            if (displayResponse.Success)
            {

                editResponse = manager.EditOrder(orderNumber, displayResponse.Orders, orderDate, customerName, state, productType, area);
            }

            Assert.AreEqual(expectedOutcome, editResponse.Success);
        }
        
        [TestCase("08012022", "1", true)]
        public void CanRemoveOrder(string orderDate, string orderNumber, bool expectedOutcome)
        {
            TestOrderRepository repository = new TestOrderRepository(_testData);

            OrderManager manager = OrderManagerFactory.Create();

            DisplayOrdersResponse displayResponse = manager.DisplayOrders(orderDate);

            RemoveOrderResponse removeResponse = new RemoveOrderResponse();

            if (displayResponse.Success)
            {

                removeResponse = manager.RemoveOrder(orderNumber, displayResponse.Orders, orderDate);
            }

            Assert.AreEqual(expectedOutcome, removeResponse.Success); 
        }
    }
}
