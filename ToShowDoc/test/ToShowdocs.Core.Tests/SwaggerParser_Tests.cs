using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Entity;
using ToShowDoc.Core.Parser;
using Xunit;

namespace ToShowDocs.Core.Tests
{
    public class SwaggerParser_Tests
    {
        private readonly SwaggerDocument _document;
        private readonly ShowDocEntity _showdoc;
        public SwaggerParser_Tests()
        {
            _showdoc = new ShowDocEntity()
            {
                AppKey = "48be8cfc2f1aa03dfb97fa5170695ad6414239409",
                AppToken = "7151e44fddf593cc1457b8b91c564cac724696720",
                ShowDocUrl = "https://www.showdoc.cc/server/api/item/updateByApi"
            };
            var str = File.ReadAllText(AppContext.BaseDirectory + "swaggerDocs.json", Encoding.UTF8);
            _document = SwaggerParser.ParseString(str);
        }

        [Fact]
        public void SwaggerDocsDefinition_Test()
        {
            _document.Definitions.First().Key.ShouldBe("IsTenantAvailableInput");

            _document.Definitions.First().Value.Required.ShouldBe(new[] { "tenancyName" });
            _document.Definitions.First().Value.Type.ShouldBe("object");
            _document.Definitions.First().Value.Properties.First().Key.ShouldBe("tenancyName");
            _document.Definitions.First().Value.Properties.First().Value.MaxLength.ShouldBe(64);
            _document.Definitions.First().Value.Properties.First().Value.MinLength.ShouldBe(0);
            _document.Definitions.First().Value.Properties.First().Value.Type.ShouldBe("string");

            _document.Definitions["IsTenantAvailableOutput"].Properties["state"].Enum.Length.ShouldBe(3);
            _document.Definitions["IsTenantAvailableOutput"].Properties["state"].Type.ShouldBe("integer");
        }

        [Fact]
        public void SwaggerDocsPath_Test()
        {
            _document.Paths["/api/TokenAuth/Authenticate"].Get.ShouldBeNull();
            _document.Paths["/api/TokenAuth/Authenticate"].Delete.ShouldBeNull();
            _document.Paths["/api/TokenAuth/Authenticate"].Put.ShouldBeNull();

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Consumes.Length.ShouldBe(4);
            _document.Paths["/api/TokenAuth/Authenticate"].Post.OperationId.ShouldBe("Authenticate");

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.Count.ShouldBe(1);

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().In.ShouldBe("body");

            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().Name.ShouldBe("model");
            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().Required.ShouldBe(false);
            _document.Paths["/api/TokenAuth/Authenticate"].Post.Parameters.First().Schema.Ref.ShouldBe("#/definitions/AuthenticateModel");

            string des = _document.Paths["/api/TokenAuth/Authenticate"].Post.Responses["Success"].Description;
            string @ref = _document.Paths["/api/TokenAuth/Authenticate"].Post.Responses["Success"].Schema.Ref;
            des.ShouldBe("Success");
            @ref.ShouldBe("#/definitions/AuthenticateResultModel");
        }


        [Fact]
        public async Task SwaggerToShowdoc_Test()
        {
            var request = _document.ToShowDocRequest();
            request.Count.ShouldBe(28);

            foreach (var item in request)
            {
                var res = await ShowDocClient.UpdateByApi(_showdoc, item);
            }
        }
    }
}
