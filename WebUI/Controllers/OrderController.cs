using Application.Repositories.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
       
            private readonly IOrderReadRepository _orderReadRepository;

            public OrderController(IOrderReadRepository orderReadRepository)
            {
                _orderReadRepository = orderReadRepository;
            }

            public async Task<IActionResult> Index()
            {
            var orders = await _orderReadRepository.GetAll()
               .Include(o => o.OrderItems)
               .ToListAsync();

            return View(orders);
            }
        
    }
}
