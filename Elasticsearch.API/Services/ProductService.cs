using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repositories;
using Nest;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly ILogger _logger;
        public ProductService(ProductRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseDTO<ProductDTO>> SaveAsync(ProductCreateDTO request)
        {
            var response = await _repository.SaveAsync(request.CreateProduct());

            if (response == null)
            {
                return ResponseDTO<ProductDTO>.Fail(new List<string> { "kayıt esnasında bir hata geldi" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDTO<ProductDTO>.Success(response.CreateDTO(), HttpStatusCode.Created);
        }

        public async Task<ResponseDTO<List<ProductDTO>>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            List<ProductDTO> productListDto = new();

            foreach (var x in products)
            {
                if (x.Feature is null)
                {
                    productListDto.Add(new ProductDTO(x.Id, x.Name, x.Price, x.Stock, null));

                    continue;
                }
               
                    productListDto.Add(new ProductDTO(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDTO(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));
                
            }

            return ResponseDTO<List<ProductDTO>>.Success(productListDto, HttpStatusCode.OK);
        }


        public async Task<ResponseDTO<ProductDTO>> GetByIdAsync(string id)
        {
            var hasProduct = await _repository.GetByIdAsync(id);

            if (hasProduct == null) return ResponseDTO<ProductDTO>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);

            return ResponseDTO<ProductDTO>.Success(hasProduct.CreateDTO(), HttpStatusCode.OK);

        }

        public async Task<ResponseDTO<bool>> UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            var isSuccess = await _repository.UpdateAsync(productUpdateDTO);

            if(!isSuccess) return ResponseDTO<bool>.Fail("ürün güncellenemedi", HttpStatusCode.BadRequest);

            return ResponseDTO<bool>.Success(true, HttpStatusCode.NoContent);
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _repository.DeleteAsync(id);

            if(!deleteResponse.IsValid && deleteResponse.Result == Result.NotFound) return ResponseDTO<bool>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);

            if(!deleteResponse.IsValid)
            {
                _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.ToString());

                return ResponseDTO<bool>.Fail("ürün silinemedi", HttpStatusCode.InternalServerError);
            }

            return ResponseDTO<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
