using DocxJsonConverter.Models;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class JsonIoServiceTests
    {
        private static string testJsonFileName = "TestJsonExportFile.json";
        private static string currentDirectory = Path.GetFullPath(@"..\..\..\");
        private static string fullJsonTestFilePath = currentDirectory + @"TestFiles\" + testJsonFileName;

        [TestMethod]
        public void SaveJsonTest()
        {
            var sut = new JsonIoService();

            var jsonFileData = new JsonConversionData(fullJsonTestFilePath);
            sut.SaveJson(jsonFileData);
        }
    }
}
