using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.Services;

namespace Elasticsearch.Web.Extensions.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName!, password!));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);

            services.AddScoped<BlogRepository>();
            services.AddScoped<BlogService>();
            services.AddScoped<EcommerceRepository>();
            services.AddScoped<EcommerceService>();
        }
    }
}
