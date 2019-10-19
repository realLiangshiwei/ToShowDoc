using System.Collections.Generic;

namespace ToShowDoc.Core.Parser.Core3x
{
    public class SwaggerDocumentCoreV3 : SwaggerDocumentAbstract
    {
        public SwaggerComponents Components { get; set; }

        public Dictionary<string, SwaggerPath> Paths { get; set; }
    }

    public class SwaggerComponents
    {
        public Dictionary<string, SwaggerComponentsSchemas> Schemas { get; set; }


        public class SwaggerComponentsSchemas
        {
            public string Type { get; set; }

            public string[] Required { get; set; }

            public Dictionary<string, SwaggerDefinitionProperty> Properties { get; set; }

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
            public string[] Tags { get; set; }
            public string Summary { get; set; }

            public ApiRequestBody RequestBody { get; set; }

            public List<ApiParameter> Parameters { get; set; }

            public Dictionary<string, ApiResponse> Responses { get; set; }

            public class ApiRequestBody
            {
                public string Description { get; set; }

                public Dictionary<string, ApiResponse.ApiResponseContent> Content { get; set; }
            }

            public class ApiResponse
            {
                public string Description { get; set; }

                public Dictionary<string, ApiResponseContent> Content { get; set; }


                public class ApiResponseContent
                {
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

}
