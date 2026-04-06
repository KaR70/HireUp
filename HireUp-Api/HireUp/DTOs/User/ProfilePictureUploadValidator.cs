using FluentValidation;
using HireUp.DTOs.Common;
using HireUp.Settings;

namespace HireUp.DTOs.User;

public class ProfilePictureUploadValidator : AbstractValidator<ProfilePictureUpload>
{
    public ProfilePictureUploadValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignatureValidator())
            .NotNull();

        RuleFor(x => x.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName.ToLower());
                return FileSettings.AllowedImagesExtensions.Contains(extension);
            })
            .WithMessage("File extension is not allowed")
            .When(x => x.Image is not null);
    }
}