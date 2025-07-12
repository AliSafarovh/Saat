using Application.Repositories.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
       
            private readonly IOrderReadRepository _orderReadRepository;
        private const int PageSize = 7;
        public OrderController(IOrderReadRepository orderReadRepository)
            {
                _orderReadRepository = orderReadRepository;
            }

        public async Task<IActionResult> Index(int page = 1)
        {
            var totalOrders = await _orderReadRepository.GetAll().CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalOrders / PageSize);

            var orders = await _orderReadRepository
                .GetAll()
                .OrderByDescending(o => o.CreatedDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Include(o => o.OrderItems)
                .ToListAsync();

            var viewModel = new OrderListViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
    }
}
