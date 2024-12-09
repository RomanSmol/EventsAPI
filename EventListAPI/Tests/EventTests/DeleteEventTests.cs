using Application.UseCases.EventCases.Commands.DeleteEvent;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.EventTests
{
    public class DeleteEventTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly DeleteEventUseCase _useCase;

        public DeleteEventTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _useCase = new DeleteEventUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingEvent_DeletesSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var existingEvent = new Event { Id = eventId };
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
            _eventRepositoryMock.Setup(repo => repo.DeleteAsync(existingEvent)).Returns(Task.CompletedTask);
            var command = new DeleteEventCommand(eventId);

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(existingEvent), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistentEvent_ThrowsKeyNotFoundException()
        {
            // Arrange
            var eventId = 1;
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

            // Создаем команду с указанием ID
            var command = new DeleteEventCommand(eventId);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Event with ID {eventId} not found.");

            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Event>()), Times.Never);
        }

    }
}
