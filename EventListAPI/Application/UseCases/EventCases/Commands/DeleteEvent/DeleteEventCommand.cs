using MediatR;

namespace Application.UseCases.EventCases.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<Unit> 
    {
        public int Id { get; }

        public DeleteEventCommand(int id)
        {
            Id = id;
        }
    }
}
