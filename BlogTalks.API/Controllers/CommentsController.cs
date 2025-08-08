using BlogTalks.API.DTOs;


using BlogTalks.Application.Comments.Commands;
using BlogTalks.Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        // GET: api/<CommentsController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var comments = await _mediator.Send(new GetAllRequest());
            return Ok(comments);
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}", Name = "GetCommentById")]
        public async Task<ActionResult> GetComment([FromRoute] GetByIdRequest request)
        {
            var comment = await _mediator.Send(request);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with ID {request.id} not found." });
            }
            return Ok(comment);
        }

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}", Name = "UpdateCommentById")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateByIdRequest request)
        {
            var response = await _mediator.Send(new UpdateByIdRequest(id, request.Text));
            if (response == null)
            {
                return NotFound(new { message = $"Comment with ID {id} not found." });
            }
            return Ok(response);

        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}", Name = "DeleteCommentById")]
        public async Task<ActionResult> Delete([FromRoute] DeleteByIdRequest request)
        {
            var comment = await _mediator.Send(request);

            if (comment == null)
            {
                return NotFound(new { message = $"Comment with ID {request.id} not found." });
            }
            return Ok(comment);
        }

        // GET api/<CommentsController>/blogpost/5
        [HttpGet("/api/blogposts/{blogPostId}/comments")]
        public async Task<ActionResult> GetCommentsByBlogPostId([FromRoute] int blogPostId)
        {
            var comments = await _mediator.Send(new GetByBlogPostIdRequest(blogPostId));
            if (comments == null)
            {
                return NotFound(new { message = $"No comments found for Blog Post ID {blogPostId}." });
            }
            return Ok(comments);
        }
    }
}
