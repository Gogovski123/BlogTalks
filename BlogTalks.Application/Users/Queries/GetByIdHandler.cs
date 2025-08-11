using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Queries
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id);
            
            if (user == null)
            {
                return null;
            }
            return Task.FromResult(new GetByIdResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }
    }
}
