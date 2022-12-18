using FluentValidation;
using VersionChanger.Models;

namespace VersionChanger.Validators;

public class PackageValidator : AbstractValidator<Package>
{
    public PackageValidator()
    {
        RuleFor(x => x.StartsWithPattern).NotEmpty();
        RuleFor(x => x.Version).NotEmpty();
    }
}