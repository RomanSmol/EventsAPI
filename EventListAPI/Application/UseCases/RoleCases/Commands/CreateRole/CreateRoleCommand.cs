using MediatR;

namespace Application.UseCases.RoleCases.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<int>
    {
        public string Name { get; set; }

        public CreateRoleCommand(string name)
        {
            Name = name;
        }
    }
}