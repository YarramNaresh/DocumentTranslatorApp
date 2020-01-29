using System.IO;

namespace DocxJsonConverter.Tests
{
    public abstract class TestBase
    {
        protected static string TestDocxFileName => "TestDocxFile.docx";
        protected static string TestJsonFileName => "TestDocxFile.json";
        protected static string CurrentDirectory => Path.GetFullPath(@"..\..\..\");
        protected static string FullDocxTestFilePath => CurrentDirectory + @"TestFiles\" + TestDocxFileName;
        protected static string FullJsonTestFilePath => CurrentDirectory + @"TestFiles\" + TestJsonFileName;
    }
}
