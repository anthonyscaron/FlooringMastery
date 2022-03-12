using FlooringMastery.BLL;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    public class RemoveOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("************************************************");
            Console.WriteLine("* Remove an Order");
            Console.WriteLine("************************************************");
            Console.Write("\nEnter an Order Date (MMDDYYYY): ");

            string orderDate = Console.ReadLine();
            orderDate = ConsoleIO.ValidateOrderDateInput(orderDate);

            DisplayOrdersResponse displayResponse = manager.DisplayOrders(orderDate);

            if (displayResponse.Success)
            {
                ConsoleIO.DisplayOrderDetails(displayResponse.Orders, orderDate);

                Console.Write("\nEnter the Order Number for the order you wish to delete: ");
                string orderNumber = Console.ReadLine();
                orderNumber = ConsoleIO.ValidateOrderNumberInput(orderNumber, displayResponse.Orders);

                Console.Write("\nWould you like to remove this order (Y/N): ");
                string confirmation = Console.ReadLine();
                confirmation = ConsoleIO.ValidateYesNoInput(confirmation);

                if (confirmation == "Y")
                {
                    RemoveOrderResponse removeResponse = manager.RemoveOrder(orderNumber, displayResponse.Orders, orderDate);

                    if (removeResponse.Success)
                    {
                        Console.WriteLine("\n************************************************");
                        Console.WriteLine("* Order removed.");
                        Console.WriteLine("************************************************");
                    }
                    else
                    {
                        Console.WriteLine("An error occured: ");
                        Console.WriteLine(removeResponse.Message);
                    }
                }
                else if (confirmation == "N")
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Remove an Order cancelled.");
                    Console.WriteLine("************************************************");
                }
                else
                {
                    Console.WriteLine("\n************************************************");
                    Console.WriteLine("* Remove an Order failed.");
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
