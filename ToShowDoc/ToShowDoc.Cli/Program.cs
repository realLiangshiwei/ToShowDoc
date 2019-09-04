using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using ToShowDoc.Core;

namespace ToShowDoc
{
    class Program
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton(PhysicalConsole.Singleton)
                .AddShowDocCore()
                .BuildServiceProvider();

            var app = new CommandLineApplication<App>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);
            try
            {
                return app.Execute(args);
            }
            catch (UnrecognizedCommandParsingException)
            {

                return 0;
            }

        }

    }
}
