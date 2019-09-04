using System;
using System.Collections.Generic;
using System.Text;

namespace ToShowDoc.Core.Entity
{
    public class ShowDocEntity
    {
        public int Id { get; set; }

        public string AppKey { get; set; }

        public string AppToken { get; set; }

        public string ShowDocUrl { get; set; }

        public string Name { get; set; }

        public string SwaggerUrl { get; set; }
    }
}
