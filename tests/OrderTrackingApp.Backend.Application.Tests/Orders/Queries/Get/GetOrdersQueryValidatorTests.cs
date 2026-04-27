using OrderTrackingApp.Backend.Application.Orders.Queries.Get;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Queries.Get;

public class GetOrdersQueryValidatorTests
{
    private readonly GetOrdersQueryValidator _sut = new();

    [Theory]
    [InlineData(1, 10)]
    [InlineData(5, 50)]
    [InlineData(null, null)]
    public void Validate_WithValidOrNullPaging_ShouldPass(int? pageNumber, int? pageSize)
    {
        var result = _sut.Validate(new GetOrdersQuery(pageNumber, pageSize));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    public void Validate_WithInvalidPageNumber_ShouldFail(int pageNumber, int pageSize)
    {
        var result = _sut.Validate(new GetOrdersQuery(pageNumber, pageSize));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetOrdersQuery.PageNumber));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -10)]
    public void Validate_WithInvalidPageSize_ShouldFail(int pageNumber, int pageSize)
    {
        var result = _sut.Validate(new GetOrdersQuery(pageNumber, pageSize));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetOrdersQuery.PageSize));
    }
}
