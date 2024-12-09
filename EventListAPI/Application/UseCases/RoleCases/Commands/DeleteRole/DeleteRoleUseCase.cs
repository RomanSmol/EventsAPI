using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.RoleCases.Commands.DeleteRole
{
    public class DeleteRoleUseCase : IRequestHandler<DeleteRoleCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");
            }

            await _roleRepository.DeleteAsync(role, cancellationToken);
            return Unit.Value; 
        }
    }
}