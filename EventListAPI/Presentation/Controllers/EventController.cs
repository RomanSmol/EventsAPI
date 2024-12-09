using Application.DTOs.EventDTOs;
using Application.DTOs.ParticipantDTOs;
using Application.UseCases.EventCases.Commands.CreateEvent;
using Application.UseCases.EventCases.Commands.CancelEventParticipation;
using Application.UseCases.EventCases.Commands.AddParticipantToEvent;
using Application.UseCases.EventCases.Commands.DeleteEvent;
using Application.UseCases.EventCases.Commands.FilteredEvents;
using Application.UseCases.EventCases.Commands.GetAllEvents;
using Application.UseCases.EventCases.Commands.GetEventById;
using Application.UseCases.EventCases.Commands.GetEventByName;
using Application.UseCases.EventCases.Commands.UpdateEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Получение всех событий
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _mediator.Send(new GetAllEventsCommand());
            return Ok(result);
        }

        // Получение события по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var result = await _mediator.Send(new GetEventByIdCommand(id));
            return Ok(result);
        }

        // Создание нового события
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var command = new CreateEventCommand(createEventDto);

            var eventId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEventById), new { id = eventId }, eventId);
        }

        // Обновление события
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto updateEventDto)
        {
            var command = new UpdateEventCommand(updateEventDto with { Id = id });
            await _mediator.Send(command);
            return NoContent();
        }

        // Удаление события
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _mediator.Send(new DeleteEventCommand(id));
            return NoContent();
        }

        // Фильтр событий
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEvents([FromQuery] EventFiltersDto filterDto)
        {
            var query = new GetFilteredEventsCommand(filterDto);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Отмена участия
        [HttpPost("{eventId}/cancel-participation/{participantId}")]
        public async Task<IActionResult> CancelEventParticipation(int eventId, int participantId)
        {
            var command = new CancelEventParticipationCommand(eventId, participantId);
            await _mediator.Send(command);
            return NoContent();
        }

        // Получение события по имени
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var query = new GetEventByNameCommand(name);
            var eventDto = await _mediator.Send(query);

            return Ok(eventDto);
        }

        [HttpPost("{eventId}/participants/{participantId}")]
        public async Task<IActionResult> AddParticipantToEvent(int eventId, int participantId)
        {
            var command = new AddParticipantToEventCommand
            {
                EventId = eventId,
                ParticipantId = participantId
            };

            await _mediator.Send(command);
            return Ok();
        }
    }
}
