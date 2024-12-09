using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.GetAllEvents;
using AutoFixture;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.EventTests
{
    public class GetAllEventsTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Fixture _fixture;
        private readonly GetAllEventsUseCase _useCase;

        public GetAllEventsTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fixture = new Fixture();
            _useCase = new GetAllEventsUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsEventDtos()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var events = _fixture.Build<Event>().With(e => e.EventDate, DateTime.Now.AddDays(1)).CreateMany(5).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);
            var command = new GetAllEventsCommand();

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(events.Select(GetEventDto.MapIntoDto));

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NoEvents_ReturnsEmptyList()
        {
            // Arrange
            var emptyEvents = new List<Event>();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(emptyEvents);

            var command = new GetAllEventsCommand();

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
