using Elasticsearch.API.Models;

namespace Elasticsearch.API.DTOs
{
    public record ProductFeatureDTO(int Width, int Height, EColor Color)
    {
    }
}
