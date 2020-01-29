using DocumentFormat.OpenXml.Packaging;
using DocxJsonConverter.Models;
using System;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.Services
{
    public class DocxFileService : IDocxFileService
    {
        private readonly FileValidator validator;
        private readonly IDocumentTextRunExtractionService textExtractor;

        public DocxFileService()
        {
            validator = new FileValidator(".docx");
            textExtractor = new DocumentTextRunExtractionService();
        }

        public DocxConversionData CreateCleanedCopy(string fullPath, bool ignoreHidden)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentNullException("path variable cannot be null or empty");
            }

            // ensure we have a valid path to a docx file
            if (!validator.IsValid(fullPath))
            {
                return null;
            }

            var origFileData = new DocxConversionData(fullPath);

            var cleanedFileName = origFileData.FileName + "_cleaned" + origFileData.FileExtension;
            var cleanedFileData = new DocxConversionData(Path.Join(origFileData.FilePath, cleanedFileName));

            try
            {
                // ensure there is not an orphaned copy already on disk
                File.Delete(cleanedFileData.FullPath);

                // Make a copy of the original using the "cleaned" file name
                File.Copy(origFileData.FullPath, cleanedFileData.FullPath);

                //Clean Document of extra tags
                using (WordprocessingDocument doc = WordprocessingDocument.Open(cleanedFileData.FullPath, true))
                {

                    OpenXmlPowerTools.SimplifyMarkupSettings settings = new OpenXmlPowerTools.SimplifyMarkupSettings
                    {
                        AcceptRevisions = true,
                        NormalizeXml = false,         //setting this to false reduces translation quality, but if true some documents have XML format errors when opening
                        RemoveBookmarks = true,
                        RemoveComments = true,
                        RemoveContentControls = true,
                        RemoveEndAndFootNotes = true,
                        RemoveFieldCodes = false, //FieldCode remove pagination
                        RemoveGoBackBookmark = true,
                        //RemoveHyperlinks = false,
                        RemoveLastRenderedPageBreak = true,
                        RemoveMarkupForDocumentComparison = true,
                        RemovePermissions = false,
                        RemoveProof = true,
                        RemoveRsidInfo = true,
                        RemoveSmartTags = true,
                        RemoveSoftHyphens = true,
                        RemoveWebHidden = true,
                        ReplaceTabsWithSpaces = false
                    };

                    OpenXmlPowerTools.MarkupSimplifier.SimplifyMarkup(doc, settings);

                    //Extract Strings for translation
                    var textBlocks = textExtractor.ExtractText(doc, ignoreHidden);
                     //var textBlocks = textExtractor.ExtractParagraph(doc, ignoreHidden);
                    cleanedFileData.Strings2Translate = textBlocks.Select(para => para.InnerText);
                }

                //Total the number of strings for translation
                cleanedFileData.TotalStrings2Translate = cleanedFileData.Strings2Translate.Count();

                if (File.Exists(cleanedFileData.FullPath))
                {
                    cleanedFileData.Messages.Add("Success");
                }
                else
                {
                    cleanedFileData.Messages.Add("Docx data failed to build properly.");
                }
            }
            catch (Exception e)
            {
                cleanedFileData.Messages.Add(e.Message);
            }

            return cleanedFileData;
        }
    }
}
