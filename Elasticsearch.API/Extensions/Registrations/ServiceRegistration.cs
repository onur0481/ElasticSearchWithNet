using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.API.Repositories;
using Elasticsearch.API.Services;

namespace Elasticsearch.API.Extensions.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName!, password!));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);

            services.AddScoped<ProductService>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<EcomerceRepository>();
        }
    }
}
