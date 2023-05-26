using Microsoft.Extensions.Configuration;

namespace ConsoleApp1Test;

public class Test : ITest
{
    public Test(IConfiguration config)
    {
        var t = config.GetSection("test").Get<string>();
        Console.WriteLine(t);
    }

    public void show()
    {
        Console.WriteLine("show");
    }
}