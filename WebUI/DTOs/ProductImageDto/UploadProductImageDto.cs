namespace WebUI.DTOs.ProductImageDto
{
    public class UploadProductImageDto
    {
        public int ProductId { get; set; }
        public List<IFormFile> Files { get; set; }
    }

}
