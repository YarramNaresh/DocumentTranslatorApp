using DocxJsonConverter.Models;

namespace DocxJsonConverter.Services
{
    public interface IDocxJsonService
    {
        JsonConversionData ExportStringsToJsonFile(DocxConversionData DocxFileData);
        JsonConversionData BuildJsonConversionData(string FilePath);
    }
}
