using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ToShowDoc.Core.Parser
{
    public class SwaggerParser
    {
        public static SwaggerDocument ParseString(string str)
        {
            return JsonConvert.DeserializeObject<SwaggerDocument>(str);
        }
    }
}
