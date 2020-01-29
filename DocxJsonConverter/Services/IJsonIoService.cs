using DocxJsonConverter.Models;

namespace DocxJsonConverter.Services
{
    public interface IJsonIoService
    {
        string SaveJson(JsonConversionData jsonData);
    }
}
