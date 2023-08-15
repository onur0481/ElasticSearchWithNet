using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.API.Models.EcommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repositories
{
    public class EcomerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";
        public EcomerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<Ecommerce>> TermQuery(string customerFirstName)
        {
            //var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
            //foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            //var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Suffix("keyword"), customerFirstName)));

            var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
            var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName).Query(termQuery));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> TermsQuery(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new();

            customerFirstNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Terms = new TermsQueryField(terms.AsReadOnly())
            //};

            //var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName).Query(termsQuery));


            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstName
            .Suffix("keyword"))
            .Terms(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> PrefixQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName)
            .Query(q => q
            .Prefix(p => p
            .Field(f => f.CustomerFullName
            .Suffix("keyword"))
            .Value(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s.Index(indexName)
            .Query(q => q
            .Range(r => r
            .NumberRange(nr => nr
            .Field(f => f.TaxFulTotalPrice)
            .Gte(fromPrice)
            .Lte(toPrice)
            ))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Size(100)
            .Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> PaginationQuery(int page, int size)
        {
            var pageFrom = (page - 1) * size;

            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Size(size).From(pageFrom)
            .Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> WilcardQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .Wildcard(w => w
            .Field(f => f
            .Suffix("keyword"))
            .Wildcard(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> FuzyQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .Fuzzy(f => f
            .Field(fl => fl.CustomerFullName
            .Suffix("keyword"))
            .Value(customerFullName)
            .Fuzziness(new Fuzziness(2))))
            .Sort(sort => sort
            .Field(fi => fi.TaxFulTotalPrice, new FieldSort { Order = SortOrder.Desc})));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> MatchQueryFullText(string categoryName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .Match(m => m
            .Field(f => f
            .Category)
            .Query(categoryName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> MatchBoolPrefixQueryFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .MatchBoolPrefix(m => m
            .Field(f => f
            .CustomerFullName)
            .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> MatchPhraseQueryFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
            .Query(q => q
            .MatchPhrase(m => m
            .Field(f => f
            .CustomerFullName)
            .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> CompoundQueryExample(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field("geoip.city_name")
                                .Value(cityName)))
                        .MustNot(mn => mn
                            .Range(r => r
                                .NumberRange(nb => nb
                                    .Field(f => f.TaxFulTotalPrice)
                                    .Lte(taxfulTotalPrice))))
                        .Should(s => s
                            .Term(tr => tr
                                .Field(f => f
                                    .Category.Suffix("keyword"))
                                    .Value(categoryName)))
                        .Filter(fi => fi
                            .Term(tr => tr
                                .Field("manufacturer.keyword")
                                .Value(menufacturer))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<Ecommerce>> CompoundQueryExampleTwo(string customerFullName)
        {
            var result = await _client.SearchAsync<Ecommerce>(s => s
            .Index(indexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(m => m
                                .Field(f => f.CustomerFullName)
                                .Query(customerFullName))
                            .Prefix(p => p
                                .Field(f => f.CustomerFullName.Suffix("keyword"))
                                .Value(customerFullName))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

    }
}
