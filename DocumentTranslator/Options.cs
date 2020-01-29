using CommandLine;

namespace DocumentTranslator
{
    public class Options
    {
        [Option('i')]
        public string InputJsonPath { get; set; }

        [Option('t')]
        public string Docx2MergePath { get; set; }
    }
}
