using Application.Exceptions.Card;
using Application.Features.Cards.Commands;
using Application.Interfaces.Repositories;
using Application.Validators.Features.Cards.Commands;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cards.Queries
{
    public class GetCardByNumberQueryResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CardHolderName { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationtYear { get; set; }
        public string CVC { get; set; }
    }

    public class GetCardByNumberQuery : IRequest<GetCardByNumberQueryResponse>
    {
        public string Number { get; set; }

        public GetCardByNumberQuery()
        {
        }

    }

    public class GetCardByNumberQueryHandler : IRequestHandler<GetCardByNumberQuery, GetCardByNumberQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<GetCardByNumberQueryHandler> _logger;
        public GetCardByNumberQueryHandler(IMapper mapper, ICardRepository cardRepository, ILogger<GetCardByNumberQueryHandler> logger)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public async Task<GetCardByNumberQueryResponse> Handle(GetCardByNumberQuery request, CancellationToken cancellationToken)
        {
            GetCardByNumberQueryResponse result = new GetCardByNumberQueryResponse();

            try
            {
                GetCardByNumberValidator validator = new GetCardByNumberValidator();
                ValidationResult validation = validator.Validate(request.Number);
                if (!validation.IsValid)
                {
                    string message = string.Empty;
                    foreach (var failure in validation.Errors)
                    {
                        message += failure.ErrorMessage + ". ";
                    }
                    throw new CardDataNotValidException(message);
                }

                result = new GetCardByNumberQueryResponse();
                var item = await _cardRepository.GetByNumberAsync(request.Number);
                if (item == null)
                    throw new NoCardNumberException(request.Number);

                result = _mapper.Map<GetCardByNumberQueryResponse>(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($" {this.ToString()}: {ex.Message}");
                throw;
            }

            return result;
        }
    }

}
