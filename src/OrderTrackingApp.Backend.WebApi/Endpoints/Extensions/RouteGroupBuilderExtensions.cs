namespace OrderTrackingApp.Backend.WebApi.Endpoints.Extensions;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapGroup(this RouteGroupBuilder group, string name, Action<RouteGroupBuilder> mapEndpoints)
    {
        mapEndpoints(group.MapGroup(name).WithTags(name).WithOpenApi());
        return group;
    }
}
