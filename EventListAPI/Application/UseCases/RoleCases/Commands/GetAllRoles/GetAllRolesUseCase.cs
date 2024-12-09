using Application.DTOs.RoleDTOs;
using Domain.Interfaces;
using MediatR;
using Application.UseCases.RoleCases;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.RoleCases.Commands.GetAllRoles
{
    public class GetAllRolesUseCase : IRequestHandler<GetAllRolesCommand, IEnumerable<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync(cancellationToken);
            return roles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            });
        }
    }
}
