using Application.Features.Cards.Commands;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cards.Queries
{
    public class GetAllCardsResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CardHolderName { get; set; }       
    }

    public class GetAllCardsQuery : IRequest<IEnumerable<GetAllCardsResponse>>
    {
        public GetAllCardsQuery()
        {
        }

    }

    public class GetAllCardsQueryHandler : IRequestHandler<GetAllCardsQuery, IEnumerable<GetAllCardsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICardRepository _cardRepository;

        public GetAllCardsQueryHandler(IMapper mapper, ICardRepository cardRepository)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
        }

        public async Task<IEnumerable<GetAllCardsResponse>> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<GetAllCardsResponse> result = new List<GetAllCardsResponse>();
            var list = await _cardRepository.GetAllAsync();
            result = _mapper.Map<IEnumerable<GetAllCardsResponse>>(list);
            return result;
        }
    }

}
