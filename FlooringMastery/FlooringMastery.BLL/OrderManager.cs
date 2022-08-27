using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class OrderManager
    {
        private IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public DisplayOrdersResponse DisplayOrders(string orderDate)
        {
            DisplayOrdersResponse response = new DisplayOrdersResponse();

            response.Orders = _orderRepository.LoadOrders(orderDate);

            if (response.Orders == null)
            {
                response.Success = false;
                response.Message = "Unable to load orders. Please try again.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public AddOrderResponse AddOrder(string orderDate, string customerName, string state, string productType, decimal area)
        {
            AddOrderResponse response = new AddOrderResponse();
            int oldOrderCount;
            int newOrderCount;
            int currentOrderNumber = 0;

            response.Orders = _orderRepository.LoadOrders(orderDate);
            if(response.Orders == null)
            {
                oldOrderCount = 0;
                List<Order> orders = new List<Order>();
                response.Orders = orders;
            }
            else
            {
                oldOrderCount = response.Orders.Count();
                currentOrderNumber = response.Orders.Select(o => o.OrderNumber).Max();
            }

            response.Order = _orderRepository.CreateOrder(customerName, state, productType, area);
            currentOrderNumber++;
            response.Order.OrderNumber = currentOrderNumber;

            response.Orders.Add(response.Order);
            newOrderCount = response.Orders.Count();

            if (newOrderCount <= oldOrderCount)
            {
                response.Success = false;
                response.Message = "Unable to add account. Please try again.";
            }
            else
            {
                response.Success = true;
                _orderRepository.SaveOrders(response.Orders, orderDate);
            }

            return response;
        }

        public EditOrderResponse EditOrder(string orderNumber, List<Order> orders, string orderDate, string customerName,
                    string state, string productType, decimal area)
        {
            EditOrderResponse response = new EditOrderResponse();

            bool edited = false;
            response.Orders = orders;

            int result = 0;
            int.TryParse(orderNumber, out result);

            Order order = _orderRepository.CreateOrder(customerName, state, productType, area);
            order.OrderNumber = result;

            foreach (var o in response.Orders)
            {
                if (o.OrderNumber == result)
                {
                    o.CustomerName = order.CustomerName;
                    o.State = order.State;
                    o.TaxRate = order.TaxRate;
                    o.ProductType = order.ProductType;
                    o.Area = order.Area;
                    o.CostPerSquareFoot = order.CostPerSquareFoot;
                    o.LaborCostPerSquareFoot = order.LaborCostPerSquareFoot;
                    o.MaterialCost = order.MaterialCost;
                    o.LaborCost = order.LaborCost;
                    o.Tax = order.Tax;
                    o.Total = order.Total;

                    response.Order = o;
                    edited = true;
                }
            }

            if (!edited)
            {
                response.Success = false;
                response.Message = "Unable to edit account. Please try again.";
            }
            else
            {
                response.Success = true;
                _orderRepository.SaveOrders(response.Orders, orderDate);
            }

            return response;
        }

        public RemoveOrderResponse RemoveOrder(string orderNumber, List<Order> orders, string orderDate)
        {
            RemoveOrderResponse response = new RemoveOrderResponse();

            int result = 0;
            int.TryParse(orderNumber, out result);

            var updatedOrders = from o in orders
                                where o.OrderNumber != result
                                select o;

            response.Orders = updatedOrders.ToList();

            int newCount = updatedOrders.Count();
            int oldCount = orders.Count();

            if (newCount != (oldCount - 1))
            {
                response.Success = false;
                response.Message = "Unable to remove order. Please try again.";
            }
            else
            {
                response.Success = true;
                _orderRepository.SaveOrders(response.Orders, orderDate);
            }

            return response;
        }
    }
}
