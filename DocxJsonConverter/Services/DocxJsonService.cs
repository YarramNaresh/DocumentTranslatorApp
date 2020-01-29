using DocxJsonConverter.Models;
using DocxJsonConverter.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.Services
{
    public class DocxJsonService : IDocxJsonService
    {
        private DocxFileService docxIoService;

        public DocxJsonService()
        {
            docxIoService = new DocxFileService();
        }

        public JsonConversionData BuildJsonConversionData(string filePath)
        {
            JsonConversionData jsonFileData = new JsonConversionData(filePath);

            //verify that we are working with a json file
            if (!filePath.ToLowerInvariant().EndsWith(".json"))
            {
                jsonFileData.Messages.Add("File is not of a .json type.");
                return jsonFileData;
            }

            //verify that the file exists
            if (!File.Exists(filePath))
            {
                jsonFileData.Messages.Add("Json file does not exist.");
                return jsonFileData;
            }

            //Create a new Document that can be cleaned of extra tags and used for inserting translated content.
            try
            {
                jsonFileData.JsonData = JObject.Parse(File.ReadAllText(filePath));
                jsonFileData.TotalStrings2Translate = jsonFileData.JsonData["lines"].Count();
            }
            catch (Exception e)
            {
                jsonFileData.Messages[0] = e.Message;
                return jsonFileData;
            }

            //Valid Schema for Json Data to being imported
            //NOTE: I used https://jsonschema.net/ to help me build the schema
            JsonSchema schema = JsonSchema.Parse(ValueRepository.JsonSchema);

            if (jsonFileData.JsonData.IsValid(schema))
            {
                jsonFileData.Messages.Add("Success");
            }
            else
            {
                jsonFileData.Messages.Add("Imported Json file contains invalid schema.");
            }

            return jsonFileData;
        }

        public JsonConversionData ExportStringsToJsonFile(DocxConversionData docxFileData)
        {
            if (docxFileData == null)
            {
                throw new ArgumentNullException("The DocxConversionData cannot be null.");
            }

            JsonConversionData jsonFileData = new JsonConversionData(docxFileData.FullPath);

            //Validate there is something to convert/translate in the Docx file
            if (docxFileData.Strings2Translate?.Count() == 0)
            {
                jsonFileData.Messages.Add("Docx File is Empty and has nothing to translate");
                return jsonFileData;
            }

            jsonFileData.JsonData = JObject.Parse(@"{ 
                'id': 'spf.io_convertion_data',
                'timestamp': " + DateTime.Now.ToString("yyyyMMddHHmmssffff") + @",
                'extractedFrom': '" + docxFileData.FileName + docxFileData.FileExtension + @"',
                'lines': []
            }");

            JArray JsonLineArray = (JArray)jsonFileData.JsonData["lines"];
            foreach (string TranslationString in docxFileData.Strings2Translate)
            {
                JsonLineArray.Add(TranslationString);
            }

            jsonFileData.TotalStrings2Translate = jsonFileData.JsonData["lines"].Count();

            //Validate there is something to convert/translate in the Docx file
            if (docxFileData.TotalStrings2Translate == jsonFileData.TotalStrings2Translate)
            {
                jsonFileData.Messages.Add("Success");
            }
            else
            {
                jsonFileData.Messages.Add("The creation of the Json Data from the docx file failed.");
            }

            return jsonFileData;
        }

    }
}
