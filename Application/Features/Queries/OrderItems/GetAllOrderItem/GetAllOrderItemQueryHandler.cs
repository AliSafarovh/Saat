using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.OrderItems.GetAllOrderItem
{
    public class GetAllOrderItemQueryHandler : IRequestHandler<GetAllOrderItemQueryRequest, GetAllOrderItemQueryResponse>
    {
        public Task<GetAllOrderItemQueryResponse> Handle(GetAllOrderItemQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
