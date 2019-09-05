using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{

    [Command(Name = "sync", Description = "sync doc")]
    [HelpOption("-h|--help")]
    public class SyncCommand
    {
        private readonly IShowDocStore _showDocStore;

        public SyncCommand(IShowDocStore showDocStore)
        {
            _showDocStore = showDocStore;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
           
        }
    }
}
