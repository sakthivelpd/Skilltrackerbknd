using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;
        private readonly ISearchService _searchService;

        public AdminController(IMediator mediator, ILogger<AdminController> logger, ISearchService searchService)
        {
            _mediator = mediator;
            _logger = logger;
            _searchService = searchService;
        }

        [HttpGet(Name = "IsAlive")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<int> IsAlive()
        {
            var response = "Admin API is in good health.";
            _logger.LogInformation(response);
            return Ok(response);
        }

        [HttpPost("search", Name = "Search")]
        public async Task<IEnumerable<Profile>> Search(SearchProfileQuery query)
        {
            return  await _mediator.Send(query);
        }

        [HttpGet("getall", Name = "GetAll")]
        public async Task<IEnumerable<Profile>> GetAll()
        {
            return await _searchService.GetProfiles();
        }
    }
}
