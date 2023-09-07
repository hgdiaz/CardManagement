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

    public class AddEditCardValidator : AbstractValidator<Card>
    {
        public AddEditCardValidator()
        {
            RuleFor(request => request.Number)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Card number es required");
            RuleFor(x => x.Number)
                .Must(x => new RegularExpressionAttribute(ValidationConstants.CardNumberRegex)
                .IsValid(x?.TrimEnd())).WithMessage("Card number must be 15 digits"); ;

            RuleFor(request => request.CardHolderName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Card holder name es required");
            RuleFor(request => request.CardHolderName)
                .Length(1, 200).WithMessage("Card holder name must have between 1 and 200 characters");

            RuleFor(request => request.ExpirationMonth)
                .InclusiveBetween(1, 12).WithMessage("Expiration month must be between 1 and 12");

            RuleFor(request => request.ExpirationtYear)
                .InclusiveBetween(2023, 2050).WithMessage("Expiration year must be between 2023 and 2050");

            RuleFor(request => request.CVC)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Card CVC es required");
            RuleFor(x => x.CVC)
                .Must(x => new RegularExpressionAttribute(ValidationConstants.CardCVCRegex)
                .IsValid(x?.TrimEnd())).WithMessage("Card CVC must be 3 digits");


        }

    }
}
