using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.GetEventById;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.EventTests
{
    public class GetEventByIdTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly GetEventByIdUseCase _useCase;

        public GetEventByIdTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _useCase = new GetEventByIdUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsEventDto()
        {
            var eventId = 1;
            var eventEntity = new Event
            {
                Id = eventId,
                Name = "Test Event",
                Location = "Test Location",
                EventDate = DateTime.Now,
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetByIdAsync(eventId))
                .ReturnsAsync(eventEntity);

            var command = new GetEventByIdCommand(eventId);

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(eventEntity.Id);
            result.Name.Should().Be(eventEntity.Name);
            result.Location.Should().Be(eventEntity.Location);

            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        }


        [Fact]
        public async Task Handle_InvalidId_ThrowsException()
        {
            // Arrange
            var eventId = 999;
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((Event)null);
            var command = new GetEventByIdCommand(eventId);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Event not found");
            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        }
    }
}
