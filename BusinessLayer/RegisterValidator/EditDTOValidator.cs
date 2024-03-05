using DTO_s_Layer.DTO_Model;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

public class EditDTOValidator : AbstractValidator<EditDTO>
{
    public EditDTOValidator()
    {
        // Email validation
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not a valid email address.")
            .Custom((email, context) =>
            {
                if (email != email.ToLower())
                    context.AddFailure("Email must be in lowercase.");
            });

        // Name validation
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(name => !name.Any(char.IsDigit)).WithMessage("Name cannot contain numbers.");

        // Password validation

        RuleFor(user => user.ProfileImg)
           .NotEmpty().WithMessage("Image is required.")
           .Must(BeValidBase64Image).WithMessage("Image must be a valid JPG or PNG image.");
    }

    private bool BeValidBase64Image(string base64Image)
    {
        if (string.IsNullOrWhiteSpace(base64Image))
            return false;

        var base64Data = Regex.Replace(base64Image, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
        return Regex.IsMatch(base64Data, @"^(\/9j\/|iVBORw0KGgo)");
    }
}
