using Application.Exceptions.Card;
using Application.Features.Cards.Queries;
using Application.Interfaces.Repositories;
using Application.Validators.Features.Cards.Commands;
using AutoMapper;
using Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class GetCardByNumberQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCardNumber_ReturnsValidResponse()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<GetCardByNumberQueryHandler>>();

            var handler = new GetCardByNumberQueryHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var validCardNumber = "111111111111111";

            var cardFromRepository = new Card
            {
                Id = 1,
                Number = validCardNumber,
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 25,
                CVC = "123"
            };

            var expectedResponse = new GetCardByNumberQueryResponse
            {
                Id = 1,
                Number = validCardNumber,
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 25,
                CVC = "123"
            };

            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(validCardNumber)).ReturnsAsync(cardFromRepository);
            mapperMock.Setup(mapper => mapper.Map<GetCardByNumberQueryResponse>(cardFromRepository))
                .Returns(expectedResponse);

            var query = new GetCardByNumberQuery { Number = validCardNumber };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse, result);
        }

        //[Fact]
        //public async Task Handle_InvalidCardNumber_ThrowsCardDataNotValidException()
        //{
        //    // Arrange
        //    var mapperMock = new Mock<IMapper>();
        //    var cardRepositoryMock = new Mock<ICardRepository>();
        //    var loggerMock = new Mock<ILogger<GetCardByNumberQueryHandler>>();

        //    var handler = new GetCardByNumberQueryHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

        //    var invalidCardNumber = "123";

        //    var validatorMock = new Mock<GetCardByNumberValidator>();
        //    var validationFailures = new ValidationResult(new[] { new ValidationFailure("Number", "Invalid card number") });
        //    validatorMock.Setup(validator => validator.Validate(invalidCardNumber)).Returns(validationFailures);

        //    var query = new GetCardByNumberQuery { Number = invalidCardNumber };

        //    // Act & Assert
        //    await Assert.ThrowsAsync<CardDataNotValidException>(() => handler.Handle(query, CancellationToken.None));
        //}

        [Fact]
        public async Task Handle_CardNotFound_ThrowsNoCardNumberException()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<GetCardByNumberQueryHandler>>();

            var handler = new GetCardByNumberQueryHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var nonExistentCardNumber = "111111111111111";

            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(nonExistentCardNumber)).ReturnsAsync((Card)null);

            var query = new GetCardByNumberQuery { Number = nonExistentCardNumber };

            // Act & Assert
            await Assert.ThrowsAsync<NoCardNumberException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
