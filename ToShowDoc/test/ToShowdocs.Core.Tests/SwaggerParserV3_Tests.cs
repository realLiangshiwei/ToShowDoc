using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using ToShowDoc.Core.ApiClient;
using ToShowDoc.Core.Entity;
using ToShowDoc.Core.Parser;
using ToShowDoc.Core.Parser.Core3x;
using Xunit;

namespace ToShowDocs.Core.Tests
{
    public class SwaggerParserV3_Tests
    {
        private readonly SwaggerDocumentCoreV3 _documentCoreV3;
        private readonly ShowDocEntity _showdoc;
        public SwaggerParserV3_Tests()
        {
            _showdoc = new ShowDocEntity()
            {
                AppKey = "5b2451ec0a8b49f5323645e6d50d9b29557907599",
                AppToken = "170c09f45fe6e70642cc17c44ff9213f1222612682",
                ShowDocUrl = "https://www.showdoc.cc/server/api/item/updateByApi"
            };
            var str = File.ReadAllText(AppContext.BaseDirectory + "swaggerDocsV3.json", Encoding.UTF8);
            _documentCoreV3 = SwaggerParser.ParseString(str) as SwaggerDocumentCoreV3;
        }

        [Fact]
        public void SwaggerDocsComponents_Test()
        {
            _documentCoreV3.Components.Schemas.First().Key.ShouldBe("Author");

            _documentCoreV3.Components.Schemas.First().Value.Type.ShouldBe("object");
            _documentCoreV3.Components.Schemas.First().Value.Properties.First().Key.ShouldBe("name");
            _documentCoreV3.Components.Schemas.First().Value.Properties.First().Value.Description.ShouldBe("作者姓名");


            _documentCoreV3.Components.Schemas["Book"].Properties["name"].MaxLength.ShouldBe(10);
            _documentCoreV3.Components.Schemas["Book"].Required.Length.ShouldBe(1);
        }

        [Fact]
        public void SwaggerDocsPath_Test()
        {
            _documentCoreV3.Paths["/api/BookStore"].Get.ShouldNotBeNull();
            _documentCoreV3.Paths["/api/BookStore"].Post.ShouldNotBeNull();
            _documentCoreV3.Paths["/api/BookStore"].Put.ShouldBeNull();

            _documentCoreV3.Paths["/api/BookStore"].Post.RequestBody.Content.Count.ShouldBe(3);
            _documentCoreV3.Paths["/api/BookStore"].Post.Responses["Success"].ShouldNotBeNull();
            _documentCoreV3.Paths["/api/BookStore"].Post.Summary.ShouldBe("添加图书");


            _documentCoreV3.Paths["/api/BookStore/{name}"].Get.Summary.ShouldBe("获取指定名称图书");
            _documentCoreV3.Paths["/api/BookStore/{name}"].Get.Parameters.Count.ShouldBe(1);
            _documentCoreV3.Paths["/api/BookStore/{name}"].Get.Parameters[0].Schema.Type.ShouldBe("string");

            
        }


        [Fact]
        public async Task SwaggerToShowdoc_Test()
        {
            var request = _documentCoreV3.ToShowDocRequest();
            
            foreach (var item in request)
            {
                var res = await ShowDocClient.UpdateByApi(_showdoc, item);
            }
        }
    }
}
