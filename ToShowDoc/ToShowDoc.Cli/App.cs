using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Cli.Commands;

namespace ToShowDoc.Cli
{
    [Command(Name = "ToShowDoc", Description = "ToShowDoc CLI Tool")]
    [HelpOption("-h|--help")]
    [Subcommand(typeof(AddCommand))]
    public class App
    {

        public void OnExecute(CommandLineApplication app)
        {

        }
    }
}
