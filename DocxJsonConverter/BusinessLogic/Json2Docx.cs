using DocxJsonConverter.Models;
using DocxJsonConverter.Services;
using System;
using System.IO;

namespace DocxJsonConverter.BusinessLogic
{
    public class Json2Docx
    {
        public string Response { get; set; }
        public string FilePath { get; set; }

        public Json2Docx(string fullJsonTestFilePath, 
                         string fullDocxTestFilePath, 
                         IDocxFileService docxFileService,
                         IDocxJsonService jsonService,
                         IMergeService mergeService)
        {
            
            JsonConversionData jsonFileData = jsonService.BuildJsonConversionData(fullJsonTestFilePath);
            //Handle Errors
            if (jsonFileData.Messages[0] != "Success")
            {
                Response = jsonFileData.Messages[0];
                return;
            }

            DocxConversionData docxFileData = docxFileService.CreateCleanedCopy(fullDocxTestFilePath, false);

            //Handle Errors
            if (docxFileData.Messages[0] != "Success")
            {
                Response = docxFileData.Messages[0];
                return;
            }

            Response = mergeService.MergeJson2Docx(jsonFileData, docxFileData);

            if(Response == "Success")
            {
                //rename DocxFile
                string docxConversionDataFullPath = docxFileData.FullPath;
                string mergedFileName = docxFileData.GetTranslatedFullPath(DateTime.Now);

                try
                {
                    File.Move(docxConversionDataFullPath, mergedFileName);

                    if(File.Exists(mergedFileName))
                    {
                        FilePath = mergedFileName;
                    }
                    else
                    {
                        Response = "Error saving merged Docx file: " + mergedFileName; 
                    }
                }
                catch(Exception e)
                {
                    Response = e.Message;
                }           
            }
        }
    }
}
