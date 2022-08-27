using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery
{
    public class ConsoleIO
    {
        public static void DisplayOrderDetails(List<Order> orders, string orderDate)
        {
            Console.WriteLine("\n************************************************");

            foreach (var o in orders)
            {
                Console.WriteLine("{0} | {1}", o.OrderNumber, orderDate);
                Console.WriteLine("{0}", o.CustomerName);
                Console.WriteLine("{0}", o.State);
                Console.WriteLine("Product: {0}", o.ProductType);
                Console.WriteLine("Materials: {0}", o.MaterialCost);
                Console.WriteLine("Labor: {0}", o.LaborCost);
                Console.WriteLine("Tax: {0}", o.Tax);
                Console.WriteLine("Total: {0}", o.Total);
                Console.WriteLine("************************************************");
            }
        }

        public static void ConfirmOrderDetails(Order order)
        {
            Console.WriteLine("\n************************************************");
            Console.WriteLine("* Order Information");
            Console.WriteLine("************************************************");
            Console.WriteLine("\nCustomer Name: {0}", order.CustomerName);
            Console.WriteLine("State: {0}", order.State);
            Console.WriteLine("Tax Rate: {0}", order.TaxRate);
            Console.WriteLine("Product Type: {0}", order.ProductType);
            Console.WriteLine("Area: {0}", order.Area);
            Console.WriteLine("Cost per Square Foot: {0}", order.CostPerSquareFoot);
            Console.WriteLine("Labor Cost per Square Foot: {0}", order.LaborCostPerSquareFoot);
            Console.WriteLine("Material Cost: {0}", order.MaterialCost);
            Console.WriteLine("Labor Cost: {0}", order.LaborCost);
            Console.WriteLine("Tax: {0}", order.Tax);
            Console.WriteLine("Total: {0}", order.Total);
        }

        public static void ShowMessage(string parameter, string message)
        {
            Console.WriteLine("\nInvalid {0}.", parameter);
            Console.WriteLine("{0} {1}", parameter, message);
        }
        
        public static string ValidateOrderDateInput(string orderDate)
        {
            int result = 0;
            bool converted = int.TryParse(orderDate, out result);

            while (orderDate.Length != 8 && !converted)
            {
                ShowMessage("Order Date", "must be all numbers in the correct format: MMDDYYYY");
                Console.Write("\nEnter an Order Date (MMDDYYYY): ");
                orderDate = Console.ReadLine();
                converted = int.TryParse(orderDate, out result);
            }

            return orderDate;
        }

        public static string ValidateFutureOrderDateInput(string orderDate)
        {
            orderDate = ValidateOrderDateInput(orderDate);

            DateTime dateTime = DateTime.ParseExact(orderDate, "MMddyyyy", CultureInfo.InvariantCulture);

            while (dateTime < DateTime.Today)
            {
                ShowMessage("Order Date", "must be a futue date.");
                Console.Write("\nEnter an Order Date (MMDDYYYY): ");
                orderDate = Console.ReadLine();
                dateTime = DateTime.ParseExact(orderDate, "MMddyyyy", CultureInfo.InvariantCulture);
            }

            return orderDate;
        }

        public static string ValidateCustomerNameInput(string customerName)
        {
            while (customerName.Length < 1)
            {
                ShowMessage("Customer Name", "cannot be blank.");
                Console.Write("\nEnter the Customer Name: ");
                customerName = Console.ReadLine();
            }

            return customerName;
        }

        public static string ValidateStateInput(string state)
        {
            string taxPath = @"C:\Bootcamp\Repos\online-net-2021-anthonyscaron\Classwork\Badge2\FlooringMastery\FlooringMastery.UI\Data\Taxes.txt";
            List<Tax> taxes = new List<Tax>();

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

                    taxes.Add(tax);
                }
            }
            var stateNames = from t in taxes
                             select t.StateName;
            stateNames = stateNames.ToList();

            var nameLength = from n in stateNames
                             select n.Length;
            nameLength = nameLength.ToList();
            int smallestName = nameLength.Min();

            while (state.Length < smallestName)
            {
                Console.WriteLine("\nInvalid Customer State.");
                Console.WriteLine("State Name must be at least {0} characters.", smallestName);
                Console.Write("\nEnter the Customer's State: ");
                state = Console.ReadLine();

                while (!stateNames.Any(s => s.Contains(state)))
                {
                    Console.WriteLine("\nInvalid Customer State.");
                    Console.WriteLine("These are the valid Customer States: ");
                    foreach (var s in stateNames)
                    {
                        Console.WriteLine("\t{0}", s);
                    }
                    Console.Write("\nEnter the Customer's State: ");
                    state = Console.ReadLine();
                }
            }

            return state;
        }

        public static string ValidateProductTypeInput(string productType)
        {
            while (productType.Length < 1)
            {
                ShowMessage("Product Type", "cannot be blank.");
                Console.Write("\nEnter the Product Type: ");
                productType = Console.ReadLine();
            }

            string productPath = @"C:\Bootcamp\Repos\online-net-2021-anthonyscaron\Classwork\Badge2\FlooringMastery\FlooringMastery.UI\Data\Products.txt";
            List<string> products = new List<string>();

            using (StreamReader sr = new StreamReader(productPath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Product product = new Product();
                    string[] columns = line.Split(',');

                    product.ProductType = columns[0];

                    products.Add(product.ProductType);
                }
            }

            while (!products.Any(s => s.Contains(productType)))
            {
                Console.WriteLine("\nInvalid Product Type.");
                Console.WriteLine("These are the valid Product Types: ");
                foreach (var p in products)
                {
                    Console.WriteLine("\t{0}", p);
                }
                Console.Write("\nEnter the Product Type: ");
                productType = Console.ReadLine();
            }

            return productType;
        }

        public static decimal ValidateAreaInput(string area)
        {
            decimal result = 0;
            bool converted = false;
            decimal minValue = 100;

            while (!converted)
            {
                decimal.TryParse(area, out result);

                while (result < minValue)
                {
                    Console.WriteLine("\nInvalid Area.");
                    Console.WriteLine("The Area must be at least {0}.", minValue);
                    Console.Write("\nEnter the Area: ");
                    area = Console.ReadLine();
                    decimal.TryParse(area, out result);
                }

                converted = true;
            }

            return result;
        }

        public static string ValidateOrderNumberInput(string orderNumber, List<Order> orders)
        {
            int result = 0;
            bool valid = false;

            while (!valid)
            {
                while (!int.TryParse(orderNumber, out result))
                {
                    ShowMessage("Order Number", "must be a number.");
                    Console.Write("\nEnter the Order Number: ");
                    orderNumber = Console.ReadLine();
                }

                var orderNumbers = from o in orders
                                   where o.OrderNumber == result
                                   select o.OrderNumber;

                orderNumbers = orderNumbers.ToList();
                int count = orderNumbers.Count();

                if (count != 1)
                {
                    ShowMessage("Order Number", "must be an existing Order Number.");
                    Console.Write("\nEnter the Order Number: ");
                    orderNumber = Console.ReadLine();
                }
                else
                {
                    valid = true;
                }
            }

            return orderNumber;
        }

        public static string ValidateYesNoInput(string confirmation)
        {
            while (confirmation != "Y" && confirmation != "N")
            {
                Console.Write("\nPlease confirm Yes or No (Y/N): ");
                confirmation = Console.ReadLine();
            }

            return confirmation;
        }

        public static string ValidateEditedCustomerNameInput(string customerName, Order order)
        {
            if (customerName == "")
            {
                customerName = order.CustomerName;
            }

            return customerName;
        }

        public static string ValidateEditedStateInput(string state, Order order)
        {
            bool edited = false;

            string taxPath = @"C:\Bootcamp\Repos\online-net-2021-anthonyscaron\Classwork\Badge2\FlooringMastery\FlooringMastery.UI\Data\Taxes.txt";
            List<Tax> taxes = new List<Tax>();

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

                    taxes.Add(tax);
                }
            }

            while (!edited)
            {
                if (state == "")
                {
                    foreach (var t in taxes)
                    {
                        if (t.StateAbbreviation == order.State)
                        {
                            state = t.StateName;
                            edited = true;
                        }
                    }
                }
                else
                {
                    var stateNames = from t in taxes
                                     select t.StateName;
                    stateNames = stateNames.ToList();

                    var nameLength = from n in stateNames
                                     select n.Length;
                    nameLength = nameLength.ToList();
                    int smallestName = nameLength.Min();

                    while (state.Length < smallestName)
                    {
                        Console.WriteLine("\nInvalid Customer State.");
                        Console.WriteLine("State Name must be at least {0} characters.", smallestName);
                        Console.Write("\nEnter the Customer's State: ");
                        state = Console.ReadLine();

                        while (!stateNames.Any(s => s.Contains(state)))
                        {
                            Console.WriteLine("\nInvalid Customer State.");
                            Console.WriteLine("These are the valid Customer States: ");
                            foreach (var s in stateNames)
                            {
                                Console.WriteLine("\t{0}", s);
                            }
                            Console.Write("\nEnter the Customer's State: ");
                            state = Console.ReadLine();
                        }
                    }

                    edited = true;
                }
            }

            return state;
        }

        public static string ValidateEditedProductTypeInput(string productType, Order order)
        {
            bool edited = false;

            while (!edited)
            {
                if (productType == "")
                {
                    productType = order.ProductType;
                    edited = true;
                }
                else
                {
                    string productPath = @"C:\Bootcamp\Repos\online-net-2021-anthonyscaron\Classwork\Badge2\FlooringMastery\FlooringMastery.UI\Data\Products.txt";
                    List<string> products = new List<string>();

                    using (StreamReader sr = new StreamReader(productPath))
                    {
                        sr.ReadLine();
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            Product product = new Product();
                            string[] columns = line.Split(',');

                            product.ProductType = columns[0];

                            products.Add(product.ProductType);
                        }
                    }

                    while (!products.Any(s => s.Contains(productType)))
                    {
                        Console.WriteLine("\nInvalid Product Type.");
                        Console.WriteLine("These are the valid Product Types: ");
                        foreach (var p in products)
                        {
                            Console.WriteLine("\t{0}", p);
                        }
                        Console.Write("\nEnter the Product Type: ");
                        productType = Console.ReadLine();
                    }

                    edited = true;
                }
            }

            return productType;
        }

        public static decimal ValidateEditedAreaInput(string area, Order order)
        {
            bool edited = false;
            decimal result = 0;
            decimal minValue = 100;

            if (area == "")
            {
                result = order.Area;
                edited = true;
            }

            while (!edited)
            {
                bool converted = false;

                while (!converted)
                {
                    decimal.TryParse(area, out result);

                    while (result < minValue)
                    {
                        Console.WriteLine("\nInvalid Area.");
                        Console.WriteLine("The Area must be at least {0}.", minValue);
                        Console.Write("\nEnter the Area: ");
                        area = Console.ReadLine();
                        decimal.TryParse(area, out result);
                    }

                    converted = true;
                }

                edited = true;
            }

            return result;
        }

        public static string ConvertStateAbbreviationToStateName(string state, Order order)
        {
            string taxPath = @"C:\Bootcamp\Repos\online-net-2021-anthonyscaron\Classwork\Badge2\FlooringMastery\FlooringMastery.UI\Data\Taxes.txt";
            List<Tax> taxes = new List<Tax>();

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

                    taxes.Add(tax);
                }
            }

            foreach (var t in taxes)
            {
                if (t.StateAbbreviation == state)
                {
                    state = t.StateName;
                }
            }

            return state;
        }
    }
}
