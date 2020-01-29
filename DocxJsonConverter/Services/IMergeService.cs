using DocxJsonConverter.Models;

namespace DocxJsonConverter.Services
{
    public interface IMergeService
    {
        string MergeJson2Docx(JsonConversionData jsonFileData, DocxConversionData docxFileData);
    }
}
