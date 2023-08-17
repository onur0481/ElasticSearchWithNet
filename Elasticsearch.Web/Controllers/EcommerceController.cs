using Elasticsearch.Web.Services;
using Elasticsearch.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Web.Controllers
{
    public class EcommerceController : Controller
    {
        private readonly EcommerceService _service;

        public EcommerceController(EcommerceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageViewModel)
        {
            var (ecommerceList, totalCount, pageLinkCount) = await _service.SearchAsync(searchPageViewModel.SearchViewModel, searchPageViewModel.Page,
                searchPageViewModel.PageSize);

            searchPageViewModel.List = ecommerceList;
            searchPageViewModel.TotalCount = totalCount;
            searchPageViewModel.PageLinkCount = pageLinkCount;

            return View(searchPageViewModel);
        }
    }
}
