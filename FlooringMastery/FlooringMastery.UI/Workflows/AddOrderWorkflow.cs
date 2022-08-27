using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("************************************************");
            Console.WriteLine("* Add an Order");
            Console.WriteLine("************************************************");
            
            Console.Write("\nEnter the future Order Date (MMDDYYYY): ");
            string orderDate = Console.ReadLine();
            orderDate = ConsoleIO.ValidateFutureOrderDateInput(orderDate);

            Console.Write("\nEnter the Customer Name: ");
            string customerName = Console.ReadLine();
            customerName = ConsoleIO.ValidateCustomerNameInput(customerName);

            Console.Write("\nEnter the name of the Customer's State: ");
            string state = Console.ReadLine();
            state = ConsoleIO.ValidateStateInput(state);

            Console.Write("\nEnter the Product Type: ");
            string productType = Console.ReadLine();
            productType = ConsoleIO.ValidateProductTypeInput(productType);

            Console.Write("\nEnter the Area: ");
            string area = Console.ReadLine();
            decimal convertedArea = ConsoleIO.ValidateAreaInput(area);

            AddOrderResponse addResponse = manager.AddOrder(orderDate, customerName, state, productType, convertedArea);

            if (addResponse.Success)
            {
                ConsoleIO.ConfirmOrderDetails(addResponse.Order);
                Console.Write("\nWould you like to place this order (Y/N): ");
                string confirmation = Console.ReadLine();
                confirmation = ConsoleIO.ValidateYesNoInput(confirmation);

                if (confirmation == "Y")
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Order added.");
                    Console.WriteLine("************************************************");
                    List<Order> orders = new List<Order>();
                    orders.Add(addResponse.Order);
                    ConsoleIO.DisplayOrderDetails(orders, orderDate);
                }
                else if (confirmation == "N")
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Add an Order cancelled.");
                    Console.WriteLine("************************************************");
                    string orderNumber = Convert.ToString(addResponse.Order.OrderNumber);
                    RemoveOrderResponse removeResponse = manager.RemoveOrder(orderNumber, addResponse.Orders, orderDate);
                }
                else
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Order Add failed.");
                    Console.WriteLine("************************************************");
                }
            }
            else
            {
                Console.WriteLine("An error occured: ");
                Console.WriteLine(addResponse.Message);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
