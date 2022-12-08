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

        public TestContext TestContext { get; set; }

        #region Test Initializa e Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.TestName == "FileNameDoesExists")
            {
                SetGoodFileName();
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    
                    TestContext.WriteLine($"Creating File: {_GoodFileName}");
                    File.AppendAllText(_GoodFileName, "Some Text");
                }
            }
        }


        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName == "FileNameDoesExists")
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine($"Delete File: {_GoodFileName}");
                    File.Delete(_GoodFileName);
                }
            }
        }



        #endregion

        [TestMethod]
        [Owner("RodrigoB")]
        [TestCategory("No Exception")]
        [Priority(3)]
        public void FileNameDoesExists()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

           
            TestContext.WriteLine($"Testing File: {_GoodFileName} ");
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
        [Description("Verificar se o arquivo existe ")]
        [TestCategory("No Exception")]
        [Priority(3)]
        [Owner("Rodrigo")]
        public void FileNameDoesNotExists()
        {


            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);

        }

        [TestMethod]
        [Priority(2)]
        [Owner("Rodrigo")]
        [TestCategory("No Exception")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsNewArgumentNullException()
        {
            FileProcess fp = new FileProcess();
            fp.FileExists("");

            Assert.Inconclusive();

        }


        [TestMethod]
        [Priority(1)]
        [Owner("RodrigoB")]
        [TestCategory("Exception")]
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

        [TestMethod]
        [Timeout(3100)]
        [Priority(0)]
        [TestCategory("TimeOut")]
        [Owner("RodrigoT")]
        public void SimulateTimeout()
        {
            System.Threading.Thread.Sleep(3000);
        }



        private const string FILE_NAME = @"FileToDeploy.txt";

        [TestMethod]
        [Timeout(3100)]
        [Priority(0)]
        [TestCategory("TimeOut")]
        [Owner("RodrigoFile")]
        [DeploymentItem(FILE_NAME)]
        public void FileNameDoesExistsUsingDeploymentItem()
        {
            FileProcess fp = new FileProcess();
            string fileName;
            bool fromCall;

            fileName = $@"{TestContext.DeploymentDirectory}\{FILE_NAME}";
            TestContext.WriteLine($"Checking File: {fileName} ");
            fromCall = fp.FileExists(fileName);

            Assert.IsTrue(fromCall);
        }


    }
}
