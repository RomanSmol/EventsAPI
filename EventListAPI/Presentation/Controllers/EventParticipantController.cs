using Application.UseCases.EventParticipantCases.Commands.GetEventParticipants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventParticipantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventParticipantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{eventId}/participants")]
        public async Task<IActionResult> GetEventParticipants(int eventId)
        {
            var query = new GetEventParticipantsCommand(eventId);
            var participants = await _mediator.Send(query);
            return Ok(participants);
        }
    }
}