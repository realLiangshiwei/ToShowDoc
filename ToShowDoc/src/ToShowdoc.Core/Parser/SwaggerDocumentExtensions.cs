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
            MarkdownTemplate = File.ReadAllText(AppContext.BaseDirectory + "Parser\\" + "MdTemplate.txt", Encoding.UTF8);
        }

        public static List<ShowDocRequest> ToShowDocRequest(this SwaggerDocument document)
        {
            var res = new List<ShowDocRequest>();

            foreach (var path in document.Paths)
            {
                AddToShowDocRequest(PathToRequest(path.Value.Get, document, path.Key, "GET"), res);
                AddToShowDocRequest(PathToRequest(path.Value.Post, document, path.Key, "POST"), res);
                AddToShowDocRequest(PathToRequest(path.Value.Delete, document, path.Key, "DELETE"), res);
                AddToShowDocRequest(PathToRequest(path.Value.Put, document, path.Key, "PUT"), res);
            }

            return res;
        }

        private static void AddToShowDocRequest(ShowDocRequest request, List<ShowDocRequest> res)
        {
            if (request != null)
                res.Add(request);
        }


        private static ShowDocRequest PathToRequest(SwaggerPath.SwaggerPathApi path, SwaggerDocument document, string apiUrl, string httpMethod)
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

        private static string BuildMarkdown(SwaggerPath.SwaggerPathApi path, SwaggerDocument document, string apiUrl, string httpMethod)
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

        private static void BuildParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocument document)
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
                if (@in == "query")
                {
                    var required = parameter.Required ? "是" : "否";

                    stringBuild.Replace("${params}", $"|{parameter.Name}|{required}|{parameter.Type}|{parameter.Description ?? "无"}|{Environment.NewLine}${{params}}");
                }
                else if (@in == "body")
                {
                    var inputKey = parameter.Schema.Ref.Substring(parameter.Schema.Ref.LastIndexOf("/", StringComparison.Ordinal) + 1);

                    ComplexProperty(document, stringBuild, inputKey, string.Empty, true);

                }
            }

        }


        private static void BuildResponseParams(SwaggerPath.SwaggerPathApi path, StringBuilder stringBuild, SwaggerDocument document)
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

        private static void BuildProperty(KeyValuePair<string, SwaggerDefinition.SwaggerDefinitionProperty> property, SwaggerDocument document, StringBuilder strBuild, string prefix, bool request, string[] required = null)
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

        private static void ArrayProperty(KeyValuePair<string, SwaggerDefinition.SwaggerDefinitionProperty> property, SwaggerDocument document, StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
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
            SwaggerDocument document, StringBuilder strBuild, string key, string prefix, bool request)
        {
            var model = document.Definitions.ContainsKey(key) ? document.Definitions.Where(x => x.Key == key)
                .Select(x => x.Value).First() : null;
            if (model == null) return;
            foreach (var p in model.Properties)
            {
                BuildProperty(p, document, strBuild, prefix, request, model.Required);
            }

        }


        private static void StringProperty(KeyValuePair<string, SwaggerDefinition.SwaggerDefinitionProperty> property,
        StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
        {
            var min = property.Value.Minimum.HasValue ? "最大长度" + property.Value.MinLength : string.Empty;
            var max = property.Value.Maximum.HasValue ? "最小长度" + property.Value.MaxLength : string.Empty;

            ReplaceParams(strBuild, prefix, property.Key, property.Value.Type, $"{description} {min} {max}", request, required);
        }

        private static void IntProperty(KeyValuePair<string, SwaggerDefinition.SwaggerDefinitionProperty> property, StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
        {
            if (property.Value.Enum != null && property.Value.Enum.Length > 0)
            {
                ReplaceParams(strBuild, prefix, property.Key, "enum", $"{description} 取值范围{string.Join(",", property.Value.Enum)}", request, required);
                return;
            }

            var min = property.Value.Minimum.HasValue ? "最小值" + property.Value.Minimum : string.Empty;
            var max = property.Value.Maximum.HasValue ? "最大值" + property.Value.Maximum : string.Empty;

            ReplaceParams(strBuild, prefix, property.Key, property.Value.Type, $"{description} {min} {max}", request, required);
        }

        public static void ReplaceParams(StringBuilder strBuild, string prefix, string key, string type,
            string description, bool request, string[] required = null)
        {
            if (!request)
            {
                strBuild.Replace("${responseParams}",
                $"|{prefix} {key}|{type}|{description}|{Environment.NewLine}${{responseParams}}");
            }
            else
            {

                var req = required == null ? "否" : required.Contains(key) ? "是" : "否";
                strBuild.Replace("${params}",
                    $"|{prefix} {key}|{req}|{type}|{description}|{Environment.NewLine}${{params}}");
            }
        }
    }


}
