using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;
using System;
using System.Configuration;
using System.IO;

namespace MyClassesTest
{

    [TestClass]
    public class FileProcessTest
    {

        private const string BAD_FILE_NAME = @"C:\Regedit.exe";
        private string _GoodFileName;

        [TestMethod]
        public void FileNameDoesExists()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            SetGoodFileName();
            File.AppendAllText(_GoodFileName, "Some Text");
            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        public void SetGoodFileName()
        {

            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", 
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData));
            }


        } 


        [TestMethod]
        public void FileNameDoesNotExists()
        {


            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsNewArgumentNullException()
        {
            FileProcess fp = new FileProcess();
            fp.FileExists("");

            Assert.Inconclusive();

        }


        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsNewArgumentNullException_UsingTryCatch()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentException)
            {
                // the test sucess
                return;
            }
            fp.FileExists("");

            Assert.Fail("Fail expected");

        }



    }
}
