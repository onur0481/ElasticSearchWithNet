using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string _indexName = "blog";
        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Blog?> Save(Blog blog)
        {
            blog.Created = DateTime.Now;

            var response = await _client.IndexAsync(blog, x => x.Index(_indexName));

            if(!response.IsValidResponse) return null;

            blog.Id = response.Id;

            return blog;
        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            List<Action<QueryDescriptor<Blog>>> ListQuery = new();

            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();

            Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match(m => m
                                    .Field(f => f.Content)
                                    .Query(searchText));

            Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(mp => mp
                                    .Field(f => f.Title)
                                    .Query(searchText));

            Action<QueryDescriptor<Blog>> tagTerm = (q) => q.Term(t => t.Field(f => f.Tags).Value(searchText));

            if (string.IsNullOrEmpty(searchText))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(titleMatchBoolPrefix);
                ListQuery.Add(tagTerm);
            }

            var result = await _client.SearchAsync<Blog>(
                s => s
                .Index(_indexName)
                .Size(1000)
                    .Query(q => q
                        .Bool(b => b
                            .Should(ListQuery.ToArray()))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToList();
        }
    }
}
