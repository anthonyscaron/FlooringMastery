using FlooringMastery.BLL;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    public class DisplayOrdersWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("************************************************");
            Console.WriteLine("* Display Orders");
            Console.WriteLine("************************************************");
            Console.Write("\nEnter an Order Date (MMDDYYYY): ");

            string orderDate = Console.ReadLine();
            orderDate = ConsoleIO.ValidateOrderDateInput(orderDate);

            DisplayOrdersResponse response = manager.DisplayOrders(orderDate);

            if (response.Success)
            {
                ConsoleIO.DisplayOrderDetails(response.Orders, orderDate);
            }
            else
            {
                Console.WriteLine("\nAn error occured: ");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
