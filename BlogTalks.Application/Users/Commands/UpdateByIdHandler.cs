using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public class UpdateByIdHandler : IRequestHandler<UpdateByIdRequest, UpdateByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UpdateByIdResponse> Handle(UpdateByIdRequest request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id);

            if (user == null)
            {
                return null;
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Password = request.Password;

            _userRepository.Update(user);

            return Task.FromResult(new UpdateByIdResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            });
        }
    }
}
