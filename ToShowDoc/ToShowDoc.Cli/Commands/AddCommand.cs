using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.Entity;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{
    [Command(Name = "add", Description = "add project")]
    [HelpOption("-h|--help")]
    public class AddCommand
    {
        private readonly IShowDocStore _showDocStore;

        public AddCommand(IShowDocStore showDocStore)
        {
            _showDocStore = showDocStore;
        }

        [Required]
        [Option("-n|--name", CommandOptionType.SingleValue, Description = "Project Name")]
        public string Name { get; set; }

        [Required]
        [Option("-ak|--ApiKey", CommandOptionType.SingleValue, Description = "ShowDoc Project ApiKey")]
        public string ApiKey { get; set; }

        [Required]
        [Option("-at|--ApiToken", CommandOptionType.SingleValue, Description = "ShowDoc Project ApiToken")]
        public string ApiToken { get; set; }

        [Required]
        [Option("-su|--SwaggerJsonUrl", CommandOptionType.SingleValue, Description = "Swagger Json Url")]
        public string SwaggerUrl { get; set; }

        [Required]
        [Option("-sdu|--ShowDocUrl", CommandOptionType.SingleValue, Description = "ShowDocUrl Name")]
        public string ShowDocUrl { get; set; }

        public async Task OnExecute(CommandLineApplication app)
        {
            await _showDocStore.AddShowDoc(new ShowDocEntity
            {
                AppKey = ApiKey,
                AppToken = ApiToken,
                Name = Name,
                ShowDocUrl = ShowDocUrl,
                SwaggerUrl = SwaggerUrl
            });
            Console.WriteLine("Add done...");
        }
    }
}
