using Domain.Entities;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using System.ComponentModel.Design;
using Application.Exceptions.Card;
using FluentValidation.Results;
using Application.Validators.Features.Cards.Commands;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cards.Commands
{
    public partial class EditCardCommand : IRequest<EditCardResult>
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CardHolderName { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationtYear { get; set; }
        public string CVC { get; set; }
    }

    public class EditCardResult
    {
        public bool Success { get; set; }
    }


    public class EditCardHandler : IRequestHandler<EditCardCommand, EditCardResult>
    {
        private readonly IMapper _mapper;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<EditCardHandler> _logger;

        public EditCardHandler(IMapper mapper, ICardRepository cardRepository, ILogger<EditCardHandler> logger)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public async Task<EditCardResult> Handle(EditCardCommand command, CancellationToken cancellationToken)
        {
            int affected = 0;
            try
            {                
                //check if the cards exists
                var exists = await _cardRepository.GetByIdAsync(command.Id);
                if (exists == null)
                    throw new NoCardIdException();

                //check if there's already a card with the same number and different id
                exists = await _cardRepository.GetByNumberAsync(command.Number);
                if ((exists != null) && (exists.Id != command.Id))
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

                affected = await _cardRepository.UpdateCardAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($" {this.ToString()}: {ex.Message}");
                throw;
            }

            if (affected > 0) { return new EditCardResult() { Success = true }; }
            else { return new EditCardResult() { Success = false }; }
        }
    }
}
