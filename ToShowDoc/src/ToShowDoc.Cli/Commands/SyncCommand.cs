using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Parser;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{

    [Command(Name = "sync", Description = "sync doc")]
    [HelpOption("-h|--help")]
    public class SyncCommand
    {
        private readonly IShowDocStore _showDocStore;
        private static readonly HttpClient HttpClient;

        static SyncCommand()
        {
            HttpClient = new HttpClient();
        }

        [Required]
        [Option("-n|--name", CommandOptionType.SingleValue, Description = "Project Name")]
        public string Name { get; set; }

        public SyncCommand(IShowDocStore showDocStore)
        {
            _showDocStore = showDocStore;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
            var showDoc = (await _showDocStore.GetAll()).FirstOrDefault(x => x.Name == Name);

            if (showDoc == null)
            {
                Console.WriteLine($"Not Found Project {Name}");
                return;
            }

            try
            {
                var docStr = await HttpClient.GetStringAsync(showDoc.SwaggerUrl);

                var document = SwaggerParser.ParseString(docStr);
                var request = document.ToShowDocRequest();

                foreach (var item in request)
                {
                    await ShowDocClient.UpdateByApi(showDoc, item);
                }

                Console.WriteLine("Sync Successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sync failed: {e.StackTrace}");

            }


        }
    }
}
