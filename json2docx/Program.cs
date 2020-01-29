using CommandLine;
using DocxJsonConverter.BusinessLogic;
using DocxJsonConverter.Services;
using System;
using System.Collections.Generic;

namespace json2docx
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts => MergeJson2Docx(opts))
            .WithNotParsed<Options>((errs) => HandleParseError(errs));
            Console.ReadLine();
        }

        static void MergeJson2Docx(Options opts)
        {

            opts.InputJsonPath = @"F:\TextTranslator\Final Packet for distribution1.json";
            opts.Docx2MergePath = @"F:\TextTranslator\Final Packet for distribution1.docx";

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
