using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.GetEventByName;
using AutoFixture;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.EventTests
{
    public class GetEventByNameTests
    {
        private readonly IFixture _fixture;
        private readonly IMapper _mapper;

        public GetEventByNameTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EventProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetEventByName_GivenNullName_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new GetEventByNameCommand(null!);

            var eventRepositoryMock = new Mock<IEventRepository>();
            var handler = new GetEventByNameUseCase(eventRepositoryMock.Object, _mapper);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("*EventName*");
        }

        [Fact]
        public async Task GetEventByName_GivenInvalidName_ThrowsNotFoundException()
        {
            // Arrange
            var command = _fixture.Create<GetEventByNameCommand>();

            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock
                .Setup(s => s.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Domain.Models.Event)null);

            var handler = new GetEventByNameUseCase(eventRepositoryMock.Object, _mapper);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Event with name '{command.EventName}' not found.");

            eventRepositoryMock.Verify(v => v.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetEventByName_GivenValidName_ReturnSuccessfullyMappedDto()
        {
            // Arrange
            var command = _fixture.Create<GetEventByNameCommand>();
            var eventEntity = _fixture.Create<Event>();

            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(eventEntity);

            var handler = new GetEventByNameUseCase(eventRepositoryMock.Object, _mapper);
            var dto = _mapper.Map<GetEventDto>(eventEntity);

            // Act 
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(dto);

            eventRepositoryMock.Verify(v => v.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }


    }
}
