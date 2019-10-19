using System;
using System.Collections.Generic;
using System.Text;

namespace ToShowDoc.Core.ApiClient
{
    public class ShowDocRequest
    {
        public ShowDocRequest()
        {
            SNumber = 99;
        }

        public string CatName { get; set; }

        public string PageTitle { get; set; }

        public string PageContent { get; set; }

        public int SNumber { get; set; }
    }
}
