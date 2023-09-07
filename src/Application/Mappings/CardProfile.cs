using Application.Features.Cards.Queries;
using Application.Features.Cards.Commands;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<GetAllCardsResponse, Card>().ReverseMap();
            CreateMap<AddCardCommand, Card>().ReverseMap();
            CreateMap<EditCardCommand, Card>().ReverseMap();
            CreateMap<GetCardByNumberQueryResponse, Card>().ReverseMap();
        }
    }
}
