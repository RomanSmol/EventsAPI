using Application.Exceptions;
using Application.UseCases.EventCases.Commands.CancelEventParticipation;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using MediatR;

public class CancelEventParticipationTests
{
    private readonly Mock<IEventParticipantRepository> _eventParticipantRepositoryMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IParticipantRepository> _participantRepositoryMock;
    private readonly CancelEventParticipationUseCase _useCase;

    public CancelEventParticipationTests()
    {
        _eventParticipantRepositoryMock = new Mock<IEventParticipantRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _participantRepositoryMock = new Mock<IParticipantRepository>();
        _useCase = new CancelEventParticipationUseCase(
            _eventParticipantRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _participantRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_CancelsParticipation()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        var eventParticipant = new EventParticipant
        {
            EventId = eventId,
            ParticipantId = participantId
        };

        _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _participantRepositoryMock.Setup(repo => repo.ExistsAsync(participantId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _eventParticipantRepositoryMock.Setup(repo => repo.GetByEventAndParticipantIdAsync(eventId, participantId, It.IsAny<CancellationToken>())).ReturnsAsync(eventParticipant);
        _eventParticipantRepositoryMock.Setup(repo => repo.DeleteAsync(eventParticipant, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var command = new CancelEventParticipationCommand(eventId, participantId);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _eventParticipantRepositoryMock.Verify(repo => repo.DeleteAsync(eventParticipant, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EventNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var command = new CancelEventParticipationCommand(eventId, participantId);

        // Act
        Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Event with ID {eventId} not found.");
    }

    [Fact]
    public async Task Handle_ParticipantNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _participantRepositoryMock.Setup(repo => repo.ExistsAsync(participantId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var command = new CancelEventParticipationCommand(eventId, participantId);

        // Act
        Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Participant with ID {participantId} not found.");
    }

    [Fact]
    public async Task Handle_EventParticipationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _participantRepositoryMock.Setup(repo => repo.ExistsAsync(participantId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _eventParticipantRepositoryMock.Setup(repo => repo.GetByEventAndParticipantIdAsync(eventId, participantId, It.IsAny<CancellationToken>())).ReturnsAsync((EventParticipant)null);

        var command = new CancelEventParticipationCommand(eventId, participantId);

        // Act
        Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"EventParticipation with ID EventId: {eventId}, ParticipantId: {participantId} not found.");
    }
}
