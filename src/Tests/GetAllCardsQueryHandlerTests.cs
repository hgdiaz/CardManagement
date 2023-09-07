using System.Collections.Generic;
using Application.Features.Cards.Queries;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Moq;
using Xunit;

namespace Tests
{
    public class GetAllCardsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsListOfGetAllCardsResponse()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var cardRepositoryMock = new Mock<ICardRepository>();

            var handler = new GetAllCardsQueryHandler(mapperMock.Object, cardRepositoryMock.Object);

            var cardsFromRepository = new List<Card>
            {
                new Card { Id = 1, Number = "111111111111111", CardHolderName = "John", ExpirationMonth = 1, ExpirationtYear = 2025, CVC = "123" },
                new Card { Id = 1, Number = "222222222222222", CardHolderName = "Homer", ExpirationMonth = 2, ExpirationtYear = 2025, CVC = "123" },
            };

            var expectedResponse = new List<GetAllCardsResponse>
            {
                new GetAllCardsResponse { Id = 1, Number = "111111111111111", CardHolderName = "John" },
                new GetAllCardsResponse { Id = 2, Number = "222222222222222", CardHolderName = "Homer" }
            };

            cardRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cardsFromRepository);
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<GetAllCardsResponse>>(cardsFromRepository))
                .Returns(expectedResponse);

            var query = new GetAllCardsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse, result);
        }
    }
}
