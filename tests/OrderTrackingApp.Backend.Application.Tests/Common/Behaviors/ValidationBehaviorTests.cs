using FluentValidation;
using FluentValidation.Results;

using MediatR;

using OrderTrackingApp.Backend.Application.Common.Behaviors;

namespace OrderTrackingApp.Backend.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    public record TestRequest(string Value) : IRequest<string>;

    [Fact]
    public async Task Handle_WhenValidatorIsNull_ShouldCallNext()
    {
        var sut = new ValidationBehavior<TestRequest, string>();
        var nextCalled = false;

        Task<string> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult("ok");
        }

        var result = await sut.Handle(new TestRequest("v"), Next, CancellationToken.None);

        result.Should().Be("ok");
        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenValidationSucceeds_ShouldCallNext()
    {
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
                 .Returns(new ValidationResult());
        var sut = new ValidationBehavior<TestRequest, string>(validator);

        var result = await sut.Handle(new TestRequest("v"), _ => Task.FromResult("ok"), CancellationToken.None);

        result.Should().Be("ok");
    }
}
