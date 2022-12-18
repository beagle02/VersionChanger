using FluentValidation;
using VersionChanger.Models;

namespace VersionChanger.Validators;

public class SettingsValidator : AbstractValidator<Settings>
{
    public SettingsValidator()
    {
        RuleFor(x => x.Path).NotEmpty();
        RuleForEach(x => x.Packages).SetValidator(new PackageValidator());
    }
}