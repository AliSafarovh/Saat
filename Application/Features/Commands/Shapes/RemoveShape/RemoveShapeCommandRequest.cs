﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.Shapes.RemoveShape
{
    public class RemoveShapeCommandRequest : IRequest<RemoveShapeCommandResponse>
    {
        public  int Id { get; set; }
    }
}
