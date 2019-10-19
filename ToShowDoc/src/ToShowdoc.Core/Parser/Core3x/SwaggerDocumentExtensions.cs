using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Parser.Core3x;

// ReSharper disable once CheckNamespace
namespace ToShowDoc.Core.Parser
{
    public static partial class SwaggerDocumentExtensions
    {

        private static ShowDocRequest PathToRequest(SwaggerPath.SwaggerPathApi path, SwaggerDocumentCoreV3 documentCoreV3, string apiUrl, string httpMethod)
        {
            if (path == null)
                return null;

            return new ShowDocRequest
            {
                CatName = path.Tags[0].Replace(":", "").Replace("\n", "").Trim().Replace(" ", ""),
                PageTitle = path.Summary ?? apiUrl,
                PageContent = BuildMarkdown(path, documentCoreV3, apiUrl, httpMethod)
            };
        }

        private static string BuildMarkdown(SwaggerPath.SwaggerPathApi path, SwaggerDocumentCoreV3 documentCoreV3, string apiUrl, string httpMethod)
        {
            var stringBuild = new StringBuilder(MarkdownTemplate);
            stringBuild.Replace("${ApiTitle}", path.Summary ?? apiUrl);
            stringBuild.Replace("${ApiUrl}", apiUrl);
            stringBuild.Replace("${HttpMethod} ", httpMethod);
            if (apiUrl.Contains("/api/TokenAuth/GetExternalAuthenticationProviders"))
            {

            }
            BuildParams(path, stringBuild, documentCoreV3);
            BuildResponseParams(path, stringBuild, documentCoreV3);

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

        private static void BuildParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocumentCoreV3 documentCoreV3)
        {
            stringBuild.Replace("${requestType}",
                path.RequestBody != null
                    ? $"请求参数内容类型\n`{string.Join(", ", path.RequestBody.Content.Select(x => x.Key))}`"
                    : "");
            if (path.Parameters != null)
            {
                foreach (var parameter in path.Parameters)
                {
                    var @in = parameter.In;
                    if (@in == "body")
                    {
                        var inputKey = parameter.Schema.Ref.Substring(parameter.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);

                        ComplexProperty(documentCoreV3, stringBuild, inputKey, string.Empty, true);

                    }
                    else
                    {
                        var required = parameter.Required ? "是" : "否";

                        var des = string.IsNullOrWhiteSpace(parameter.Description) ? "无" : parameter.Description;
                        stringBuild.Replace("${params}", $"|{parameter.Name}|{required}|{parameter.Schema.Type}|{des}|{Environment.NewLine}${{params}}");
                    }
                }
            }
            else if (path.RequestBody != null)
            {
                var inputKey = path.RequestBody.Content.First().Value.Schema.Ref.Substring(path.RequestBody.Content.First().Value.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);

                ComplexProperty(documentCoreV3, stringBuild, inputKey, string.Empty, true);
            }

        }


        private static void BuildResponseParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocumentCoreV3 documentCoreV3)
        {
            if (path.Responses == null || path.Responses.Count <= 0)
            {
                stringBuild.Replace("${responseType}", "");
                return;
            }
            var success = path.Responses.ContainsKey("Success") ? path.Responses["Success"] : null;

            if (success?.Content == null || success.Content.Count <= 0)
            {
                stringBuild.Replace("${responseType}", "");
                return;
            }
            stringBuild.Replace("${responseType}", $"响应内容类型\n`{string.Join(", ", success.Content.Select(x => x.Key))}`");

            var schema = success.Content.First().Value.Schema;
            if (string.IsNullOrWhiteSpace(schema.Ref))
            {
                if (schema.Items?.Ref != null)
                {
                    ComplexProperty(documentCoreV3, stringBuild, schema.Items.Ref.Substring(schema.Items.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1), schema.Type + " -", false);
                    return;
                }
                ReplaceParams(stringBuild, string.Empty, schema.Type, schema.Type, schema.Type, false);
            }
            else
            {
                var key = schema.Ref.Substring(schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(documentCoreV3, stringBuild, key, string.Empty, false);
            }

        }

        private static void BuildProperty(KeyValuePair<string, SwaggerDefinitionProperty> property, SwaggerDocumentCoreV3 documentCoreV3, StringBuilder strBuild, string prefix, bool request, string[] required = null)
        {
            if (!string.IsNullOrWhiteSpace(property.Value.Ref))
            {
                var modelKey = property.Value.Ref.Substring(property.Value.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(documentCoreV3, strBuild, modelKey, property.Key + " -", request);
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
                    ArrayProperty(property, documentCoreV3, strBuild, description, prefix, request, required);
                    break;
                default:
                    ReplaceParams(strBuild, prefix, property.Key, property.Value.Type, description, request, required);
                    break;

            }
        }

        private static void ArrayProperty(KeyValuePair<string, SwaggerDefinitionProperty> property, SwaggerDocumentCoreV3 documentCoreV3, StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
        {

            if (!string.IsNullOrWhiteSpace(property.Value.Items.Type))
            {
                ReplaceParams(strBuild, prefix, property.Key, $"{property.Value.Type} - {property.Value.Items.Type}", description, request, required);
            }
            else
            {
                var modelKey = property.Value.Items.Ref.Substring(property.Value.Items.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);
                ComplexProperty(documentCoreV3, strBuild, modelKey, property.Key + " -", request);

            }
        }

        private static void ComplexProperty(
            SwaggerDocumentCoreV3 documentCoreV3, StringBuilder strBuild, string key, string prefix, bool request)
        {
            var model = documentCoreV3.Components.Schemas.ContainsKey(key) ? documentCoreV3.Components.Schemas.Where(x => x.Key == key)
                .Select(x => x.Value).First() : null;
            if (model == null) return;
            foreach (var p in model.Properties)
            {
                BuildProperty(p, documentCoreV3, strBuild, prefix, request, model.Required);
            }

        }
    }


}
