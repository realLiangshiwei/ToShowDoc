using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{
    [Command(Name = "l", FullName = "list", Description = "show all project")]
    [HelpOption("-h|--help")]
    public class ListCommand
    {
        private readonly IShowDocStore _showDocStore;

        public ListCommand(IShowDocStore showDocStore)
        {
            _showDocStore = showDocStore;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
            var result = await _showDocStore.GetAll();

            if (result.Any())
            {
                foreach (var docEntity in result)
                {
                    Console.WriteLine(docEntity.Name);
                    Console.WriteLine($"-------- Id : {docEntity.Id}");
                    Console.WriteLine($"-------- ApiKey : {docEntity.AppKey}");
                    Console.WriteLine($"-------- ApiToken : {docEntity.AppToken}");
                    Console.WriteLine($"-------- SwaggerUrl : {docEntity.SwaggerUrl}");
                    Console.WriteLine($"-------- ShowDocUrl : {docEntity.ShowDocUrl}");
                }
            }
            else
            {
                Console.WriteLine("No items exist");
            }

        }
    }
}
