using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Parser.Core2x;
using ToShowDoc.Core.Parser.Core3x;

namespace ToShowDoc.Core.Parser
{
    public static partial class SwaggerDocumentExtensions
    {
        private static readonly string MarkdownTemplate;

        static SwaggerDocumentExtensions()
        {
            MarkdownTemplate = File.ReadAllText(AppContext.BaseDirectory + "Parser\\" + "MdTemplate.txt", Encoding.UTF8);
        }

        public static List<ShowDocRequest> ToShowDocRequest(this SwaggerDocumentAbstract document)
        {
            var res = new List<ShowDocRequest>();

            switch (document)
            {
                case SwaggerDocumentCoreV3 v3:
                {
                    foreach (var path in v3.Paths)
                    {
                        AddToShowDocRequest(PathToRequest(path.Value.Get, v3, path.Key, "GET"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Post, v3, path.Key, "POST"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Delete, v3, path.Key, "DELETE"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Put, v3, path.Key, "PUT"), res);
                    }

                    break;
                }
                case SwaggerDocumentCoreV2 v2:
                {
                    foreach (var path in v2.Paths)
                    {
                        AddToShowDocRequest(PathToRequest(path.Value.Get, v2, path.Key, "GET"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Post, v2, path.Key, "POST"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Delete, v2, path.Key, "DELETE"), res);
                        AddToShowDocRequest(PathToRequest(path.Value.Put, v2, path.Key, "PUT"), res);
                    }

                    break;
                }
            }
            

            return res;
        }

        private static void AddToShowDocRequest(ShowDocRequest request, List<ShowDocRequest> res)
        {
            if (request != null)
                res.Add(request);
        }


        private static void StringProperty(KeyValuePair<string, SwaggerDefinitionProperty> property,
        StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
        {
            var min = property.Value.Minimum.HasValue ? "最大长度" + property.Value.MinLength : string.Empty;
            var max = property.Value.Maximum.HasValue ? "最小长度" + property.Value.MaxLength : string.Empty;

            ReplaceParams(strBuild, prefix, property.Key, property.Value.Type, $"{description} {min} {max}", request, required);
        }

        private static void IntProperty(KeyValuePair<string, SwaggerDefinitionProperty> property, StringBuilder strBuild, string description, string prefix, bool request, string[] required = null)
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
