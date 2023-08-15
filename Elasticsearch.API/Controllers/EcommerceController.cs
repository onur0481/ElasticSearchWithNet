using Elasticsearch.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EcommerceController : ControllerBase
    {
        private readonly EcomerceRepository _ecomerceRepository;

        public EcommerceController(EcomerceRepository ecomerceRepository)
        {
            _ecomerceRepository = ecomerceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _ecomerceRepository.TermQuery(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            return Ok(await _ecomerceRepository.TermsQuery(customerFirstNameList));
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _ecomerceRepository.PrefixQuery(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _ecomerceRepository.RangeQuery(fromPrice,toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _ecomerceRepository.MatchAllQuery());
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page, int size)
        {
            return Ok(await _ecomerceRepository.PaginationQuery(page,size));
        }

        [HttpGet]
        public async Task<IActionResult> WilcardQuery(string customerFullName)
        {
            return Ok(await _ecomerceRepository.WilcardQuery(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> FuzyQuery(string customerFullName)
        {
            return Ok(await _ecomerceRepository.FuzyQuery(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string category)
        {
            return Ok(await _ecomerceRepository.MatchQueryFullText(category));
        }

        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerFullName)
        {
            return Ok(await _ecomerceRepository.MatchBoolPrefixQueryFullText(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchPhraseQueryFullText(string customerFullName)
        {
            return Ok(await _ecomerceRepository.MatchPhraseQueryFullText(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExample(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer)
        {
            return Ok(await _ecomerceRepository.CompoundQueryExample(cityName, taxfulTotalPrice, categoryName, menufacturer));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
        {
            return Ok(await _ecomerceRepository.CompoundQueryExampleTwo(customerFullName));
        }
    }
}
