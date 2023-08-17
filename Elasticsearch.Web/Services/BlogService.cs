using Elasticsearch.Web.Models;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel blogCreateViewModel)
        {

            Blog blog = new();
            blog.Title = blogCreateViewModel.Title;
            blog.UserId = Guid.NewGuid();
            blog.Content = blogCreateViewModel.Content;
            blog.Tags = blogCreateViewModel.Tags.Split(",");

            var isCreatedBlog = await _blogRepository.Save(blog);
            
            return isCreatedBlog != null;
        }

        public async Task<List<BlogViewModel>> SearchAsync(string searchText)
        {
            var blogList = await _blogRepository.SearchAsync(searchText);

            return blogList.Select(blog => new BlogViewModel() {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Tags = String.Join(",", blog.Tags),
                Created = blog.Created.ToShortDateString(),
                UserId = blog.UserId.ToString(),
            }).ToList();
        }
    }
}
