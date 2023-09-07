using Application.Exceptions.Card;
using Application.Features.Cards.Commands;
using Application.Interfaces.Repositories;
using Application.Validators.Features.Cards.Commands;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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
    public class AddCardHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCardData_AddsCardAndReturnsResult()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<AddCardHandler>>();

            var handler = new AddCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var validCommand = new AddCardCommand
            {
                Number = "111111111111111", 
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 2025,
                CVC = "123"
            };

            var cardToAdd = new Card
            {
                Id = 1,
                Number = validCommand.Number,
                CardHolderName = validCommand.CardHolderName,
                ExpirationMonth = validCommand.ExpirationMonth,
                ExpirationtYear = validCommand.ExpirationtYear,
                CVC = validCommand.CVC
            };

            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(validCommand.Number)).ReturnsAsync((Card)null);
            mapperMock.Setup(mapper => mapper.Map<Card>(validCommand)).Returns(cardToAdd);
            cardRepositoryMock.Setup(repo => repo.AddAsync(cardToAdd)).ReturnsAsync(cardToAdd);

            // Act
            var result = await handler.Handle(validCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cardToAdd.Id, result.Id);
        }

        [Fact]
        public async Task Handle_DuplicateCardNumber_ThrowsCardExistsException()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<AddCardHandler>>();

            var handler = new AddCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var existingCardNumber = "111111111111111";

            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(existingCardNumber)).ReturnsAsync(new Card());

            var duplicateCommand = new AddCardCommand
            {
                Number = existingCardNumber,
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 2025,
                CVC = "123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<CardExistsException>(() => handler.Handle(duplicateCommand, CancellationToken.None));
        }

        //[Fact]
        //public async Task Handle_InvalidCardData_ThrowsCardDataNotValidException()
        //{
        //    // Arrange
        //    var mapperMock = new Mock<IMapper>();
        //    var cardRepositoryMock = new Mock<ICardRepository>();
        //    var loggerMock = new Mock<ILogger<AddCardHandler>>();

        //    var handler = new AddCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

        //    var invalidCommand = new AddCardCommand
        //    {
        //        Number = "123",
        //        CardHolderName = "John Doe",
        //        ExpirationMonth = 12,
        //        ExpirationtYear = 2025,
        //        CVC = "123"
        //    };

        //    var invalidCardCommand = new Card
        //    {
        //        Id = 1,
        //        Number = "123",
        //        CardHolderName = "John Doe",
        //        ExpirationMonth = 12,
        //        ExpirationtYear = 2025,
        //        CVC = "123"
        //    };

        //    var validatorMock = new Mock<AddEditCardValidator>();
        //    var validationFailures = new ValidationResult(new[] { new ValidationFailure("Number", "Card number must be 15 digits") });
        //    validatorMock.Setup(validator => validator.Validate(invalidCardCommand)).Returns(validationFailures);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<CardDataNotValidException>(() => handler.Handle(invalidCommand, CancellationToken.None));
        //}
    }
}



