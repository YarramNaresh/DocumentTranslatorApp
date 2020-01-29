using CommandLine;

namespace docx2json
{
    public class Options
    {
        [Option('i')]
        public string Docx2ConvertPath { get; set; }
    }

}
