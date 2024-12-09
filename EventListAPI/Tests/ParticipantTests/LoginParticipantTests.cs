using Application.DTOs.TokensDTOs;
using Application.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using FluentAssertions;
using Application.UseCases.ParticipantCases.Commands.LoginParticipant;
using System.Linq.Expressions;
using Application.Validators;

namespace Tests.ParticipantTests
{
    public class LoginParticipantTests
    {
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<ITokensGenerator> _mockTokensGenerator;
        private readonly ParticipantLoginValidator _participantValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LoginParticipantUseCase _useCase;

        public LoginParticipantTests()
        {
            _mockParticipantRepository = new Mock<IParticipantRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockTokensGenerator = new Mock<ITokensGenerator>();
            _mockMapper = new Mock<IMapper>();
            _participantValidator = new ParticipantLoginValidator();

            _useCase = new LoginParticipantUseCase(
                _mockParticipantRepository.Object,
                _mockPasswordHasher.Object,
                _mockTokensGenerator.Object,
                _mockMapper.Object,
                _participantValidator
            );
        }

        [Fact]
        public async Task Handle_ParticipantDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var command = new LoginParticipantCommand
            {
                Email = "test@example.com",
                Password = "password123"
            };

            _mockParticipantRepository.Setup(repo =>
                repo.GetByPredicateAsync(
                    It.IsAny<Expression<Func<Participant, bool>>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<Participant>());

            // Act
            Func<Task> act = async () =>
            {
                await _useCase.Handle(command, CancellationToken.None);
            };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Domain.Models.Participant with ID {command.Email} not found.");
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var command = new LoginParticipantCommand
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var participant = new Participant
            {
                Id = 1,
                Email = "test@example.com",
                Password = "hashedPassword",
                RegistrationDate = DateTime.UtcNow,
                Role = new Role { Name = "User" }
            };

            _mockParticipantRepository.Setup(repo =>
                repo.GetByPredicateAsync(
                    It.IsAny<Expression<Func<Participant, bool>>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<Participant> { participant });

            _mockPasswordHasher.Setup(p => p.VerifyHashedPassword(participant.Password, command.Password))
                .Returns(false);

            // Act
            Func<Task> act = async () =>
            {
                await _useCase.Handle(command, CancellationToken.None);
            };

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage("Wrong password");
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var command = new LoginParticipantCommand
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var participant = new Participant
            {
                Id = 1,
                Email = "test@example.com",
                Password = "hashedPassword",
                RegistrationDate = DateTime.UtcNow,
                Role = new Role { Name = "User" }
            };

            var accessToken = new Token { Value = "access_token", Expires = DateTime.UtcNow.AddMinutes(30) };
            var refreshToken = new Token { Value = "refresh_token", Expires = DateTime.UtcNow.AddDays(7) };

            _mockParticipantRepository.Setup(repo =>
                repo.GetByPredicateAsync(
                    It.IsAny<Expression<Func<Participant, bool>>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<Participant> { participant });

            _mockPasswordHasher.Setup(p => p.VerifyHashedPassword(participant.Password, command.Password)).Returns(true);
            _mockTokensGenerator.Setup(t => t.GenerateAccessToken(participant, new List<string> { participant.Role.Name })).Returns(accessToken);
            _mockTokensGenerator.Setup(t => t.GenerateRefreshToken()).Returns(refreshToken);

            // Act
            var result = await _useCase.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be(accessToken.Value);
            result.RefreshToken.Should().Be(refreshToken.Value);
            result.ExpiresIn.Should().Be(accessToken.Expires);
        }
    }
}
