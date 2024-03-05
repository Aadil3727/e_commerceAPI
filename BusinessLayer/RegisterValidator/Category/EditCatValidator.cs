using DTO_s_Layer.DTO_Model;
using DTO_s_Layer.DTO_Model.Category;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

public class EditCatValidator : AbstractValidator<EditCatDTO>
{
    public EditCatValidator()
    {

        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(name => !name.Any(char.IsDigit)).WithMessage("Name cannot contain numbers.");

        RuleFor(user => user.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters")
            .MaximumLength(500).WithMessage("Description must not exceed 200 characters");

        RuleFor(user => user.CatImg)
           .NotEmpty().WithMessage("Image is required.")
           .Must(BeValidBase64Image).WithMessage("Image must be a valid JPG or PNG image.");
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
