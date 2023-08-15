namespace Elasticsearch.API.DTOs
{
    public record ProductUpdateDTO(string id,string Name, decimal Price, int Stock, ProductFeatureDTO ProductFeature)
    {
    }
}
