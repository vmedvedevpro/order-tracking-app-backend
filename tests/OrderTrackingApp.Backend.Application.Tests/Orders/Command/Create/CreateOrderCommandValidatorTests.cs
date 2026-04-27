using OrderTrackingApp.Backend.Application.Orders.Command.Create;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Command.Create;

public class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommandValidator _sut = new();

    [Fact]
    public void Validate_WithValidDescription_ShouldNotHaveErrors()
    {
        var command = new CreateOrderCommand("Some valid description");

        var result = _sut.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyOrNullDescription_ShouldFail(string? description)
    {
        var command = new CreateOrderCommand(description!);

        var result = _sut.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateOrderCommand.Description));
    }

    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ShouldFail()
    {
        var command = new CreateOrderCommand(new string('a', 201));

        var result = _sut.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateOrderCommand.Description));
    }

    [Fact]
    public void Validate_WhenDescriptionAtMaxLength_ShouldPass()
    {
        var command = new CreateOrderCommand(new string('a', 200));

        var result = _sut.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
