using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Shared.Constants;

namespace Application.Validators.Features.Cards.Commands
{
    public class GetCardByNumberValidator : AbstractValidator<string>
    {
        public GetCardByNumberValidator()
        {
            RuleFor(request => request)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Card number es required");
            RuleFor(x => x)
                .Must(x => new RegularExpressionAttribute(ValidationConstants.CardNumberRegex)
                .IsValid(x?.TrimEnd())).WithMessage("Card number must be 15 digits"); ;



        }

    }
}
