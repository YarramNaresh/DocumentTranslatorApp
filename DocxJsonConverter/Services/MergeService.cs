using DocxJsonConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocxJsonConverter.Services
{
    public class MergeService : IMergeService
    {
        private readonly IDocxTextMerger textMerger;

        public MergeService(IDocxTextMerger textMerger)
        {
            this.textMerger = textMerger ?? throw new ArgumentNullException("The DocxTextMerger cannot be null.");
        }

        public string MergeJson2Docx(JsonConversionData jsonFileData, DocxConversionData docxFileData)
        {
            string ResponseMessage = "fail";

            if (jsonFileData.TotalStrings2Translate != docxFileData.TotalStrings2Translate)
            {
                return "MERGE FAILED: Docx file and Json file do not the same number of strings to merge.";
            }

            IEnumerable<string> DocxStrings = null;
            try
            {
                //Merge Json data into Docx File.
                DocxStrings = textMerger.Merge(jsonFileData, docxFileData);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            if (DocxStrings.Count() == jsonFileData.TotalStrings2Translate)
            {
                ResponseMessage = "Success";
            }

            return ResponseMessage;
        }
    }
}
