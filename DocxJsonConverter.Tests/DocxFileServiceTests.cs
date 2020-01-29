using DocxJsonConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocxJsonConverter.Tests
{
    [TestClass]
    public class DocxFileServiceTests : TestBase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCleanedCopy_NullPathTest()
        {
            var sut = new DocxFileService();
            var result = sut.CreateCleanedCopy(null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCleanedCopy_EmptyPathTest()
        {
            var sut = new DocxFileService();
            var result = sut.CreateCleanedCopy(string.Empty, false);
        }

        [TestMethod]
        public void CreateCleanedCopy_InvalidPathTest()
        {
            var sut = new DocxFileService();
            var result = sut.CreateCleanedCopy("blah blah", false);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateCleanedCopy_InvalidFileExtensionTest()
        {
            var sut = new DocxFileService();
            var result = sut.CreateCleanedCopy("yaya.jpg", false);

            Assert.IsNull(result);
        }


        [TestMethod]
        public void CreateCleanedCopyTest()
        {
            var sut = new DocxFileService();
            var result = sut.CreateCleanedCopy(FullDocxTestFilePath, false);

            Assert.IsNotNull(result);

            //Clean Up file that was created
            File.Delete(result.FullPath);
        }
    }
}
