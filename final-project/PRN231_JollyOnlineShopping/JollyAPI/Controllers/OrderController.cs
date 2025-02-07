using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace JollyAPI.Controllers
{
    [Route("api/[orders]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
		private readonly OrderService orderService;
		public OrderController(OrderService orderService)
		{
			this.orderService = orderService;
		}

		[HttpGet("/orders/myorder/{id}")]
		public ActionResult<IList<Order>> MyOrders(int id) => orderService.MyOrder(id);

		[HttpPut("/orders/status/{id}")]
		public void Update(int id, string? status) => orderService.UpdateStatus(id, status);

		[HttpGet("/orders/detail/{orderId}")]
		public ActionResult<IList<OrderDetailDTO>> GetDetailByOrderId(int orderId) => orderService.GetDetailByOrderId(orderId);

		[HttpPost("/orders/create")]
		public void PostOrder(OrderDTO orderDto) => orderService.UpdateOrder(orderDto);

		[HttpDelete("/orders/delete/{orderId}")]
		public void DeleteOrderById(int orderId) => orderService.DeleteOrderById(orderId);

		[HttpGet("/orders")]
		public ActionResult<IList<Order>> Get() => orderService.GetAllOrders();
        [HttpPost("/orders/checkout")]
        public void Checkout(CheckoutDTO checkoutDTO) => orderService.Checkout(checkoutDTO);

		[HttpGet("/orders/myorder/{id}/{fromDate}/{toDate}")]
		public ActionResult<IList<Order>> MyOrdersSearch(int id, DateTime fromDate, DateTime toDate) => orderService.MyOrderSearch(id, fromDate, toDate);


    }

}
