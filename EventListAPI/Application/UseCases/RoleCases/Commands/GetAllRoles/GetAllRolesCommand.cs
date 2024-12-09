using Application.DTOs.RoleDTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.RoleCases.Commands.GetAllRoles
{
    public record GetAllRolesCommand : IRequest<IEnumerable<RoleDto>>;
}
