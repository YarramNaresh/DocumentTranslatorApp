using DocxJsonConverter.BusinessLogic;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class json2docxTests : TestBase
    {
        private IDocxFileService docxFileService = new DocxFileService();
        private IDocxJsonService docxJsonService = new DocxJsonService();
        private IMergeService mergeService = new MergeService(new DocxTextMerger(new DocumentTextRunExtractionService()));

        [TestMethod]
        public void TestJson2DocxConversion()
        {
            Json2Docx Json2DocxTest = new Json2Docx(FullJsonTestFilePath, FullDocxTestFilePath, docxFileService, docxJsonService, mergeService);

            Assert.AreEqual("Success", Json2DocxTest.Response);

            //Validate that the Docx file was saved properly
            string exportedFullFilePath = Json2DocxTest.FilePath;
            Assert.IsTrue(File.Exists(exportedFullFilePath));
            var rawFileContents = File.ReadAllLines(exportedFullFilePath);
            Assert.IsNotNull(rawFileContents);

            // Delete the file we created
            File.Delete(Json2DocxTest.FilePath);
        }
    }
}
