using System;
using System.Collections.Generic;
using System.Text;

namespace ToShowDoc.Core.Parser
{
    public class SwaggerDocumentAbstract
    {
    }

    public class SwaggerDefinitionProperty
    {
        public string Ref { get; set; }

        public string Type { get; set; }

        public int? MaxLength { get; set; }

        public int? MinLength { get; set; }

        public string Description { get; set; }

        public string Pattern { get; set; }

        public double? Minimum { get; set; }

        public double? Maximum { get; set; }

        public string Format { get; set; }

        public PropertyItems Items { get; set; }

        public int[] @Enum { get; set; }

        public class PropertyItems
        {
            public string Type { get; set; }

            public string Ref { get; set; }
        }
    }

    public class ApiParameter
    {
        public string @In { get; set; }

        public string Name { get; set; }

        public bool Required { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
        public ParameterSchema Schema { get; set; }

        public class ParameterSchema
        {
            public string Type { get; set; }

            public string Ref { get; set; }

        }
    }
}
