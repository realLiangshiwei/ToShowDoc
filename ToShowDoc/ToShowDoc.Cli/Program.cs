using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace ToShowDoc
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
