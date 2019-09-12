using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Commands;

namespace ToShowDoc
{
    [Command(Name = "ToShowDoc", Description = "ToShowDoc CLI Tool")]
    [HelpOption("-h|--help")]
    [Subcommand(typeof(AddCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(UpdateCommand))]
    [Subcommand(typeof(DeleteCommand))]
    [Subcommand(typeof(SyncCommand))]
    public class App
    {

        public void OnExecute(CommandLineApplication app)
        {
            if (!app.Arguments.Any())
            {
                app.ShowHelp();
            }
        }
    }
}
