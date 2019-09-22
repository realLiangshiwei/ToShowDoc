using System;
using System.Collections.Generic;
using System.Text;
using ToShowDoc.Core.ApiClient;

namespace ToShowDoc.Core.Parser
{
    public static class SwaggerDocumentExtensions
    {
        public static List<ShowDocRequest> ToShowDocRequest(this SwaggerDocument document)
        {
            var res = new List<ShowDocRequest>();

            foreach (var path in document.Paths)
            {
                var showDocRequest = path.Value.Get?.PathToRequest(path.Key);
                showDocRequest = path.Value.Get?.PathToRequest(path.Key);
                showDocRequest = path.Value.Delete?.PathToRequest(path.Key);
                showDocRequest = path.Value.Post?.PathToRequest(path.Key);
                showDocRequest = path.Value.Put?.PathToRequest(path.Key);

                res.Add(showDocRequest);
            }

            return res;
        }


        public static ShowDocRequest PathToRequest(this SwaggerPath.SwaggerPathApi path, string apiUrl)
        {
            return new ShowDocRequest
            {
                //TODO 
            };
        }


    }
}
