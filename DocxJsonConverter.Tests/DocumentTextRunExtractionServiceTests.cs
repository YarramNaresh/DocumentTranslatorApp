using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class DocumentTextRunExtractionServiceTests
    {
        private List<Text> bodyText;
        private List<Text> headerText;
        private List<Text> footerText;
        private MemoryStream docStream;
        private WordprocessingDocument doc;

        [TestInitialize]
        public void Setup()
        {
            // Setup the mock WordProcessingDocument
            bodyText = CreateMockText("body", 10);
            headerText = CreateMockText("header", 10);
            footerText = CreateMockText("footer", 10);

            docStream = new MemoryStream();
            doc = WordprocessingDocument.Create(docStream, WordprocessingDocumentType.Document, true);
            MainDocumentPart mainPart = doc.AddMainDocumentPart();

            new Document(new Body()).Save(mainPart);

            var body = mainPart.Document.Body;

            // Add new text.
            bodyText.ForEach(text =>
            {
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(text);
            });

            // setup headers
            var headerPart = mainPart.Document.MainDocumentPart.AddNewPart<HeaderPart>();
            headerPart.Header = new Header();
            headerText.ForEach(headerText =>
            {
                headerPart.Header.AppendChild(headerText);
            });

            // setup footers
            var footerPart = mainPart.Document.MainDocumentPart.AddNewPart<FooterPart>();
            footerPart.Footer = new Footer();
            footerText.ForEach(footerText =>
            {
                footerPart.Footer.AppendChild(footerText);
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            doc.Dispose();
            docStream.Close();
            docStream.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtractTextWithNullWordProcessingDocumentTest()
        {
            var sut = new DocumentTextRunExtractionService();
            sut.ExtractText(null);
        }

        [TestMethod]
        public void ExtractTextDoNotIgnoreHiddenTextTests()
        {
            var sut = new DocumentTextRunExtractionService();

            var result = sut.ExtractText(doc, false);

            Assert.IsNotNull(result);
            // 1/2 of the test strings should have been ignored
            Assert.AreEqual((bodyText.Count() + headerText.Count() + footerText.Count()) / 2, result.Count());
        }

        // TODO: Need to add a vanish tag around some text
        //       to ensure we test for ignoring hidden text

        /*
        [TestMethod]
        public void ExtractTextIgnoreHiddenTextTests()
        {
            var sut = new DocumentTextRunExtractionService();

            var result = sut.ExtractText(mockDoc.Object, true);

            Assert.IsNotNull(result);

            // 1/2 of the test strings should have been ignored
            // and 1 of the ones not ignored should have been hidden
            Assert.AreEqual((bodyText.Count() / 2) - 1, result.Count());
        }
        */

        private List<Text> CreateMockText(string prefix, int numberOfStrings)
        {
            var result = new List<Text>();
            for (int i = 0; i < numberOfStrings; ++i)
            {
                if (i % 2 == 0)
                {
                    result.Add(new Text($"{prefix} Test String {i}"));
                }
                else
                {
                    // Make 1/2 of the test strings empty
                    result.Add(new Text(string.Empty));
                }
            }

            return result;
        }
    }
}
