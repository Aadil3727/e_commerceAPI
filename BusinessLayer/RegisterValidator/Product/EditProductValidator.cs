using DTO_s_Layer.DTO_Model.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.RegisterValidator.Product
{
    public class EditProductValidator : AbstractValidator<ProductEditDTO>
    {
        public EditProductValidator() {
            RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(name => !name.Any(char.IsDigit)).WithMessage("Name cannot contain numbers.");

            // Password validation

            RuleFor(user => user.Images)
               .NotEmpty().WithMessage("Image is required.")
               .Must(BeValidBase64Image).WithMessage("Image must be a valid JPG or PNG image.");

            RuleFor(user => user.offerprice)
                .NotEmpty().WithMessage("Offer price is required.")
                .When(user => user.IsOffer)
                .WithMessage("Offer price cannot be empty when offer is marked as true.");

        }

        private bool BeValidBase64Image(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                return false;

            // Optional: Remove the MIME type prefix if present
            var base64Data = Regex.Replace(base64Image, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            return Regex.IsMatch(base64Data, @"^(\/9j\/|iVBORw0KGgo)");
        }
    }
}
