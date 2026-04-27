using OrderTrackingApp.Backend.Application.Orders.Queries.GetById;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Queries.GetById;

public class GetOrderByIdQueryValidatorTests
{
    private readonly GetOrderByIdQueryValidator _sut = new();

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(long.MaxValue)]
    public void Validate_WithPositiveId_ShouldPass(long id)
    {
        var result = _sut.Validate(new GetOrderByIdQuery(id));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositiveId_ShouldFail(long id)
    {
        var result = _sut.Validate(new GetOrderByIdQuery(id));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetOrderByIdQuery.Id));
    }
}
