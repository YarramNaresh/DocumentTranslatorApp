using DocxJsonConverter.BusinessLogic;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class docx2jsonTest : TestBase
    {
        private IDocxFileService docxFileService = new DocxFileService();
        private IDocxJsonService docxJsonService = new DocxJsonService();
        private IJsonIoService jsonIoService = new JsonIoService();

        [TestMethod]
        public void TestDocx2JsonConversion()
        {
            var sut = new Docx2Json(jsonIoService, docxJsonService, docxFileService);

            var filePath = Path.GetDirectoryName(FullDocxTestFilePath);
            var fileExtension = Path.GetExtension(FullDocxTestFilePath);
            var fileName = Path.GetFileName(filePath).Replace(fileExtension, "");

            var tempDocxPath = Path.Join(filePath, fileName + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension);
            File.Copy(FullDocxTestFilePath, tempDocxPath);

            var result = sut.ExtractStringsToJsonFile(tempDocxPath);

            Assert.AreEqual("Success", result.Messages.First());

            //Validate that the Json file was saved properly
            Assert.IsTrue(File.Exists(result.FullPath));
            var rawFileContents = File.ReadAllLines(result.FullPath);
            Assert.IsNotNull(rawFileContents);

            //Clean Up files that was created
            File.Delete(tempDocxPath);
            File.Delete(result.FullPath);
        }
    }
}
