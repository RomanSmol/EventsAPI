using Application.UseCases.ParticipantCases.Commands.GetParticipantById;
using Application.UseCases.ParticipantCases.Commands.LoginParticipant;
using Application.UseCases.ParticipantCases.Commands.RegisterParticipant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParticipantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterParticipant([FromBody] RegisterParticipantCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetParticipantById), new { participantId = result.Id }, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginParticipant([FromBody] LoginParticipantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{participantId}")]
        public async Task<IActionResult> GetParticipantById(int participantId)
        {
            var query = new GetParticipantByIdCommand(participantId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
