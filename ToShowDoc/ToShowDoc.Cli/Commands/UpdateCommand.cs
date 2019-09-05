using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ToShowDoc.Core.ShowDoc;

namespace ToShowDoc.Commands
{
    [Command(Name = "upd", FullName = "update", Description = "update project")]
    [HelpOption("-h|--help")]
    public class UpdateCommand
    {
        private readonly IShowDocStore _showDocStore;

        public UpdateCommand(IShowDocStore showDocStore)
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
            var showDoc = (await _showDocStore.GetAll()).FirstOrDefault(x => x.Name == Name);

            if (showDoc == null)
            {
                Console.WriteLine($"Not Found Project {Name}");
                return;
            }

            showDoc.Name = Name;
            showDoc.AppKey = ApiKey;
            showDoc.AppToken = ApiToken;
            showDoc.ShowDocUrl = ShowDocUrl;
            showDoc.SwaggerUrl = SwaggerUrl;

            await _showDocStore.UpdateShowDoc(showDoc);
            Console.WriteLine("Update Success");
        }
    }
}
