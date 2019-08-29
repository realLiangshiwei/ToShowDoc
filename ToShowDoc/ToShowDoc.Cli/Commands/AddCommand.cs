using McMaster.Extensions.CommandLineUtils;

namespace ToShowDoc.Cli.Commands
{
    [Command(Name = "add", Description = "add project")]
    [HelpOption("-h|--help")]
    public class AddCommand
    {
        [Option("-n|--name", CommandOptionType.SingleValue, Description = "Project Name")]
        public string Name { get; set; }

        [Option("-sdu|--ShowDocUrl", CommandOptionType.SingleValue, Description = "ShowDocUrl Name")]
        public string ShowDocUrl { get; set; }

        [Option("-ak|--ApiKey", CommandOptionType.SingleValue, Description = "ShowDoc Project ApiKey")]
        public string ApiKey { get; set; }

        [Option("-at|--ApiToken", CommandOptionType.SingleValue, Description = "ShowDoc Project ApiToken")]
        public string ApiToken { get; set; }

        [Option("-su|--SwaggerJsonUrl", CommandOptionType.SingleValue, Description = "Swagger Json Url")]
        public string SwaggerUrl { get; set; }
    }
}
