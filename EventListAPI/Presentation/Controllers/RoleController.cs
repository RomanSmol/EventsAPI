using Application.UseCases.RoleCases.Commands.CreateRole;
using Application.UseCases.RoleCases.Commands.DeleteRole;
using Application.UseCases.RoleCases.Commands.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var command = new CreateRoleCommand(roleName);
            var roleId = await _mediator.Send(command);
            return Ok(new { Id = roleId });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var query = new GetAllRolesCommand();
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var command = new DeleteRoleCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}