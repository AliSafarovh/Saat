﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Products.GetAllProduct
{
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
