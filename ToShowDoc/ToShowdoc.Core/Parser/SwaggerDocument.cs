using System.Collections.Generic;

namespace ToShowDoc.Core.Parser
{
    public class SwaggerDocument
    {
        public Dictionary<string, SwaggerDefinition> Definitions { get; set; }

        public Dictionary<string, SwaggerPath> Paths { get; set; }
    }

    public class SwaggerDefinition
    {
        public string Type { get; set; }

        public string[] Required { get; set; }

        public Dictionary<string, SwaggerDefinitionProperty> Properties { get; set; }

        public class SwaggerDefinitionProperty
        {
            public string Type { get; set; }

            public int MaxLength { get; set; }

            public int MinLength { get; set; }

            public string Description { get; set; }

            public string Pattern { get; set; }

            public int MinNum { get; set; }

            public int MaxNum { get; set; }

            public string Format { get; set; }

            public PropertyItems Items { get; set; }

            public int[] @Enum { get; set; }

            public class PropertyItems
            {
                public string Type { get; set; }
            }
        }
    }

    public class SwaggerPath
    {
        public SwaggerPathApi Post { get; set; }

        public SwaggerPathApi Get { get; set; }

        public SwaggerPathApi Delete { get; set; }

        public SwaggerPathApi Put { get; set; }

        public class SwaggerPathApi
        {
            public string[] Consumes { get; set; }
            public string[] Produces { get; set; }
            public string[] Tags { get; set; }
            public string OperationId { get; set; }

            public List<dynamic> Parameters { get; set; }

            public dynamic Responses { get; set; }

        }
    }

}
