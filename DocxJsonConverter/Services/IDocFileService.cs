using DocxJsonConverter.Models;

namespace DocxJsonConverter.Services
{
    public interface IDocxFileService
    {
        DocxConversionData CreateCleanedCopy(string fullPath, bool ignoreHidden);
    }
}
