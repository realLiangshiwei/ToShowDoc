using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Parser.Core2x;

// ReSharper disable once CheckNamespace
namespace ToShowDoc.Core.Parser
{
    public static partial class SwaggerDocumentExtensions
    {

        private static ShowDocRequest PathToRequest(SwaggerPath.SwaggerPathApi path, SwaggerDocumentCoreV2 document, string apiUrl, string httpMethod)
        {
            if (path == null)
                return null;

            return new ShowDocRequest
            {
                CatName = path.Tags[0].Replace(":", "").Replace("\n", "").Trim().Replace(" ", ""),
                PageTitle = path.Summary ?? apiUrl,
                PageContent = BuildMarkdown(path, document, apiUrl, httpMethod)
            };
        }

        private static string BuildMarkdown(SwaggerPath.SwaggerPathApi path, SwaggerDocumentCoreV2 document, string apiUrl, string httpMethod)
        {
            var stringBuild = new StringBuilder(MarkdownTemplate);
            stringBuild.Replace("${ApiTitle}", path.Summary ?? apiUrl);
            stringBuild.Replace("${ApiUrl}", apiUrl);
            stringBuild.Replace("${HttpMethod} ", httpMethod);
            if (apiUrl.Contains("/api/TokenAuth/GetExternalAuthenticationProviders"))
            {

            }
            BuildParams(path, stringBuild, document);
            BuildResponseParams(path, stringBuild, document);

            stringBuild.Replace("${HttpMethod} ", httpMethod);

            var result = stringBuild.ToString();


            if (result.Contains("|参数名|必选|类型|说明|" + Environment.NewLine + "|:----|:---|:-----|-----|" + Environment.NewLine + "${params}"))
            {
                stringBuild.Replace("|参数名|必选|类型|说明|" + Environment.NewLine + "|:----|:---|:-----|-----|" + Environment.NewLine + "${params}", "无");
            }
            else
            {
                stringBuild.Replace("${params}", "");
            }
            if (result.Contains("|参数名|类型|说明|" + Environment.NewLine + "|:-----|:-----|-----|" + Environment.NewLine + "${responseParams}"))
            {
                stringBuild.Replace("|参数名|类型|说明|" + Environment.NewLine + "|:-----|:-----|-----|" + Environment.NewLine + "${responseParams}", "无");
            }
            else
            {
                stringBuild.Replace("${responseParams}", "");
            }

            return stringBuild.ToString();

        }

        private static void BuildParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocumentCoreV2 document)
        {
            if (path.Consumes != null && path.Consumes.Length > 0)
            {
                stringBuild.Replace("${requestType}", $"请求参数内容类型\n`{string.Join(", ", path.Consumes)}`");
            }
            else
            {
                stringBuild.Replace("${requestType}", "");
            }

            foreach (var parameter in path.Parameters)
            {
                var @in = parameter.In;
                if (@in == "body")
                {
                    var inputKey = parameter.Schema.Ref.Substring(parameter.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);

                    ComplexProperty(document, stringBuild, inputKey, string.Empty, true);

                }
                else
                {
                    var required = parameter.Required ? "是" : "否";
                    var des = string.IsNullOrWhiteSpace(parameter.Description) ? "无" : parameter.Description;
                    stringBuild.Replace("${params}", $"|{parameter.Name}|{required}|{parameter.Type}|{des}|{Environment.NewLine}${{params}}");
                }
            }

        }

        private static void BuildResponseParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocumentCoreV2 document)
        {
            if (path.Produces != null && path.Produces.Length > 0)
            {
                stringBuild.Replace("${responseType}", $"响应内容类型\n`{string.Join(", ", path.Produces)}`");
            }
            else
            {
                stringBuild.Replace("${responseType}", "");
            }

            if (path.Responses == null || path.Responses.Count <= 0) return;
            var success = path.Responses.ContainsKey("Success") ? path.Responses["Success"] : null;

            if (success?.Schema == null) return;
            if (string.IsNullOrWhiteSpace(success.Schema.Ref))
            {
                if (success.Schema.Items?.Ref != null)
                {
                    ComplexProperty(document, stringBuild, success.Schema.Items.Ref.Substring(success.Schema.Items.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1), success.Schema.Type + " -", false);
                    return;
                }
                ReplaceParams(stringBuild, string.Empty, success.Schema.Type, success.Schema.Type, success.Schema.Type, false);
            }
            else
            {
                var key = success.Schema.Ref.Substring(success.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(document, stringBuild, key, string.Empty, false);
            }

        }

        private static void BuildProperty(KeyValuePair<string, SwaggerDefinitionProperty> property, SwaggerDocumentCoreV2 document, StringBuilder strBuild, string prefix, bool request, string[] required = null)
        {
            if (!string.IsNullOrWhiteSpace(property.Value.Ref))
            {
                var modelKey = property.Value.Ref.Substring(property.Value.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(document, strBuild, modelKey, property.Key + " -", request);
                return;
            }

            var description = string.IsNullOrWhiteSpace(property.Value.Description) ? "无" : property.Value.Description;

            switch (property.Value.Type.ToLower())
            {
                case "integer":
                    IntProperty(property, strBuild, description, prefix, request, required);
                    break;
                case "string":
                    StringProperty(property, strBuild, description, prefix, request, required);
                    break;
                case "array":
                    ArrayProperty(property, document, strBuild, description, prefix, request, required);
                    break;
                default:
                    ReplaceParams(strBuild, prefix, property.Key, property.Value.Type, description, request, required);
                    break;

            }
        }

        private static void ArrayProperty(KeyValuePair<string, SwaggerDefinitionProperty> property, SwaggerDocumentCoreV2 document, StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
        {

            if (!string.IsNullOrWhiteSpace(property.Value.Items.Type))
            {
                ReplaceParams(strBuild, prefix, property.Key, $"{property.Value.Type} - {property.Value.Items.Type}", description, request, required);
            }
            else
            {
                var modelKey = property.Value.Items.Ref.Substring(property.Value.Items.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(document, strBuild, modelKey, property.Key + " -", request);

            }
        }

        private static void ComplexProperty(
            SwaggerDocumentCoreV2 document, StringBuilder strBuild, string key, string prefix, bool request)
        {
            var model = document.Definitions.ContainsKey(key) ? document.Definitions.Where(x => x.Key == key)
                .Select(x => x.Value).First() : null;
            if (model == null) return;
            foreach (var p in model.Properties)
            {
                BuildProperty(p, document, strBuild, prefix, request, model.Required);
            }

        }

    }
}
