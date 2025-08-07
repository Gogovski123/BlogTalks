using BlogTalks.API.DTOs;
using BlogTalks.Application.BlogPost.Queries;
using BlogTalks.Application.BlogPosts.Commands;
using BlogTalks.Application.BlogPosts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        // GET: api/<BlogPostsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetAll()
        {
            var blogPosts = await _mediator.Send(new GetAllRequest());
            return Ok(blogPosts);
        }

        // GET api/<BlogPostsController>/5
        [HttpGet("{id}", Name = "GetBlogPostById")]
        public async Task<ActionResult> Get([FromRoute] GetByIdRequest request)
        {
            var blogPost = await _mediator.Send(request);
            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID {request.id} not found." });
            }
            return Ok(blogPost);
        }

        // POST api/<BlogPostsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
            var response = _mediator.Send(new UpdateByIdRequest(id, request.Title, request.Text, request.Tags));
            if (response == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        // DELETE api/<BlogPostsController>/5
        [HttpDelete("{id}", Name = "DeleteBlogPostById")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var blogPost = await _mediator.Send(new DeleteByIdRequest(id));

            if (blogPost == null)
            {
                return NotFound(new { message = $"Blog post with ID { id } not found." });
            }

            return NoContent();
        }
    }
}
