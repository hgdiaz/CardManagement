using Domain.Entities;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Validators.Features.Cards.Commands;
using FluentValidation.Results;
using Application.Exceptions.Card;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cards.Commands
{
    public partial class DeleteCardCommand : IRequest<DeleteCardResult>
    {
        public int Id { get; set; }
    }

    public class DeleteCardResult
    {        
        public bool Success { get; set; }
    }


    public class DeleteCardHandler : IRequestHandler<DeleteCardCommand, DeleteCardResult>
    {
        private readonly IMapper _mapper;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<DeleteCardHandler> _logger;

        public DeleteCardHandler(IMapper mapper, ICardRepository cardRepository, ILogger<DeleteCardHandler> logger)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public async Task<DeleteCardResult> Handle(DeleteCardCommand command, CancellationToken cancellationToken)
        {
            int affected = 0;
            try
            {
                //check if the cards exists
                var exists = await _cardRepository.GetByIdAsync(command.Id);
                if (exists == null)
                    throw new NoCardIdException();

                affected = await _cardRepository.DeleteCardAsync(command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($" {this.ToString()}: {ex.Message}");
                throw;
            }

            if (affected > 0) { return new DeleteCardResult() { Success = true }; }
            else { return new DeleteCardResult() { Success = false }; }

        }
    }
}
