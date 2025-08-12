using BlogTalks.Application.Users.Commands;
using BlogTalks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogTalks.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<UsersController>
        [HttpGet]
       
        public async Task<ActionResult> Get()
        {
            var users = await _mediator.Send(new GetAllRequest());
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult> GetUser([FromRoute] int id)
        {
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
            var response = await _mediator.Send(request);
            
            return Ok(response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}", Name = "UpdateUserById")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
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
            var response = await _mediator.Send(new DeleteByIdRequest(id));
            return Ok(response);
        }

    }
}
