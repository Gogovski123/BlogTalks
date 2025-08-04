using BlogTalks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class DeleteByIdHandler : IRequestHandler<DeleteCommentByIdCommand, DeleteByIdResponse>
    {
        private readonly FakeDataStore _fakeDataStore;

        public DeleteByIdHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }

        public async Task<DeleteByIdResponse> Handle(DeleteCommentByIdCommand request, CancellationToken cancellationToken)
        {
            var comment = await _fakeDataStore.DeleteCommentById(request.id);

            if (comment == null)
            {
                return null;
            }

            return new DeleteByIdResponse
            {
                
            };
        }
    }
}
