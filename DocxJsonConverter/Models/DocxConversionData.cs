using System;
using System.Collections.Generic;
using System.IO;

namespace DocxJsonConverter.Models
{
    public class DocxConversionData
    {
        public readonly string FilePath;
        public readonly string FileName;
        public readonly string FileExtension;
        public IEnumerable<string> Strings2Translate;
        public int TotalStrings2Translate;
        public readonly List<string> Messages;

        public string FullPath => Path.Join(FilePath, FileName + FileExtension);

        public string GetTranslatedFullPath(DateTime time)
        {
            return Path.Join(FilePath, FileName + "_translated_" + time.ToString("yyyyMMdd_HHmmss") + FileExtension);
        }

        public DocxConversionData(string filePath)
        {
            FilePath = Path.GetDirectoryName(filePath);
            FileExtension = Path.GetExtension(filePath);
            FileName = Path.GetFileName(filePath).Replace(FileExtension, "");
            Messages = new List<string>();
        }
    }
}
