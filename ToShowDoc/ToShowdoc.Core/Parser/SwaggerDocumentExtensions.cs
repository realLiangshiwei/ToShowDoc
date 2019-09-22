using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToShowDoc.Core.ApiClient;

namespace ToShowDoc.Core.Parser
{
    public static class SwaggerDocumentExtensions
    {
        private static readonly string MarkdownTemplate;

        static SwaggerDocumentExtensions()
        {
            MarkdownTemplate = File.ReadAllText(AppContext.BaseDirectory + "MdTemplate.txt");
        }

        public static List<ShowDocRequest> ToShowDocRequest(this SwaggerDocument document)
        {
            var res = new List<ShowDocRequest>();

            foreach (var path in document.Paths)
            {
                var showDocRequest = PathToRequest(path.Value.Get, null, document, path.Key, "GET");
                showDocRequest = PathToRequest(path.Value.Post, showDocRequest, document, path.Key, "POST");
                showDocRequest = PathToRequest(path.Value.Delete, showDocRequest, document, path.Key, "DELETE");
                showDocRequest = PathToRequest(path.Value.Put, showDocRequest, document, path.Key, "PUT");

                res.Add(showDocRequest);
            }

            return res;
        }


        private static ShowDocRequest PathToRequest(SwaggerPath.SwaggerPathApi path, ShowDocRequest docRequest, SwaggerDocument document, string apiUrl, string httpMethod)
        {
            if (path == null)
                return docRequest;

            return new ShowDocRequest
            {
                CatName = path.Tags[0].Replace(":", "").Replace("\n", "").Trim(),
                PageTitle = path.Summary ?? apiUrl,
                PageContent = BuildMarkdown(path, document, apiUrl, httpMethod)
            };
        }

        private static string BuildMarkdown(SwaggerPath.SwaggerPathApi path, SwaggerDocument document, string apiUrl, string httpMethod)
        {
            var stringBuild = new StringBuilder(MarkdownTemplate);
            stringBuild.Replace("${ApiTitle}", path.Summary ?? apiUrl);
            stringBuild.Replace("${ApiUrl}", apiUrl);
            stringBuild.Replace("${HttpMethod} ", httpMethod);

            BuildParams(path, stringBuild, document);
            BuildResponseParams(path, stringBuild, document);

            stringBuild.Replace("${HttpMethod} ", httpMethod);

            stringBuild.Replace("${params}", "");
            stringBuild.Replace("${responseParams}", "");


            return stringBuild.ToString();

        }

        private static void BuildParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocument document)
        {
            foreach (var parameter in path.Parameters)
            {
                var @in = parameter.In;
                if (@in == "query")
                {
                    var required = parameter.Required ? "是" : "否";

                    stringBuild.Replace("${params}", $"|{parameter.Name}|{required}|{parameter.Type}|{parameter.Description}|\n${{params}}");
                }
                else if (@in == "body")
                {
                    var inputKey = parameter.Schema.Ref.Substring(parameter.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                    var input = document.Definitions.ContainsKey(inputKey) ? document.Definitions
                        .Where(x => x.Key == inputKey)
                        .Select(x => x.Value).First() : null;
                    if (input == null) continue;
                    foreach (var property in input.Properties)
                    {
                        var req = input.Required.Contains(property.Key) ? "是" : "否";
                        stringBuild.Replace("${params}",
                            $"|{property.Key}|{req}|{property.Value.Type}|{property.Value.Description}|\n${{params}}");
                    }
                }
            }

        }


        private static void BuildResponseParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocument document)
        {
            if (path.Responses == null || path.Responses.Count <= 0) return;
            var success = path.Responses.ContainsKey("Success") ? path.Responses["Success"] : null;

            if (success?.Schema == null) return;
            if (string.IsNullOrWhiteSpace(success.Schema.Ref))
            {
                stringBuild.Replace("${responseParams}",
                    $"|{success.Schema.Type}|{success.Schema.Type}|{success.Schema.Type}|\n${{responseParams}}");
            }
            else
            {
                var outputKey = success.Schema.Ref.Substring(success.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                var output = document.Definitions.ContainsKey(outputKey) ? document.Definitions
                    .Where(x => x.Key == outputKey)
                    .Select(x => x.Value).First() : null;
                if (output == null) return;
                foreach (var property in output.Properties)
                {
                    stringBuild.Replace("${responseParams}",
                        $"|{property.Key}|{property.Value.Type}|{property.Value.Description}|\n${{responseParams}}");
                }
            }

        }
    }
}
