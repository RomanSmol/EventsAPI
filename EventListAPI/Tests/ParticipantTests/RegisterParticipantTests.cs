using Application.DTOs.ParticipantDTOs;
using Application.Interfaces;
using Application.UseCases.ParticipantCases.Commands.RegisterParticipant;
using Application.Validators;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Tests.ParticipantTests
{
    public class RegisterParticipantTests
    {
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ParticipantValidator _participantValidator;
        private readonly RegisterParticipantUseCase _useCase;

        public RegisterParticipantTests()
        {
            _mockParticipantRepository = new Mock<IParticipantRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockMapper = new Mock<IMapper>();
            _participantValidator = new ParticipantValidator();

            _useCase = new RegisterParticipantUseCase(
                _mockParticipantRepository.Object,
                _mockPasswordHasher.Object,
                _mockMapper.Object,
                _participantValidator
            );
        }

        [Fact]
        public async Task Handle_ValidParticipant_ReturnsParticipantReadDto()
        {
            // Arrange
            var command = new RegisterParticipantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Password = "password123"
            };

            var participant = new Participant
            {
                Id = 1,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = "hashedPassword",
                RegistrationDate = DateTime.UtcNow
            };

            _mockMapper.Setup(m => m.Map<Participant>(command)).Returns(participant);
            _mockPasswordHasher.Setup(h => h.HashPassword(command.Password)).Returns(participant.Password);
            _mockParticipantRepository.Setup(repo =>
                repo.AddAsync(It.IsAny<Participant>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockParticipantRepository.Setup(repo =>
                repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map<ParticipantReadDto>(participant))
                .Returns(new ParticipantReadDto
                {
                    Id = participant.Id,
                    FirstName = participant.FirstName,
                    LastName = participant.LastName,
                    Email = participant.Email
                });

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(participant.Id);
            result.FirstName.Should().Be(participant.FirstName);
            result.LastName.Should().Be(participant.LastName);
            result.Email.Should().Be(participant.Email);
        }

        [Fact]
        public async Task Handle_InvalidParticipant_ThrowsValidationException()
        {
            // Arrange
            var command = new RegisterParticipantCommand
            {
                FirstName = "",
                LastName = "Doe",
                Email = "invalidemail",
                Password = ""
            };

            var participant = new Participant
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email
            };

            _mockMapper.Setup(m => m.Map<Participant>(command)).Returns(participant);

            // Act
            Func<Task> act = async () => await _useCase.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Validation failed: *");
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = new RegisterParticipantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Password = "password123" 
            };

            _mockParticipantRepository.Setup(repo =>
                repo.AddAsync(It.IsAny<Participant>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () =>
            {
                await _useCase.Handle(command, CancellationToken.None);
            };

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Cannot pass null model to Validate. (Parameter 'instanceToValidate')");
        }
    }
}
