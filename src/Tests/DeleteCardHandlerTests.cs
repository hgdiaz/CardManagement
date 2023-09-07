using Application.Exceptions.Card;
using Application.Features.Cards.Commands;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DeleteCardHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingCardId_DeletesCardAndReturnsSuccess()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<DeleteCardHandler>>();

            var handler = new DeleteCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var existingCardId = 1; 

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCardId)).ReturnsAsync(new Card());
            cardRepositoryMock.Setup(repo => repo.DeleteCardAsync(existingCardId)).ReturnsAsync(1);

            var deleteCommand = new DeleteCardCommand { Id = existingCardId };

            // Act
            var result = await handler.Handle(deleteCommand, CancellationToken.None);

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
            var loggerMock = new Mock<ILogger<DeleteCardHandler>>();

            var handler = new DeleteCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var nonExistentCardId = 0; 

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentCardId)).ReturnsAsync((Card)null);

            var deleteCommand = new DeleteCardCommand { Id = nonExistentCardId };

            // Act & Assert
            await Assert.ThrowsAsync<NoCardIdException>(() => handler.Handle(deleteCommand, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ErrorInDeletion_ThrowsException()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();
            var loggerMock = new Mock<ILogger<DeleteCardHandler>>();

            var handler = new DeleteCardHandler(mapperMock.Object, cardRepositoryMock.Object, loggerMock.Object);

            var existingCardId = 1; 

            cardRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCardId)).ReturnsAsync(new Card());
            cardRepositoryMock.Setup(repo => repo.DeleteCardAsync(existingCardId)).Throws(new Exception("Database error"));

            var deleteCommand = new DeleteCardCommand { Id = existingCardId };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(deleteCommand, CancellationToken.None));
        }
    }
}
