using Elasticsearch.API.Models;

namespace Elasticsearch.API.DTOs
{
    public record ProductDTO(string Id, string Name, decimal Price, int Stock, ProductFeatureDTO? Feature)
    {     
       
    }
}
