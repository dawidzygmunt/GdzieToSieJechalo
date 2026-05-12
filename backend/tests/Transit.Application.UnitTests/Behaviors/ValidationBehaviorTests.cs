using FluentAssertions;
using FluentValidation;
using MediatR;
using Transit.Application.Common.Behaviors;
using ValidationException = Transit.Application.Common.Exceptions.ValidationException;

namespace Transit.Application.UnitTests.Behaviors;

file record TestCommand(string Name) : IRequest<string>;

file class TestValidator : AbstractValidator<TestCommand>
{
    public TestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nazwa jest wymagana.");
    }
}

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Behavior_przepuszcza_poprawne_zadanie()
    {
        var validators = new List<IValidator<TestCommand>> { new TestValidator() };
        var behavior = new ValidationBehavior<TestCommand, string>(validators);
        var result = await behavior.Handle(new TestCommand("Test"), ct => Task.FromResult("OK"), CancellationToken.None);
        result.Should().Be("OK");
    }

    [Fact]
    public async Task Behavior_rzuca_ValidationException_dla_nieprawidlowego_zadania()
    {
        var validators = new List<IValidator<TestCommand>> { new TestValidator() };
        var behavior = new ValidationBehavior<TestCommand, string>(validators);
        var act = async () => await behavior.Handle(new TestCommand(""), ct => Task.FromResult("OK"), CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.ContainsKey("Name"));
    }
}
