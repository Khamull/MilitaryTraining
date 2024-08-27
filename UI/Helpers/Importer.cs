using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public class Importer
    {
        public static async Task<List<T>> LoadDataFromJsonAsync<T>(string filePath)
        {
            var jsonString = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}
