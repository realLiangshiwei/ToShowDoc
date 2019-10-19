using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToShowDoc.Core.Entity;

namespace ToShowDoc.Core.ShowDoc
{
    public class JsonShowDocDataProvider : IShowDocDataProvider
    {
        private const string FilePath = "config\\showDocConfig.json";

        public async Task<List<ShowDocEntity>> LoadData()
        {
            var filePath = AppContext.BaseDirectory + FilePath;

            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<List<ShowDocEntity>>(await ReadTextAsync(filePath));
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await WriteTextAsync(filePath, "[]");

            return new List<ShowDocEntity>();

        }

        public async Task SaveChanges(List<ShowDocEntity> data)
        {
            await WriteTextAsync(AppContext.BaseDirectory + FilePath, JsonConvert.SerializeObject(data));
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                return await file.ReadToEndAsync();
            }
        }

        private Task WriteTextAsync(string filePath, string text)
        {
            File.WriteAllText(filePath, text);

            return Task.CompletedTask;
        }
    }
}
