using Application.Exceptions.Card;
using Application.Features.Cards.Commands;
using Application.Interfaces.Repositories;
using Application.Mappings;
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
    public class EditCardHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingCardIdAndValidData_UpdatesCardAndReturnsSuccess()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<EditCardHandler>>();

            var handler = new EditCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var existingCardId = 1; // Replace with an existing card ID in your repository

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCardId)).ReturnsAsync(new Card());
            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<string>())).ReturnsAsync((Card)null);
            cardRepositoryMock.Setup(repo => repo.UpdateCardAsync(It.IsAny<Card>())).ReturnsAsync(1);

            var validCommand = new EditCardCommand
            {
                Id = existingCardId,
                Number = "111111111111111",
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 2025,
                CVC = "123"
            };

            var cardToUpdate = new Card
            {
                Id = existingCardId,
                Number = validCommand.Number,
                CardHolderName = validCommand.CardHolderName,
                ExpirationMonth = validCommand.ExpirationMonth,
                ExpirationtYear = validCommand.ExpirationtYear,
                CVC = validCommand.CVC
            };

            mapperMock.Setup(mapper => mapper.Map<Card>(validCommand)).Returns(cardToUpdate);

            // Act
            var result = await handler.Handle(validCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_NonExistentCardId_ThrowsNoCardIdException()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<EditCardHandler>>();

            var handler = new EditCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var nonExistentCardId = 0;

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentCardId)).ReturnsAsync((Card)null);

            var editCommand = new EditCardCommand { Id = nonExistentCardId };

            // Act & Assert
            await Assert.ThrowsAsync<NoCardIdException>(() => handler.Handle(editCommand, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DuplicateCardNumber_ThrowsCardExistsException()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<EditCardHandler>>();

            var handler = new EditCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var existingCardId = 1;
            var existingCardNumber = "111111111111111";

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCardId)).ReturnsAsync(new Card());
            cardRepositoryMock.Setup(repo => repo.GetByNumberAsync(existingCardNumber)).ReturnsAsync(new Card());

            var editCommand = new EditCardCommand
            {
                Id = existingCardId,
                Number = existingCardNumber,
                CardHolderName = "John Doe",
                ExpirationMonth = 12,
                ExpirationtYear = 2025,
                CVC = "123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<CardExistsException>(() => handler.Handle(editCommand, CancellationToken.None));
        }

    }
}
