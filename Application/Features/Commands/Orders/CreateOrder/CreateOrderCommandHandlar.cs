using Application.Repositories.Orders;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IOrderWriteRepository _orderWriteRepository;

        public CreateOrderCommandHandler(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Mail = request.Mail,
                Location = request.Location,
                City = request.City,
                AdditionalInfo = request.AdditionalInfo,
                OrderItems = request.OrderItems.Select(x => new OrderItem
                {
                    Name = x.Name,
                    Color = x.Color,
                    Count = x.Count,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice
                }).ToList()
            };

            await _orderWriteRepository.AddAsync(order);
            await _orderWriteRepository.SaveChangesAsync();

            return new CreateOrderCommandResponse
            {
                OrderId = order.Id,
                Message = "Sifariş uğurla yaradıldı."
            };
        }
    }
}
