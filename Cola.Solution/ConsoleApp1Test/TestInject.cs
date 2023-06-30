using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1Test;

public static class TestInject
{
    public static void AddTestInject(this IServiceCollection services)
    {
        services.AddSingleton<ITest, Test>();
    }
}