using Application.Repositories.ProductImages;
using Application.Services;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommandRequest, UpdateProductImageCommandResponse>
    {
        private readonly IProductImageReadRepository _productImageReadRepository;
        private readonly IProductImageWriteRepository _productImageWriteRepository;
        private readonly IFileService _fileService;

        public UpdateProductImageCommandHandler(
            IProductImageReadRepository productImageReadRepository,
            IProductImageWriteRepository productImageWriteRepository,
            IFileService fileService)
        {
            _productImageReadRepository = productImageReadRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _fileService = fileService;
        }

        public async Task<UpdateProductImageCommandResponse> Handle(UpdateProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var productImage = await _productImageReadRepository.GetByIdAsync(request.Id);
            if (productImage == null)
                throw new Exception("Şəkil tapılmadı.");

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

            if (request.File != null)
            {
                var oldFilePath = Path.Combine(rootPath, Path.GetFileName(productImage.ImagePath));

                var newFileName = await _fileService.UpdateAsync(request.File, oldFilePath, rootPath);

                productImage.ImagePath = Path.Combine("images", "products", newFileName).Replace("\\", "/");
            }
            else throw new Exception("Yeni şəkil faylı göndərilməyib.");

            productImage.ProductId = request.ProductId;

            await _productImageWriteRepository.SaveChangesAsync();

            return new();
        }
    }
}
