using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    class EditOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("************************************************");
            Console.WriteLine("* Edit an Order");
            Console.WriteLine("************************************************");
            Console.Write("\nEnter an Order Date (MMDDYYYY): ");

            string orderDate = Console.ReadLine();
            orderDate = ConsoleIO.ValidateOrderDateInput(orderDate);

            DisplayOrdersResponse displayResponse = manager.DisplayOrders(orderDate);

            if (displayResponse.Success)
            {
                List<Order> revertedOrders = displayResponse.Orders;

                ConsoleIO.DisplayOrderDetails(displayResponse.Orders, orderDate);

                Console.Write("\nEnter the Order Number for the order you wish to edit: ");
                string orderNumber = Console.ReadLine();
                orderNumber = ConsoleIO.ValidateOrderNumberInput(orderNumber, displayResponse.Orders);

                var orderToEdit = from o in displayResponse.Orders
                                  where o.OrderNumber == int.Parse(orderNumber)
                                  select o;

                orderToEdit = orderToEdit.ToList();
                Order order = orderToEdit.First();

                Order revertedOrder = order.Copy();

                Console.WriteLine("\nYou will be able to edit the existing order information below.");
                Console.WriteLine("If you don't wish to edit the information, hit ENTER to skip.");

                Console.WriteLine("\nThe current Customer Name is {0}.", order.CustomerName);
                Console.Write("\nEnter the new Customer Name: ");
                string newCustomerName = Console.ReadLine();
                newCustomerName = ConsoleIO.ValidateEditedCustomerNameInput(newCustomerName, order);

                Console.WriteLine("\nThe current State Abbreviation is {0}.", order.State);
                Console.Write("\nEnter the name of the new Customer's State: ");
                string newState = Console.ReadLine();
                newState = ConsoleIO.ValidateEditedStateInput(newState, order);

                Console.WriteLine("\nThe current Product Type is {0}.", order.ProductType);
                Console.Write("\nEnter the new Product Type: ");
                string newProductType = Console.ReadLine();
                newProductType = ConsoleIO.ValidateEditedProductTypeInput(newProductType, order);

                Console.WriteLine("\nThe current Area is {0}.", order.Area);
                Console.Write("\nEnter the new Area: ");
                string newArea = Console.ReadLine();
                decimal convertedArea = ConsoleIO.ValidateEditedAreaInput(newArea, order);

                EditOrderResponse editResponse = manager.EditOrder(orderNumber, displayResponse.Orders, orderDate, newCustomerName,
                    newState, newProductType, convertedArea);

                ConsoleIO.ConfirmOrderDetails(editResponse.Order);

                Console.Write("\nWould you like to edit this order (Y/N): ");
                string confirmation = Console.ReadLine();
                confirmation = ConsoleIO.ValidateYesNoInput(confirmation);

                if(confirmation == "Y")
                {
                    if (editResponse.Success)
                    {
                        Console.WriteLine("\n************************************************");
                        Console.WriteLine("* Order edited.");
                        Console.WriteLine("************************************************");
                        ConsoleIO.DisplayOrderDetails(editResponse.Orders, orderDate);
                    }
                    else
                    {
                        Console.WriteLine("An error occured: ");
                        Console.WriteLine(editResponse.Message);
                    }
                }
                else if (confirmation == "N")
                {
                    string revertedState = ConsoleIO.ConvertStateAbbreviationToStateName(revertedOrder.State, revertedOrder);

                    EditOrderResponse revertEditResponse = manager.EditOrder(orderNumber, revertedOrders, orderDate, revertedOrder.CustomerName, revertedState,
                        revertedOrder.ProductType, revertedOrder.Area);
                    
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Edit an Order canceled.");
                    Console.WriteLine("************************************************");
                }
                else
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Edit an Order failed.");
                    Console.WriteLine("************************************************");
                }
            }
            else
            {
                Console.WriteLine("\nAn error occured: ");
                Console.WriteLine(displayResponse.Message);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
