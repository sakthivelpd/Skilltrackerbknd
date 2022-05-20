using FluentValidation.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Profile.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IMediator mediator, ILogger<ProfileController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet(Name = "IsAlive")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<int> IsAlive()
        {
            _logger.LogInformation("Checking Profile API live status.");
            string response = "Profile API is in good health.";
            return Ok(response);
        }

        [HttpPost(Name = "AddProfile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> AddProfile([FromBody] AddProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateProfile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> UpdateProfile([FromBody] UpdateProfileCommand command,
            [FromHeader(Name = "x-userid")] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                var errors = new List<ValidationFailure> { new ValidationFailure("", "UserId header missing") };
                throw new FluentValidation.ValidationException(errors);
            }

            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProfileVM), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProfileVM>> GetProfile(string id)
        {
            var query = new GetProfileQuery();
            query.EmpId = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }       
    }
}
