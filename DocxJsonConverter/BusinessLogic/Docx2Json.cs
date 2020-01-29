using DocxJsonConverter.Models;
using DocxJsonConverter.Services;
using DocxJsonConverter.Translator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.BusinessLogic
{
    public class Docx2Json
    {
        private readonly IJsonIoService jsonIoService;
        private readonly IDocxJsonService docxJSonService;
        private readonly IDocxFileService docxFileService;

        public string FilePath { get; set; }

        public Docx2Json(IJsonIoService jsonIoService, IDocxJsonService docxJsonService, IDocxFileService docxFileService)
        {
            this.jsonIoService = jsonIoService ?? throw new ArgumentNullException("The Json IO service cannot be null.");
            this.docxJSonService = docxJsonService ?? throw new ArgumentNullException("The docx json service cannot be null.");
            this.docxFileService = docxFileService ?? throw new ArgumentNullException("The docx file service cannot be null.");
        }

        public JsonConversionData ExtractStringsToJsonFile(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentOutOfRangeException("The file path cannot be null or empty.");
            }

            //Extract Docx data out information from Docx file
            DocxConversionData cleanedDocxFileData = docxFileService.CreateCleanedCopy(fullFilePath, true);

            //Handle Errors
            if (cleanedDocxFileData.Messages[0] != "Success")
            {
                var errorResult = new JsonConversionData(fullFilePath);
                errorResult.Messages.Add(cleanedDocxFileData.Messages[0]);
                return errorResult;
            }

            // We don't need the "cleaned" copy of the docx any longer, so delete it
            if (File.Exists(cleanedDocxFileData.FullPath))
            {
                File.Delete(cleanedDocxFileData.FullPath);
            }

            // We want the json file we create to have the same name as the original file, so
            // restore the original path in our data object
            var origFileData = new DocxConversionData(fullFilePath)
            {
                TotalStrings2Translate = cleanedDocxFileData.TotalStrings2Translate,
                Strings2Translate = cleanedDocxFileData.Strings2Translate
            };
            origFileData.Messages.AddRange(cleanedDocxFileData.Messages);

            //translate data to target language
            List<string> TranslatedStrings = new List<string>();
            foreach (string line in origFileData.Strings2Translate)
            {
                string TranslatedLine = TextTranslator.TranslateText(line, "es", "en");
                TranslatedStrings.Add(TranslatedLine);
            }
            origFileData.Strings2Translate = (IEnumerable<string>)TranslatedStrings;

            //Convert docx data to json data
            JsonConversionData jsonFileData = docxJSonService.ExportStringsToJsonFile(origFileData);

            //Handle Errors
            //if (jsonFileData.Messages[0] != "Success")
            //{
            //    return jsonFileData;
            //}

            //Delete any Json file that might have the same name as the new one we are creating.
            if (File.Exists(jsonFileData.FullPath))
            {
                try
                {
                    File.Delete(jsonFileData.FullPath);
                }
                catch(Exception e)
                {
                    var errorResult = new JsonConversionData(fullFilePath);
                    errorResult.Messages.Add(e.Message);
                    return errorResult;
                }
            }

            //Export Json data to file
            var result = jsonIoService.SaveJson(jsonFileData);

            //Handle Errors
            if (result != "Success")
            {
                jsonFileData.Messages.Insert(0, "Failed");
            }

            return jsonFileData;
        }
    }
}
