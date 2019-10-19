using Newtonsoft.Json;

namespace ToShowDoc.Core.Parser
{
    public class SwaggerParser
    {
        public static SwaggerDocument ParseString(string str)
        {
            str = str.Replace("$ref", "ref");
            str = str.Replace("\"200\":", "\"Success\":");
            str = str.Replace("\"401\":", "\"Unauthorized\":");
            str = str.Replace("\"403\":", "\"Forbidden\":");
            return JsonConvert.DeserializeObject<SwaggerDocument>(str);
        }
    }
}
