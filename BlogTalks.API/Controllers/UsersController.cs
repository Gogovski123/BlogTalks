using BlogTalks.Application.Users.Commands;
using BlogTalks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogTalks.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
       
        public async Task<ActionResult> Get()
        {
            _logger.LogInformation("Fetching all users.");
            var users = await _mediator.Send(new GetAllRequest());
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult> GetUser([FromRoute] int id)
        {
            _logger.LogInformation($"Fetching user with ID {id}.");
            var request = new GetByIdRequest(id);
            var user = await _mediator.Send(request);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        // GET api/<CommentsController>/users/5
        [HttpGet("api/users/by_email")]
        public async Task<ActionResult> GetByEmail([FromQuery] string email)
        {
            _logger.LogInformation($"Fetching user with email {email}.");
            var request = new GetByEmailRequest(email);
            var user = await _mediator.Send(request);
            if (user == null)
            {
                return NotFound(new { message = $"User with email {email} not found." });
            }
            return Ok(user);
        }

        // POST api/<UsersController>/register
        
        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] RegisterRequest request)
        {
            _logger.LogInformation($"Registering user with email {request.Email}.");
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return BadRequest(new { message = "User already exists with this email." });
            }
            return Ok(response);
        }

        // POST api/<UsersController>/login
       
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"User login attempt with email {request.Email}.");
            var response = await _mediator.Send(request);
            
            return Ok(response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}", Name = "UpdateUserById")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
            _logger.LogInformation($"Updating user with ID {id}.");
            var response = await _mediator.Send(new UpdateByIdRequest(id, request.Name, request.Email, request.Password));
            if (response == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(response);
        }

        [HttpDelete("{id}", Name = "DeleteUserById")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting user with ID {id}.");
            var response = await _mediator.Send(new DeleteByIdRequest(id));
            return Ok(response);
        }

    }
}
