using System.Collections.Generic;

namespace ToShowDoc.Core.Parser.Core2x
{
    public class SwaggerDocumentCoreV2 : SwaggerDocumentAbstract
    {
        public Dictionary<string, SwaggerDefinition> Definitions { get; set; }

        public Dictionary<string, SwaggerPath> Paths { get; set; }
    }

    public class SwaggerDefinition
    {
        public string Type { get; set; }

        public string[] Required { get; set; }

        public Dictionary<string, SwaggerDefinitionProperty> Properties { get; set; }
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
         
            public string Summary { get; set; }

            public List<ApiParameter> Parameters { get; set; }

            public Dictionary<string, ApiResponse> Responses { get; set; }


            public class ApiResponse
            {
                public string Description { get; set; }

                public ResponseSchema Schema { get; set; }


                public class ResponseSchema
                {
                    public string Ref { get; set; }

                    public string Type { get; set; }

                    public SwaggerDefinitionProperty.PropertyItems Items { get; set; }
                }


            }

        }
    }

}
