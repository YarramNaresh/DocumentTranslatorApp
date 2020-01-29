using CommandLine;
using DocxJsonConverter.BusinessLogic;
using DocxJsonConverter.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace docx2json
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts => MergeJson2Docx(opts))
            .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        static void MergeJson2Docx(Options opts)
        {
           // opts.InputJsonPath = @"F:\TextTranslator\theotech-docx_translator-54e33cd4e397\theotech-docx_translator-54e33cd4e397\DocxJsonConverter.Tests\TestFiles\TestDocxFile.json";
            opts.Docx2ConvertPath = @"F:\TextTranslator\Final Packet for distribution1.docx";
            
            if (opts.Docx2ConvertPath != null)
            {
                Docx2Json docx2JsonConversion = new Docx2Json(new JsonIoService(), new DocxJsonService(), new DocxFileService());
                var result = docx2JsonConversion.ExtractStringsToJsonFile(opts.Docx2ConvertPath);
                
                if (result.Messages.First() == "Success")
                {
                    Console.WriteLine(result.Messages.First());
                    Console.WriteLine("Exported File Name:");
                    Console.WriteLine(result.FullPath);
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