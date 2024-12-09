using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.FilteredEvents;
using AutoFixture;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.EventTests
{
    public class GetFilteredEventsTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Fixture _fixture;
        private readonly GetFilteredEventsUseCase _useCase;

        public GetFilteredEventsTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _useCase = new GetFilteredEventsUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_NoFilters_ReturnsAllEvents()
        {
            // Arrange
            var events = _fixture.CreateMany<Event>(5).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            var command = new GetFilteredEventsCommand(new EventFiltersDto());

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(events.Select(GetEventDto.MapIntoDto));

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_FilterByEventDate_ReturnsFilteredEvents()
        {
            // Arrange
            var eventDate = DateTime.Today;
            var events = _fixture.Build<Event>().With(e => e.EventDate, eventDate).CreateMany(3).Concat(_fixture.Build<Event>()
                                                 .With(e => e.EventDate, eventDate.AddDays(1)).CreateMany(2)).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);
            var command = new GetFilteredEventsCommand(new EventFiltersDto { EventDate = eventDate });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().OnlyContain(e => e.EventDate == eventDate);

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_FilterByLocation_ReturnsFilteredEvents()
        {
            // Arrange
            var location = "New York";
            var events = _fixture.Build<Event>().With(e => e.Location, location).CreateMany(2).Concat(_fixture.Build<Event>()
                                                 .With(e => e.Location, "Los Angeles").CreateMany(3)).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            var command = new GetFilteredEventsCommand(new EventFiltersDto { Location = location });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().OnlyContain(e => e.Location == location);

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_FilterByCategory_ReturnsFilteredEvents()
        {
            // Arrange
            var category = "Music";
            var events = _fixture.Build<Event>().With(e => e.Category, category).CreateMany(3).Concat(_fixture.Build<Event>()
                                                 .With(e => e.Category, "Sports").CreateMany(2)).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);
            var command = new GetFilteredEventsCommand(new EventFiltersDto { Category = category });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().OnlyContain(e => e.Category == category);

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_CombinedFilters_ReturnsFilteredEvents()
        {
            // Arrange
            var eventDate = DateTime.Today;
            var location = "New York";
            var events = _fixture.Build<Event>().With(e => e.EventDate, eventDate).With(e => e.Location, location).CreateMany(2).Concat(_fixture.CreateMany<Event>(3)).ToList();

            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            var command = new GetFilteredEventsCommand(new EventFiltersDto
            {
                EventDate = eventDate,
                Location = location
            });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().OnlyContain(e => e.EventDate == eventDate && e.Location == location);

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NoMatches_ReturnsEmpty()
        {
            // Arrange
            var events = _fixture.CreateMany<Event>(5).ToList();
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            var command = new GetFilteredEventsCommand(new EventFiltersDto
            {
                EventDate = DateTime.Today.AddYears(1) 
            });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
