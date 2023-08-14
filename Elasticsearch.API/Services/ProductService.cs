using Elasticsearch.API.DTOs;
using Elasticsearch.API.Repositories;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseDTO<ProductDTO>> SaveAsync(ProductCreateDTO request)
        {
            var response = await _repository.SaveAsync(request.CreateProduct());

            if(response == null)
            {
                return ResponseDTO<ProductDTO>.Fail(new List<string> { "kayıt esnasında bir hata geldi" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDTO<ProductDTO>.Succes(response.CreateDTO(), HttpStatusCode.Created);
        }
    }
}
