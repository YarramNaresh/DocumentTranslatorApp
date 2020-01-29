using CommandLine;
using DocxJsonConverter.BusinessLogic;
using DocxJsonConverter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.IO;
using DocumentFormat.OpenXml.InkML;

namespace DocumentTranslator
{
    public class TextWithStyle
    {
        public string Text { get; set; }
        public string Style { get; set; }
    }
    public class Program
    {

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts => MergeJson2Docx(opts))
            .WithNotParsed<Options>((errs) => HandleParseError(errs));

            #region coomented
            //List<TextWithStyle> TextList = new List<TextWithStyle>();
            //using (var document = WordprocessingDocument.Open(@"F:\TextTranslator\Final Packet for distribution1.docx", false))
            //{
            //    // Gets the MainDocumentPart of the WordprocessingDocument 
            //    var main = document.MainDocumentPart;
            //    // document fonts
            //    var fonts = main.FontTablePart;
            //    // document styles
            //    var styles = main.StyleDefinitionsPart;
            //    var effects = main.StylesWithEffectsPart;
            //    // root element part of the doc
            //    var doc = main.Document;
            //    // actual document body
            //    var body = doc.Body;

            //    // styles on paragraps
            //    foreach (Paragraph para in body.Descendants<Paragraph>()
            //      .Where(e => e.ParagraphProperties != null && e.ParagraphProperties.ParagraphStyleId != null))
            //    {
            //        TextWithStyle textstyle = new TextWithStyle();
            //        textstyle.Text = para.InnerText;
            //        textstyle.Style = para.ParagraphProperties.ParagraphStyleId.Val;
            //        TextList.Add(textstyle);
            //    }
            //    // styles on Runs
            //    foreach (Run run in body.Descendants<Run>()
            //      .Where(r => r.RunProperties != null && r.RunProperties.RunStyle != null))
            //        Console.WriteLine("Text: {0}->Run style: {1}", run.InnerText, run.RunProperties.RunStyle.Val);
            //}

            //// Create Stream
            //using (MemoryStream mem = new MemoryStream())
            //{
            //    // Create Document
            //    using (WordprocessingDocument wordDocument =
            //        WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            //    {
            //        // Add a main document part. 
            //        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

            //        // Create the document structure and add some text.
            //        mainPart.Document = new Document();
            //        Body docBody = new Body();
            //        foreach (TextWithStyle txt in TextList)
            //        {
            //            Paragraph p = new Paragraph();
            //            ParagraphProperties ppr = new ParagraphProperties();
            //            ParagraphStyleId ppi = new ParagraphStyleId();
            //            ppi.Val.Value = txt.Style;
            //            ppr.ParagraphStyleId
            //            p.ParagraphProperties.ParagraphStyleId.Val.Value = txt.Style;
            //            Run r = new Run();
            //            Text t = new Text(txt.Text);
            //            r.Append(t);
            //            p.Append(r);


            //            docBody.Append(p);
            //        }
            //        wordDocument.SaveAs(@"F:\TextTranslator\Final Packet for distribution112.docx");
            //        // Add your docx content here
            //    }


            // Download File

            //}
            #endregion

        }
        static void MergeJson2Docx(Options opts)
        {
            // opts.InputJsonPath = @"F:\TextTranslator\theotech-docx_translator-54e33cd4e397\theotech-docx_translator-54e33cd4e397\DocxJsonConverter.Tests\TestFiles\TestDocxFile.json";
            opts.Docx2MergePath = @"F:\TextTranslator\Final Packet for distribution1.docx";


            if (opts.Docx2MergePath != null)
            {
                Docx2Json docx2JsonConversion = new Docx2Json(new JsonIoService(), new DocxJsonService(), new DocxFileService());
                var result = docx2JsonConversion.ExtractStringsToJsonFile(opts.Docx2MergePath);

                if (result.Messages.First() == "Success")
                {
                    Console.WriteLine(result.Messages.First());
                    Console.WriteLine("Exported File Name:");
                    Console.WriteLine(result.FullPath);

                    opts.InputJsonPath = result.FullPath;

                    if (opts.InputJsonPath != null && opts.Docx2MergePath != null)
                    {
                        var docTextRunExtractionService = new DocumentTextRunExtractionService();
                        var docxTextMerger = new DocxTextMerger(docTextRunExtractionService);
                        Json2Docx Json2DocxMerge = new Json2Docx(opts.InputJsonPath, opts.Docx2MergePath, new DocxFileService(), new DocxJsonService(), new MergeService(docxTextMerger));

                        if (Json2DocxMerge.Response == "Success")
                        {
                            Console.WriteLine(Json2DocxMerge.Response);
                            Console.WriteLine("Exported File Name:");
                            Console.WriteLine(Json2DocxMerge.FilePath);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: " + Json2DocxMerge.Response);
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Please enter a valid -i and -t file path");
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: " + result.Messages.First());
                }
            }
            else
            {
                Console.WriteLine("ERROR: Please enter a valid -i .docx file path");
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("ERROR: Please enter a valid command.");

            foreach (Error e in errs)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
