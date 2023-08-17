using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Services
{
    public class EcommerceService
    {
        private readonly EcommerceRepository _repository;

        public EcommerceService(EcommerceRepository repository)
        {
            _repository = repository;
        }

        public async Task<(List<EcommerceViewModel>,long totalCount, long pageLinkCount)> SearchAsync(EcommerceSearchViewModel searchViewModel, int page, int pageSize)
        {
            var (ecommerceList, totalCount) = await _repository.SearchAsync(searchViewModel, page, pageSize);

            var pageLinkCountCalculate = totalCount % pageSize;
            long pageLinkCount = 0;

            if(pageLinkCountCalculate == 0)
            {
                pageLinkCount = totalCount / pageSize;
            }
            else
            {
                pageLinkCount = (totalCount / pageSize)+1;
            }

            var ecommerceListViewModel = ecommerceList.Select(x => new EcommerceViewModel()
            {
                Category = String.Join(",", x.Category),
                CustomerFullName = x.CustomerFullName,
                CustomerFirstName = x.CustomerFirstName,
                CustomerLastName = x.CustomerLastName,
                OrderDate = x.OrderDate.ToShortDateString(),
                OrderId = x.OrderId,
                Gender = x.Gender.ToLower(),
                Id = x.Id,
                TaxFulTotalPrice = x.TaxFulTotalPrice,
            }).ToList();

            return (ecommerceListViewModel, totalCount, pageLinkCount);
        }
    }
}
