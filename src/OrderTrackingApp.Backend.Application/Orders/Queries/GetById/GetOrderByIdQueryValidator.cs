using FluentValidation;

namespace OrderTrackingApp.Backend.Application.Orders.Queries.GetById;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator() =>
        RuleFor(v => v.Id)
            .GreaterThan(0);
}
