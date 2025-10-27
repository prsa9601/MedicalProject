namespace MedicalProject.Services.Product
{
    public interface IProductService
    {
    }
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client;
        }
    }
}
