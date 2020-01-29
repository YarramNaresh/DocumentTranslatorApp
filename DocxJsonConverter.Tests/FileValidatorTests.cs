using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class FileValidatorTests
    {
        private static string testDocxFileName = "TestDocxFile.docx";
        private static string testJsonFileName = "TestDocxFile.json";
        private static string currentDirectory = Path.GetFullPath(@"..\..\..\");
        private static string fullDocxTestFilePath = currentDirectory + @"TestFiles\" + testDocxFileName;
        private static string fullJsonTestFilePath = currentDirectory + @"TestFiles\" + testJsonFileName;

        [TestMethod]
        public void IsValidTest()
        {
            var sut = new FileValidator("docx");

            Assert.IsTrue(sut.IsValid(fullDocxTestFilePath));
            Assert.IsFalse(sut.IsValid(fullJsonTestFilePath));
        }
    }
}
