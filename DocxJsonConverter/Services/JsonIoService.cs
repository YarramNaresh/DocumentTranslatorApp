using DocxJsonConverter.Models;
using System;
using System.IO;

namespace DocxJsonConverter.Services
{
    public class JsonIoService : IJsonIoService
    {
        public string SaveJson(JsonConversionData jsonFileData)
        {
            string ResponseMessage = "failed";

            try
            {
                File.Delete(jsonFileData.FullPath);
                File.WriteAllText(jsonFileData.FullPath, jsonFileData.JsonData.ToString());
                if (File.Exists(jsonFileData.FullPath))
                {
                    ResponseMessage = "Success";
                }
                else
                {
                    ResponseMessage = "Json file failed to save.";
                }

            }
            catch (Exception e)
            {
                ResponseMessage = e.Message;
            }

            return ResponseMessage;
        }
    }
}
