using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Web.Models;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Repositories
{
    public class EcommerceRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string indexName = "kibana_sample_data_ecommerce";
        public EcommerceRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }
       

        public async Task<(List<Ecommerce> list, long count)> SearchAsync(EcommerceSearchViewModel viewModel, int page, int pageSize)
        {
            List<Action<QueryDescriptor<Ecommerce>>> listQuery = new();

            if(viewModel is null)
            {
                listQuery.Add(g => g.MatchAll());
                return await CalculateResultSet(page, pageSize, listQuery);
            }

            if (!string.IsNullOrEmpty(viewModel.Category))
            {
                listQuery.Add(q => q.Match(m => m.Field(f => f.Category).Query(viewModel.Category)));
            }

            if (!string.IsNullOrEmpty(viewModel.CustomerFullName))
            {
                listQuery.Add(q => q.Match(m => m.Field(f => f.CustomerFullName).Query(viewModel.CustomerFullName)));
            }

            if (viewModel.OrderDateStart.HasValue)
            {
                listQuery.Add(q => q.Range(r => r.DateRange(dr => dr.Field(f => f.OrderDate).Gte(viewModel.OrderDateStart.Value))));
            }

            if (viewModel.OrderDateEnd.HasValue)
            {
                listQuery.Add(q => q.Range(r => r.DateRange(dr => dr.Field(f => f.OrderDate).Lte(viewModel.OrderDateEnd.Value))));
            }

            if (!string.IsNullOrEmpty(viewModel.Gender))
            {
                listQuery.Add(q => q.Term(t => t.Field(f => f.Gender).Value(viewModel.Gender).CaseInsensitive()));
            }

            if (!listQuery.Any())
            {
                listQuery.Add(g => g.MatchAll());
            }

            

            return await CalculateResultSet(page, pageSize, listQuery);
        }
        private async Task<(List<Ecommerce> list, long count)> CalculateResultSet(int page, int pageSize, List<Action<QueryDescriptor<Ecommerce>>> listQuery)
        {
            var pageFrom = (page - 1) * pageSize;

            var result = await _elasticsearchClient.SearchAsync<Ecommerce>(s => s
            .Index(indexName).Size(pageSize).From(pageFrom)
            .Query(q => q
            .Bool(b => b
            .Must(listQuery.ToArray()))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return (list: result.Documents.ToList(), result.Total);
        }
    }
}
