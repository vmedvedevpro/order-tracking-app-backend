using FluentValidation;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Update;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.Order)
            .NotNull();

        RuleFor(v => v.Order.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}
