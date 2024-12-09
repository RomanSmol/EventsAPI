using Application.Exceptions;
using Application.UseCases.EventCases.Commands.AddParticipantToEvent;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

public class AddParticipantToEventTests
{
    private readonly Mock<IEventParticipantRepository> _mockEventParticipantRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IParticipantRepository> _mockParticipantRepository;
    private readonly AddParticipantToEventUseCase _useCase;

    public AddParticipantToEventTests()
    {
        _mockEventParticipantRepository = new Mock<IEventParticipantRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockParticipantRepository = new Mock<IParticipantRepository>();

        _useCase = new AddParticipantToEventUseCase(
            _mockEventParticipantRepository.Object,
            _mockEventRepository.Object,
            _mockParticipantRepository.Object
        );
    }

    [Fact]
    public async Task Handle_EventAndParticipantExist_AddsEventParticipant()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _mockEventRepository.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockParticipantRepository.Setup(repo => repo.ExistsAsync(participantId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var command = new AddParticipantToEventCommand
        {
            EventId = eventId,
            ParticipantId = participantId
        };
        Func<Task> act = async () =>
        {
            await _useCase.Handle(command, CancellationToken.None);
        };

        // Assert
        await act.Should().NotThrowAsync();
        _mockEventParticipantRepository.Verify(
            repo => repo.AddAsync(It.Is<EventParticipant>(ep =>
                ep.EventId == eventId &&
                ep.ParticipantId == participantId &&
                ep.RegistrationDate <= DateTime.UtcNow
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_EventDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _mockEventRepository.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var command = new AddParticipantToEventCommand
        {
            EventId = eventId,
            ParticipantId = participantId
        };

        // Act
        Func<Task> act = async () =>
        {
            await _useCase.Handle(command, CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Event with ID {eventId} not found.");
    }

    [Fact]
    public async Task Handle_ParticipantDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = 1;
        var participantId = 2;

        _mockEventRepository.Setup(repo => repo.ExistsAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockParticipantRepository.Setup(repo => repo.ExistsAsync(participantId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new AddParticipantToEventCommand
        {
            EventId = eventId,
            ParticipantId = participantId
        };

        // Act
        Func<Task> act = async () =>
        {
            await _useCase.Handle(command, CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Participant with ID {participantId} not found.");
    }
}
