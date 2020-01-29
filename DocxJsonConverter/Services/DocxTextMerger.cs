using DocumentFormat.OpenXml.Packaging;
using DocxJsonConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocxJsonConverter.Services
{
    public class DocxTextMerger : IDocxTextMerger
    {
        private readonly IDocumentTextRunExtractionService textExtractionService;

        public DocxTextMerger(IDocumentTextRunExtractionService textExtractionService)
        {
            this.textExtractionService = textExtractionService ?? throw new ArgumentNullException("The text extraction service cannot be null.");
        }

        public IEnumerable<string> Merge(JsonConversionData jsonFileData, DocxConversionData docxFileData, Boolean ignoreHidden = false)
        {
            if (docxFileData == null)
            {
                throw new ArgumentNullException("docxFileData", "DocxConversionData cannot be null.");
            }

            if (jsonFileData == null)
            {
                throw new ArgumentNullException("jsonFileData", "JsonFileData cannot be null.");
            }

            string docxConversionDataFullPath = docxFileData.FullPath;

            using (WordprocessingDocument doc = WordprocessingDocument.Open(docxConversionDataFullPath, true))
            {
                var strings2Translate = textExtractionService.ExtractText(doc, ignoreHidden);

                for (int j = 0; j < docxFileData.TotalStrings2Translate; j++)
                {
                    int indexInDocument = j + 1;
                    var newValue = (string)jsonFileData.JsonData["lines"][j];

                    strings2Translate.Take(indexInDocument).Last().Text = newValue;
                }

                IEnumerable<string> outputStrings2Translate = strings2Translate.Select(text => text.Text);
                return outputStrings2Translate;
            }
        }
    }
}
