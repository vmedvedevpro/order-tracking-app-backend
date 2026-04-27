using OrderTrackingApp.Backend.Application.Orders.Command.Update;
using OrderTrackingApp.Backend.Domain.Entities;
using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Command.Update;

public class UpdateOrderCommandValidatorTests
{
    private readonly UpdateOrderCommandValidator _sut = new();

    private static Order CreateOrder(string description = "desc") =>
        new() { OrderNumber = 1, Description = description, Status = OrderStatus.Created };

    [Fact]
    public void Validate_WithValidOrder_ShouldPass()
    {
        var command = new UpdateOrderCommand(CreateOrder());

        var result = _sut.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyDescription_ShouldFail(string? description)
    {
        var command = new UpdateOrderCommand(CreateOrder(description!));

        var result = _sut.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.EndsWith(nameof(Order.Description)));
    }

    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ShouldFail()
    {
        var command = new UpdateOrderCommand(CreateOrder(new string('x', 201)));

        var result = _sut.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.EndsWith(nameof(Order.Description)));
    }
}
