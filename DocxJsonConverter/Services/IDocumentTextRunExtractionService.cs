using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace DocxJsonConverter.Services
{
    public interface IDocumentTextRunExtractionService
    {
        List<Text> ExtractText(WordprocessingDocument doc, bool ignoreHidden = false);
        List<Paragraph> ExtractParagraph(WordprocessingDocument doc, bool ignoreHidden = false);
    }
}
