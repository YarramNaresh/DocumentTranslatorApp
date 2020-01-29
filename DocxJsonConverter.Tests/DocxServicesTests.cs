using DocxJsonConverter.Models;
using DocxJsonConverter.Repositories;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Schema;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class DocxServicesTests : TestBase
    {
        private IDocxJsonService docxJsonService = new DocxJsonService();
        private IDocxFileService docxFileService = new DocxFileService();
        
        [TestMethod]
        public void TestBuildDocxConversionData()
        {
            //Make sure Test File Exists
            TestingTestDocxFile();

            //Test Service Method
            DocxConversionData docxFileData = docxFileService.CreateCleanedCopy(FullDocxTestFilePath, false);
            Assert.IsNotNull(docxFileData);
            Assert.AreEqual("Success", docxFileData.Messages[0]);
            Assert.IsTrue(docxFileData.TotalStrings2Translate > 0);

            Assert.IsTrue(File.Exists(docxFileData.FullPath));

            //Clean Up file that was created
            File.Delete(docxFileData.FullPath);
        }

        [TestMethod]
        public void TestConvertDocx2Json()
        {
            //make sure test files exist
            TestingTestDocxFile();

            //Valid Schema for Json Data to be exported exported
            //NOTE: I used https://jsonschema.net/ to help me build the schema
            JsonSchema schema = JsonSchema.Parse(ValueRepository.JsonSchema);

            //Test Service Methods
            DocxConversionData docxFileData = docxFileService.CreateCleanedCopy(FullDocxTestFilePath, false);
            JsonConversionData jsonFileData = docxJsonService.ExportStringsToJsonFile(docxFileData);

            Assert.AreEqual("Success", jsonFileData.Messages[0]);
            //Validate Json being exported has a valid schema
            Assert.IsTrue(jsonFileData.JsonData.IsValid(schema));
            //Validate that exported Json contains the same number of strings to be translated
            Assert.AreEqual(jsonFileData.TotalStrings2Translate, docxFileData.TotalStrings2Translate);

            //Clean Up file that was created
            File.Delete(docxFileData.FullPath);
        }

        [TestMethod]
        public void TestExportJson()
        {
            //make sure test files exist
            TestingJsonFileTest();
            TestingTestDocxFile();

            var jsonIoService = new JsonIoService();

            //Test Service Methods
            DocxConversionData docxFileData = docxFileService.CreateCleanedCopy(FullDocxTestFilePath, false);
            JsonConversionData jsonFileData = docxJsonService.ExportStringsToJsonFile(docxFileData);

            File.Delete(jsonFileData.FullPath);
            jsonIoService.SaveJson(jsonFileData);

            Assert.AreEqual("Success", jsonFileData.Messages[0]);
            //Validate Json being exported has a valid schema
            Assert.IsTrue(File.Exists(jsonFileData.FullPath));
            var rawFileContents = File.ReadAllLines(jsonFileData.FullPath);
            Assert.IsNotNull(rawFileContents);

            //Clean Up file that was created
            File.Delete(docxFileData.FullPath);
            File.Delete(jsonFileData.FullPath);
        }

        [TestMethod]
        public void TestBuildJsonConversionData()
        {
            //make sure test file exist
            TestingTestDocxFile();

            //Build Json Data
            JsonConversionData JsonFileData = docxJsonService.BuildJsonConversionData(FullJsonTestFilePath);
            Assert.AreEqual(JsonFileData.Messages[0], "Success");

            //Validate Json being exported has a valid schema
            Assert.IsTrue(JsonFileData.JsonData.IsValid(JsonSchema.Parse(ValueRepository.JsonSchema)));

            //Validate there is strings to translate
            Assert.IsTrue(JsonFileData.TotalStrings2Translate > 0);
            Assert.IsTrue(JsonFileData.JsonData["lines"].Count() > 0);
        }

        [TestMethod]
        public void TestMergeJson2Docx()
        {
            //make sure test files exist
            TestingJsonFileTest();
            TestingTestDocxFile();

            JsonConversionData jsonFileData = docxJsonService.BuildJsonConversionData(FullJsonTestFilePath);
            DocxConversionData docxFileData = docxFileService.CreateCleanedCopy(FullDocxTestFilePath, false);

            var mergeService = new MergeService(new DocxTextMerger(new DocumentTextRunExtractionService()));
            string ResponseMessage = mergeService.MergeJson2Docx(jsonFileData, docxFileData);

            Assert.AreEqual(ResponseMessage, "Success");
            Assert.IsTrue(File.Exists(docxFileData.FullPath));

            //Clean Up file that was created
            File.Delete(docxFileData.FullPath);
        }

        private void TestingTestDocxFile()
        {
            //Make sure the test file exist and isn't empty
            Assert.IsTrue(File.Exists(FullDocxTestFilePath));
            var rawFileContents = File.ReadAllLines(FullDocxTestFilePath);
            Assert.IsNotNull(rawFileContents);
        }

        private void TestingJsonFileTest()
        {
            //Make sure the test Json file exist and isn't empty
            Assert.IsTrue(File.Exists(FullJsonTestFilePath));
            var rawFileContents = File.ReadAllLines(FullJsonTestFilePath);
            Assert.IsNotNull(rawFileContents);
        }
    }
}
