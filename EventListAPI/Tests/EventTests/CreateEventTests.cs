using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.CreateEvent;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Tests.EventTests
{
    public class CreateEventTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly CreateEventUseCase _useCase;

        public CreateEventTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _useCase = new CreateEventUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidEvent_ReturnsEventId()
        {
            // Arrange
            var eventDto = new CreateEventDto(
                "Test Event",
                "Description",
                DateTime.Now.AddDays(1),
                "Test Location",
                "Test Category",
                100,
                null
            );

            var command = new CreateEventCommand(eventDto);
            var newEvent = eventDto.MapIntoEventEntity();

            _eventRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Event>()))
                .ReturnsAsync((Event e) =>
                {
                    e.Id = 1;
                    return e; 
                });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidEvent_ThrowsValidationException()
        {
            // Arrange
            var eventDto = new CreateEventDto(
                string.Empty, 
                "Description",
                DateTime.Now.AddDays(-1), 
                "Test Location",
                "Test Category",
                100,
                null
            );

            var command = new CreateEventCommand(eventDto);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*"); 
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}
