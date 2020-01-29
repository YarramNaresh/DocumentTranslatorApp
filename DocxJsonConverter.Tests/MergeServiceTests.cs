using DocxJsonConverter.Models;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class MergeServiceTests
    {
        private static readonly string testDocxFileName = "TestDocxFile.docx";
        private static readonly string testJsonFileName = "TestDocxFile.json";
        private static readonly string currentDirectory = Path.GetFullPath(@"..\..\..\");
        private static readonly string fullDocxTestFilePath = currentDirectory + @"TestFiles\" + testDocxFileName;
        private static readonly string fullJsonTestFilePath = currentDirectory + @"TestFiles\" + testJsonFileName;

        [TestMethod]
        public void MergeJson2Docx_EnsureStringCountsMatchTest()
        {
            var mockTextMerger = new Mock<IDocxTextMerger>();
            var sut = new MergeService(mockTextMerger.Object);

            var jsonFileData = new JsonConversionData(fullJsonTestFilePath)
            {
                TotalStrings2Translate = 5
            };

            var docxFileData = new DocxConversionData(fullDocxTestFilePath)
            {
                TotalStrings2Translate = 3
            };

            var result = sut.MergeJson2Docx(jsonFileData, docxFileData);

            Assert.AreEqual(result.IndexOf("MERGE FAILED"), 0);
        }

        [TestMethod]
        public void MergeJson2Docx_TextBuilderExceptionHandledTest()
        {
            var exceptionMessage = "This is the exception message.";
            var mockTextMerger = new Mock<IDocxTextMerger>();
            mockTextMerger.Setup(tm => tm.Merge(It.IsAny<JsonConversionData>(), It.IsAny<DocxConversionData>(), It.IsAny<bool>()))
                           .Throws(new ArgumentNullException("docxFileData", exceptionMessage));

            var sut = new MergeService(mockTextMerger.Object);

            var jsonFileData = new JsonConversionData(fullJsonTestFilePath);
            var docxFileData = new DocxConversionData(fullDocxTestFilePath);

            var result = sut.MergeJson2Docx(jsonFileData, docxFileData);

            Assert.AreEqual(0, result.IndexOf(exceptionMessage));
        }

        [TestMethod]
        public void MergeJson2Docx_TranslatedStringsDontMatchJsonStringCountTest()
        {
            var resultStrings = new List<string>();
            resultStrings.Add("Result string 1");
            resultStrings.Add("Result string 2");
            resultStrings.Add("Result string 3");
            resultStrings.Add("Result string 4");
            resultStrings.Add("Result string 5");

            var mockTextMerger = new Mock<IDocxTextMerger>();
            mockTextMerger.Setup(tb => tb.Merge(It.IsAny<JsonConversionData>(), It.IsAny<DocxConversionData>(), It.IsAny<bool>()))
                           .Returns(resultStrings);

            var sut = new MergeService(mockTextMerger.Object);

            var jsonFileData = new JsonConversionData(fullJsonTestFilePath)
            {
                TotalStrings2Translate = 10
            };

            var docxFileData = new DocxConversionData(fullDocxTestFilePath)
            {
                TotalStrings2Translate = 10
            };

            var result = sut.MergeJson2Docx(jsonFileData, docxFileData);

            Assert.AreEqual(result, "fail");
        }

        [TestMethod]
        public void MergeJson2Docx_TranslatedStringsMatchJsonStringCountTest()
        {
            var resultStrings = new List<string>();
            resultStrings.Add("Result string 1");
            resultStrings.Add("Result string 2");
            resultStrings.Add("Result string 3");
            resultStrings.Add("Result string 4");
            resultStrings.Add("Result string 5");

            var mockTextMerger = new Mock<IDocxTextMerger>();
            mockTextMerger.Setup(tb => tb.Merge(It.IsAny<JsonConversionData>(), It.IsAny<DocxConversionData>(), It.IsAny<bool>()))
                           .Returns(resultStrings);

            var sut = new MergeService(mockTextMerger.Object);

            var jsonFileData = new JsonConversionData(fullJsonTestFilePath)
            {
                TotalStrings2Translate = 5
            };

            var docxFileData = new DocxConversionData(fullDocxTestFilePath)
            {
                TotalStrings2Translate = 5
            };

            var result = sut.MergeJson2Docx(jsonFileData, docxFileData);

            Assert.AreEqual(result, "Success");
        }

    }
}
