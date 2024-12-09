using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.RoleCases.Commands.CreateRole
{
    public class CreateRoleUseCase : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var newRole = new Role
            {
                Name = request.Name
            };

            await _roleRepository.AddAsync(newRole, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);

            return newRole.Id;
        }
    }
}
