using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace JollyAPI.DAO
{
	public class OrderDAO
	{
		private readonly JollyShoppingOnlineContext _context;

		public OrderDAO(JollyShoppingOnlineContext context)
		{
			_context = context;
		}

		public List<Order> GetOrders()
		{
			var orders = new List<Order>();
			try
			{
				using (var context = new JollyShoppingOnlineContext())
				{
					orders = context.Orders.OrderByDescending(x => x.Id).ToList();
				}
			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}
			return orders;
		}
		public void AddOrder(OrderDTO orderDto)
		{
			User user = _context.Users.Where(u => u.Id == orderDto.UserId).FirstOrDefault();
			Product product = _context.Products.Where(p => p.Id == orderDto.ProductId).FirstOrDefault();

			var order = new Order
			{
				UserId = orderDto.UserId,
				CustomerName = user.FullName,
				Date = orderDto.Date,
				Phone = orderDto.Phone,
				Address = orderDto.Address,
				Status = orderDto.Status,
				Total = (double?)(product.Price * orderDto.Quantity)
			};

			_context.Orders.Add(order);
			_context.SaveChanges();

			int orderId = order.Id;

			var orderDetails = new OrderDetail
			{
				OrderId = orderId,
				ProductId = orderDto.ProductId,
				Quantity = orderDto.Quantity,
				Price = (double?)product.Price
			};

			_context.OrderDetails.Add(orderDetails);
			_context.SaveChanges();
		}


		public List<OrderDetailDTO> GetDetailByOrderId(int orderId)
		{
			List<OrderDetail> orderDetails = _context.OrderDetails.Where(x => x.OrderId == orderId)
				.Include(od => od.Product)
				.ToList();
			List<OrderDetailDTO> OrderDetailDTO = new List<OrderDetailDTO>();

			if (!orderDetails.Any())
			{
				OrderDetailDTO = new List<OrderDetailDTO>();

			}
			foreach (var detail in orderDetails)
			{
				OrderDetailDTO detailDTO = new OrderDetailDTO();
				detailDTO.OrderId = detail.OrderId;

				if (detail.Product != null)
				{
					detailDTO.ProductName = detail.Product.Name;
					detailDTO.UnitPrice = detail.Product.Price;
				}
				detailDTO.Image = _context.Products.Where(x => x.Id == detail.ProductId)
					.Include(p => p.Images).ToList().FirstOrDefault().Images.FirstOrDefault().Url;
				detailDTO.CustomerName = _context.Orders.Where(x => x.Id == detail.OrderId).FirstOrDefault().CustomerName;
				detailDTO.Quantity = detail.Quantity;
				OrderDetailDTO.Add(detailDTO);
			}
			return OrderDetailDTO;
		}

		public void DeleteOrderById(int orderId)
		{
			Order order = _context.Orders.Find(orderId);
			_context.Orders.Remove(order);
			_context.OrderDetails.RemoveRange(_context.OrderDetails.Include(x => x.Order).Where(od => od.Order.Id == orderId));
			_context.SaveChanges();
		}

		public void UpdateOrder(OrderDTO orderDto)
		{
			User user = _context.Users.Where(u => u.Id == orderDto.UserId).FirstOrDefault();
			var order = new Order
			{
				UserId = orderDto.UserId,
				CustomerName = user.FullName,
				Date = orderDto.Date,
				Phone = orderDto.Phone,
				Address = orderDto.Address,
				Status = orderDto.Status,
			};
			_context.Orders.Add(order);
			_context.SaveChanges();
		}

		public List<Order> MyOrders(int id)
		{
			List<Order> orders = _context.Orders
				.Where(x => x.UserId == id).ToList();
			return orders;
		}
		public List<Order> MyOrdersSearch(int id, DateTime from, DateTime to)
		{
			List<Order> orders = _context.Orders
				.Where(x => x.UserId == id
					  && x.Date >= from
					  && x.Date <= to)
		   .ToList();
			return orders;
		}

		public void Update(int id, string status)
		{
			Order orderToUpdate = _context.Orders.Find(id);

			orderToUpdate.Status = status;
			_context.SaveChanges();
		}

		public List<Order> getRecentOrder()
		{
			return _context.Orders.OrderByDescending(x => x.Date).Take(7).ToList();
		}

		public void Checkout(CheckoutDTO checkoutDTO)
		{
			User user = _context.Users.Where(x => x.Id == checkoutDTO.UserId).FirstOrDefault();
			List<Item> items = _context.Items.Where(x => x.CartId == user.CartId).ToList();
			_context.Items.RemoveRange(items);
			_context.SaveChanges();
			var order = new Order
			{
				UserId = checkoutDTO.UserId,
				CustomerName = checkoutDTO.CustomerName,
				Status = "pending",
				Phone = checkoutDTO.Phone,
				Address = checkoutDTO.Address,
				Total = double.Parse(checkoutDTO.Total),
				Date = DateTime.Today
			};
			_context.Orders.Add(order);
			_context.SaveChanges(true);
			foreach (var detail in checkoutDTO.OrderDetails)
			{
				try
				{
					var orderDetail = new OrderDetail
					{
						OrderId = order.Id,
						ProductId = detail.ProductId,
						Quantity = detail.Quantity,
						Price = (double)detail.Price,
						Size = detail.Size,
						Color = detail.Color
					};
					_context.OrderDetails.AddRange(orderDetail);
					_context.SaveChanges(true);
				}
				catch (Exception)
				{

				}
			}
		}
	}
}