using Elasticsearch.API.Repositories;
using Elasticsearch.API.Services;
using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));

            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            services.AddSingleton(client);

            services.AddScoped<ProductService>();
            services.AddScoped<ProductRepository>();
        }
    }
}
