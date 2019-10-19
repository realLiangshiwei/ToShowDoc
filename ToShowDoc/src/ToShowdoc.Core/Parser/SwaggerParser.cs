using Newtonsoft.Json;
using ToShowDoc.Core.Parser.Core2x;
using ToShowDoc.Core.Parser.Core3x;

namespace ToShowDoc.Core.Parser
{
    public class SwaggerParser
    {
        public static SwaggerDocumentAbstract ParseString(string str)
        {
            str = str.Replace("$ref", "ref");
            str = str.Replace("\"200\":", "\"Success\":");
            str = str.Replace("\"401\":", "\"Unauthorized\":");
            str = str.Replace("\"403\":", "\"Forbidden\":");


            if (str.Contains("\"openapi\": \"3.0"))
            {
                return JsonConvert.DeserializeObject<SwaggerDocumentCoreV3>(str);
            }

            return JsonConvert.DeserializeObject<SwaggerDocumentCoreV2>(str);

        }
    }
}
