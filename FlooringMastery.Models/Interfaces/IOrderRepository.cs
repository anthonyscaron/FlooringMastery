using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> LoadOrders(string orderDate);
        void SaveOrders(List<Order> orders, string orderDate);
        Order CreateOrder(string customerName, string state, string productType, decimal area);
        string ConvertOrderToFileFormat(Order order);
    }
}
