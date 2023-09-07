using Domain.Entities;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using FluentValidation.Results;
using Application.Exceptions.Card;
using Application.Validators.Features.Cards.Commands;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cards.Commands
{
    public partial class AddCardCommand : IRequest<AddCardResult>
    {
        public string Number { get; set; }
        public string CardHolderName { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationtYear { get; set; }
        public string CVC { get; set; }
    }

    public class AddCardResult
    {
        public int Id { get; set; }
    }


    public class AddCardHandler : IRequestHandler<AddCardCommand, AddCardResult>
    {
        private readonly IMapper _mapper;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<AddCardHandler> _logger;

        public AddCardHandler(IMapper mapper, ICardRepository cardRepository, ILogger<AddCardHandler> logger)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public async Task<AddCardResult> Handle(AddCardCommand command, CancellationToken cancellationToken)
        {
            Card card = new Card();
            try
            {
                //check if there's already a card with the same number
                var exists = await _cardRepository.GetByNumberAsync(command.Number);
                if (exists != null)
                    throw new CardExistsException(command.Number);

                //validate the card
                var item = _mapper.Map<Card>(command);
                AddEditCardValidator validator = new AddEditCardValidator();
                ValidationResult validation = validator.Validate(item);
                if (!validation.IsValid)
                {
                    string message = string.Empty;
                    foreach (var failure in validation.Errors)
                    {
                        message += failure.ErrorMessage + ". ";
                    }
                    throw new CardDataNotValidException(message);
                }

                card = await _cardRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($" {this.ToString()}: {ex.Message}");
                throw;
            }

            return new AddCardResult() { Id = card.Id };
        }
    }
}
