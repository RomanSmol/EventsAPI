using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.RoleCases.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<Unit>
    {
        public int RoleId { get; }

        public DeleteRoleCommand(int roleId)
        {
            RoleId = roleId;
        }
    }
}
