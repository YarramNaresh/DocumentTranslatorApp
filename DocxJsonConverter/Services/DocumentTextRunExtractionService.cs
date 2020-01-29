using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocxJsonConverter.Services
{
    public class DocumentTextRunExtractionService : IDocumentTextRunExtractionService
    {
        //  public List<Paragraph> ExtractText(WordprocessingDocument doc, bool ignoreHidden = false)
        public List<Text> ExtractText(WordprocessingDocument doc, bool ignoreHidden = false)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("The word processing document cannot be null.");
            }

            // var strings2Translate = new List<Paragraph>();
            var strings2Translate = new List<Text>();

            // Get all of the strings in the body that are not empty strings
            var body = doc.MainDocumentPart.Document.Body;
            IEnumerable<Paragraph> paragraphs = body.Descendants<Paragraph>();

            var paras = body.Elements<Paragraph>();
            List<string> ReplaceText = new List<string>();


            //strings2Translate.AddRange(body.Descendants<Paragraph>()
            //                             .Where(para => !String.IsNullOrEmpty(para.InnerText)));

            strings2Translate.AddRange(body.Descendants<Text>()
                                           .Where(text => !String.IsNullOrEmpty(text.Text)));

            // get all of the strings in the header that are not empty strings
            var headers = doc.MainDocumentPart.HeaderParts.Select(p => p.Header);
            foreach (var header in headers)
            {
                // strings2Translate.AddRange(header.Descendants<Paragraph>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
                strings2Translate.AddRange(header.Descendants<Text>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
            }

            // get all of the strings in the footer that are not empty strings
            var footers = doc.MainDocumentPart.FooterParts.Select(p => p.Footer);
            foreach (var footer in footers)
            {
                //strings2Translate.AddRange(footer.Descendants<Paragraph>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
                strings2Translate.AddRange(footer.Descendants<Text>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
            }

            // Remove any hidden strings if we're supposed to
            if (ignoreHidden)
            {
                strings2Translate.RemoveAll(t => t.Parent.Descendants<Vanish>().Any());
            }

            return strings2Translate;
        }

        public List<Paragraph> ExtractParagraph(WordprocessingDocument doc, bool ignoreHidden = false)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("The word processing document cannot be null.");
            }

            var strings2Translate = new List<Paragraph>();
            

            // Get all of the strings in the body that are not empty strings
            var body = doc.MainDocumentPart.Document.Body;
            IEnumerable<Paragraph> paragraphs = body.Descendants<Paragraph>();

            var paras = body.Elements<Paragraph>();
            List<string> ReplaceText = new List<string>();


            strings2Translate.AddRange(body.Descendants<Paragraph>().Where(para => para!=null));



            // get all of the strings in the header that are not empty strings
            var headers = doc.MainDocumentPart.HeaderParts.Select(p => p.Header);
            foreach (var header in headers)
            {
                 strings2Translate.AddRange(header.Descendants<Paragraph>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
            }

            // get all of the strings in the footer that are not empty strings
            var footers = doc.MainDocumentPart.FooterParts.Select(p => p.Footer);
            foreach (var footer in footers)
            {
                strings2Translate.AddRange(footer.Descendants<Paragraph>().Where(para => !String.IsNullOrEmpty(para.InnerText) && para.InnerText.Length > 0));
            }

            // Remove any hidden strings if we're supposed to
            if (ignoreHidden)
            {
                strings2Translate.RemoveAll(t => t.Parent.Descendants<Vanish>().Any());
            }

            return strings2Translate;
        }
    }
}
