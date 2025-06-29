using Application.Features.Commands.Products.CreateProduct;
using Application.Repositories.Colors;
using Application.Repositories.Products;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IColorReadRepository _colorReadRepository;

    public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IColorReadRepository colorReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _colorReadRepository = colorReadRepository;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DiscountPrice = request.DiscountPrice,
            BestSeller = request.BestSeller,
            BrandId = request.BrandId,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            GenderId = request.GenderId,
            ShapeId = request.ShapeId,
            ProductColors = new List<ProductColor>()
        };

        if (request.ColorIds != null && request.ColorIds.Any())
        {
            foreach (var colorId in request.ColorIds)
            {
                product.ProductColors.Add(new ProductColor
                {
                    ColorId = colorId
                });
            }
        }


        await _productWriteRepository.AddAsync(product);
        await _productWriteRepository.SaveChangesAsync();
        return new CreateProductCommandResponse();
    }
}
