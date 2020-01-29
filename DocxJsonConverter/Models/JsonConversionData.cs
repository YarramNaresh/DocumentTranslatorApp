using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocxJsonConverter.Models
{
    public class JsonConversionData
    {
        public readonly string FilePath;
        public readonly string FileName;
        public readonly string FileExtension = ".json";
        public JObject JsonData;
        public int TotalStrings2Translate;
        public readonly List<string> Messages;

        public string FullPath => Path.Join(FilePath, FileName + FileExtension);

        public JsonConversionData(string docxFilePath)
        {
            Messages = new List<string>();
            if (string.IsNullOrEmpty(docxFilePath))
            {
                throw new ArgumentNullException("the file path must be valid.");
            }

            FilePath = Path.GetDirectoryName(docxFilePath);

            string fileExtension = Path.GetExtension(docxFilePath);
            FileName = Path.GetFileName(docxFilePath).Replace(fileExtension, "");
        }
    }
}
