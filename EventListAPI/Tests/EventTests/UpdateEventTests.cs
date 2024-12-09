using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.UpdateEvent;
using Application.Validators;
using AutoFixture;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.EventTests
{
    public class UpdateEventTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly UpdateEventUseCase _useCase;

        public UpdateEventTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior()); 

            _eventRepositoryMock = new Mock<IEventRepository>();
            _useCase = new UpdateEventUseCase(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidEvent_UpdatesSuccessfully()
        {
            // Arrange
            var updateDto = _fixture.Build<UpdateEventDto>().With(e => e.EventDate, DateTime.Now.AddDays(1)) .Create();
            var existingEvent = _fixture.Build<Event>().With(e => e.EventDate, DateTime.Now.AddDays(1)) .Create();

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(updateDto.Id)).ReturnsAsync(existingEvent);
            _eventRepositoryMock.Setup(repo => repo.UpdateAsync(existingEvent)).Returns(Task.CompletedTask);

            var command = new UpdateEventCommand(updateDto);

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(updateDto.Id), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task Handle_EventNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var updateDto = _fixture.Create<UpdateEventDto>();
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(updateDto.Id)).ReturnsAsync((Event)null!);
            var command = new UpdateEventCommand(updateDto);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Event with ID {updateDto.Id} not found.");
            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(updateDto.Id), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidEvent_ThrowsValidationException()
        {
            // Arrange
            var updateDto = _fixture.Build<UpdateEventDto>()
                .With(x => x.Name, string.Empty) // Генерируем DTO с пустым именем
                .Create();

            var existingEvent = _fixture.Build<Event>()
                .With(x => x.Name, string.Empty) // Сущность с пустым именем
                .Create();

            _eventRepositoryMock
                .Setup(repo => repo.GetByIdAsync(updateDto.Id))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock
                .Setup(repo => repo.UpdateAsync(existingEvent))
                .Returns(Task.CompletedTask);

            var command = new UpdateEventCommand(updateDto);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Name*"); // Проверяем, что ошибка связана с именем

            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(updateDto.Id), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }

    }
}
