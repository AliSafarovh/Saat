using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandlar : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        public Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
