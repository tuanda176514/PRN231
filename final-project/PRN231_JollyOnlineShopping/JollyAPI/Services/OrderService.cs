using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.Services
{
    public class OrderService
    {
		private readonly OrderDAO orderDAO;
		public OrderService(OrderDAO orderDAO)
		{
			this.orderDAO = orderDAO;
		}
		public List<Order> GetAllOrders() => orderDAO.GetOrders();
		public void Checkout(CheckoutDTO checkoutDTO) => orderDAO.Checkout(checkoutDTO);

		public void AddOrder(OrderDTO orderDto) => orderDAO.AddOrder(orderDto);
		public List<OrderDetailDTO> GetDetailByOrderId(int orderId) => orderDAO.GetDetailByOrderId(orderId);

		public void DeleteOrderById(int orderId) => orderDAO.DeleteOrderById(orderId);

		public void UpdateOrder(OrderDTO orderDto) => orderDAO.UpdateOrder(orderDto);
		public void UpdateStatus(int id, string status) => orderDAO.Update(id, status);
		public List<Order> MyOrder(int userId) => orderDAO.MyOrders(userId);

        public List<Order> MyOrderSearch(int userId, DateTime from, DateTime to) => orderDAO.MyOrdersSearch(userId, from, to);
    }
}
