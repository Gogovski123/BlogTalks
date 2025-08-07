using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public record DeleteByIdRequest(int id) : IRequest<DeleteByIdResponse>;
    


}
