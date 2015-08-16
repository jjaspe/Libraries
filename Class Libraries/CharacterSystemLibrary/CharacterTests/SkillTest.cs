using CharacterSystemLibrary.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace CharacterTests
{
    
    
    /// <summary>
    ///This is a test class for SkillTest and is intended
    ///to contain all SkillTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SkillTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for fromXml
        ///</summary>
        [TestMethod()]
        public void fromXmlTest()
        {
            XmlDocument doc=new XmlDocument();
            Skill expected = new Skill("Skill",20); 
            XmlNode node = expected.toXml(doc);

            Skill actual = new Skill();
            actual.fromXml(node);

            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
