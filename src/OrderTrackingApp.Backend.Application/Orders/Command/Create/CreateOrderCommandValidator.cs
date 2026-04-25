using FluentValidation;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator() =>
        RuleFor(v => v.Description)
            .NotEmpty()
            .MaximumLength(200);
}
