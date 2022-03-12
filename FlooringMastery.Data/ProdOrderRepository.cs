using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class ProdOrderRepository : IOrderRepository
    {
        private string _filePath;
        private List<Order> _orders = new List<Order>();

        public ProdOrderRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Order> LoadOrders(string orderDate)
        {
            string testPath = _filePath + "Orders_" + orderDate + ".txt";

            if (!File.Exists(testPath))
            {
                _orders = null;
                return _orders;
            }
            else
            {

                using (StreamReader sr = new StreamReader(testPath))
                {
                    sr.ReadLine();
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Order newOrder = new Order();
                        string[] columns = line.Split(',');

                        newOrder.OrderNumber = int.Parse(columns[0]);
                        newOrder.CustomerName = columns[1];
                        newOrder.State = columns[2];
                        newOrder.TaxRate = decimal.Parse(columns[3]);
                        newOrder.ProductType = columns[4];
                        newOrder.Area = decimal.Parse(columns[5]);
                        newOrder.CostPerSquareFoot = decimal.Parse(columns[6]);
                        newOrder.LaborCostPerSquareFoot = decimal.Parse(columns[7]);
                        newOrder.MaterialCost = decimal.Parse(columns[8]);
                        newOrder.LaborCost = decimal.Parse(columns[9]);
                        newOrder.Tax = decimal.Parse(columns[10]);
                        newOrder.Total = decimal.Parse(columns[11]);

                        _orders.Add(newOrder);
                    }
                }
            }

            return _orders;
        }

        public void SaveOrders(List<Order> orders, string orderDate)
        {
            string testPath = _filePath + "Orders_" + orderDate + ".txt";

            if (File.Exists(testPath))
            {
                File.Delete(testPath);
            }

            File.Create(testPath).Dispose();

            using (StreamWriter sw = new StreamWriter(testPath))
            {
                sw.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");

                if (orders != null)
                {
                    foreach (var o in orders)
                    {
                        sw.WriteLine(ConvertOrderToFileFormat(o));
                    }
                }
            }
        }

        public Order CreateOrder(string customerName, string state, string productType, decimal area)
        {
            Order order = new Order();
            Tax tax = ProdOrderRepository.GetTaxInfo(state);
            Product product = ProdOrderRepository.GetProductInfo(productType);

            order.OrderNumber = 0;
            order.CustomerName = customerName;
            order.State = tax.StateAbbreviation;
            order.TaxRate = tax.TaxRate;
            order.ProductType = productType;
            order.Area = area;
            order.CostPerSquareFoot = product.CostPerSquareFoot;
            order.LaborCostPerSquareFoot = product.LaborCostPerSquareFoot;
            order.MaterialCost = Math.Round((order.Area * order.CostPerSquareFoot), 2);
            order.LaborCost = Math.Round((order.Area * order.LaborCostPerSquareFoot), 2);
            order.Tax = Math.Round(((order.MaterialCost + order.LaborCost) * (order.TaxRate / 100)), 2);
            order.Total = Math.Round((order.MaterialCost + order.LaborCost + order.Tax), 2);

            return order;
        }

        public static Tax GetTaxInfo(string state)
        {
            string taxPath = @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Taxes.txt";
            Tax taxInfo = new Tax();

            using (StreamReader sr = new StreamReader(taxPath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Tax tax = new Tax();
                    string[] columns = line.Split(',');

                    tax.StateAbbreviation = columns[0];
                    tax.StateName = columns[1];
                    tax.TaxRate = decimal.Parse(columns[2]);

                    if (tax.StateName == state)
                    {
                        taxInfo = tax;
                    }
                }
            }

            return taxInfo;
        }

        public static Product GetProductInfo(string productType)
        {
            string productPath = @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Products.txt";
            Product productInfo = new Product();

            using (StreamReader sr = new StreamReader(productPath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Product product = new Product();
                    string[] columns = line.Split(',');

                    product.ProductType = columns[0];
                    product.CostPerSquareFoot = decimal.Parse(columns[1]);
                    product.LaborCostPerSquareFoot = decimal.Parse(columns[2]);

                    if (product.ProductType == productType)
                    {
                        productInfo = product;
                    }
                }
            }

            return productInfo;
        }

        public string ConvertOrderToFileFormat(Order order)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", order.OrderNumber, order.CustomerName,
                order.State, order.TaxRate, order.ProductType, order.Area, order.CostPerSquareFoot, order.LaborCostPerSquareFoot,
                order.MaterialCost, order.LaborCost, order.Tax, order.Total);
        }
    }
}
