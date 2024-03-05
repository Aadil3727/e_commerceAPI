using DTO_s_Layer.DTO_Model;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
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
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,8}$")
            .WithMessage("Password must be up to 8 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character.");

        RuleFor(user => user.ProfileImg)
           .NotEmpty().WithMessage("Image is required.")
           .Must(BeValidBase64Image).WithMessage("Image must be a valid JPG or PNG image.");
    }

    private bool BeValidBase64Image(string base64Image)
    {
        if (string.IsNullOrWhiteSpace(base64Image))
            return false;

        // Optional: Remove the MIME type prefix if present
        var base64Data = Regex.Replace(base64Image, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

        // Check if the base64 string, after removing the MIME type, starts with the headers for JPG or PNG images
        // This regex looks for the start of base64-encoded data that typically represents JPG or PNG images
        // Adjust the pattern to match the specific characteristics of your base64 data if necessary
        return Regex.IsMatch(base64Data, @"^(\/9j\/|iVBORw0KGgo)");
    }
}
