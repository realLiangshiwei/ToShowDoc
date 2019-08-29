using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace ToShowDoc.Cli
{
    class Program
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton(PhysicalConsole.Singleton)
                .BuildServiceProvider();

            var app = new CommandLineApplication<App>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            return app.Execute(args);
        }

    }
}
