using CommandLine;

namespace json2docx
{
    public class Options
    {
        [Option('i')]
        public string InputJsonPath { get; set; }

        [Option('t')]
        public string Docx2MergePath { get; set; }
    }
}
