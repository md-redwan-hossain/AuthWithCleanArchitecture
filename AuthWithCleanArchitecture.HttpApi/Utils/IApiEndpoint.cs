namespace AuthWithCleanArchitecture.HttpApi.Utils;

public interface IApiEndpoint
{
    static abstract void DefineRoutes(IEndpointRouteBuilder routes);
}