using Cola.CaConsole;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaException;

public static class ColaExceptionInject
{
    public static IServiceCollection AddColaExceptionSingleton(this IServiceCollection services)
    {
        services.AddSingleton<IColaException, ColaException>();
        ConsoleHelper.WriteInfo("注入类型【 IColaException, ColaException 】");
        return services;
    }
}