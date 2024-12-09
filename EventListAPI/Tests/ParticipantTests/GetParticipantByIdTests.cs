using Application.DTOs.ParticipantDTOs;
using Application.UseCases.ParticipantCases.Commands.GetParticipantById;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

public class GetParticipantByIdUseCaseTests
{
    private readonly Mock<IParticipantRepository> _mockParticipantRepository;
    private readonly GetParticipantByIdUseCase _useCase;

    public GetParticipantByIdUseCaseTests()
    {
        _mockParticipantRepository = new Mock<IParticipantRepository>();
        _useCase = new GetParticipantByIdUseCase(_mockParticipantRepository.Object);
    }

    [Fact]
    public async Task Handle_ParticipantExists_ReturnsParticipantDto()
    {
        // Arrange
        var participantId = 1;
        var participantEntity = new Participant
        {
            Id = participantId,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.UtcNow.AddYears(-30),
            Email = "john.doe@example.com",
            RegistrationDate = DateTime.UtcNow
        };

        _mockParticipantRepository.Setup(repo => repo.GetByIdAsync(participantId))
            .ReturnsAsync(participantEntity);

        var command = new GetParticipantByIdCommand(participantId);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(participantId);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("john.doe@example.com");

        // Verify that the repository method was called
        _mockParticipantRepository.Verify(repo => repo.GetByIdAsync(participantId), Times.Once);
    }

    [Fact]
    public async Task Handle_ParticipantDoesNotExist_ThrowsException()
    {
        // Arrange
        var participantId = 1;
        _mockParticipantRepository.Setup(repo => repo.GetByIdAsync(participantId))
            .ReturnsAsync((Participant)null);

        var command = new GetParticipantByIdCommand(participantId);

        // Act
        Func<Task> act = async () =>
        {
            await _useCase.Handle(command, CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Participant not found");

        // Verify that the repository method was called
        _mockParticipantRepository.Verify(repo => repo.GetByIdAsync(participantId), Times.Once);
    }
}
