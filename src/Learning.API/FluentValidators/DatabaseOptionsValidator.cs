using FluentValidation;

using Learning.API.Options;

namespace Learning.API.FluentValidators;

public sealed class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(x => x.CommandTimeout)
            .Must(x => x > 10)
            .WithMessage("CommandTimeout should be more than 10");
    }
}
