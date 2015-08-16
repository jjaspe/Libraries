using CharacterSystemLibrary.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace CharacterTests
{
    
    
    /// <summary>
    ///This is a test class for CharacterTest and is intended
    ///to contain all CharacterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CharacterTest
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
        ///A test for toXml
        ///</summary>
        [TestMethod()]
        public void xmlSkillsTest()
        {
            Character expected = new Character(); // TODO: Initialize to an appropriate value
            XmlDocument creator = new XmlDocument(); // TODO: Initialize to an appropriate value
            expected.addSkill(new Skill("Skill", 20));
            XmlNode node=expected.toXml(creator);

            Character actual = new Character();
            actual.fromXml(node);

            Assert.AreEqual(expected.Skills.Count, actual.Skills.Count);
        }

        [TestMethod()]
        public void xmlStatsTest()
        {
            Character expected = new Character(); // TODO: Initialize to an appropriate value
            XmlDocument creator = new XmlDocument(); // TODO: Initialize to an appropriate value
            expected.addStat(new Stat("Stat", 20));
            XmlNode node = expected.toXml(creator);

            Character actual = new Character();
            actual.fromXml(node);

            Assert.AreEqual(expected.Stats.Count, actual.Stats.Count);
        }

        [TestMethod()]
        public void xmlAttributesTest()
        {
            Character expected = new Character(); // TODO: Initialize to an appropriate value
            XmlDocument creator = new XmlDocument(); // TODO: Initialize to an appropriate value
            expected.addAttribute(new CharacterSystemLibrary.Classes.Attribute("Attribute"));
            XmlNode node = expected.toXml(creator);

            Character actual = new Character();
            actual.fromXml(node);

            Assert.AreEqual(expected.Attributes.Count, actual.Attributes.Count);
        }

        [TestMethod()]
        public void xmlCharacterTest()
        {
            Character expected = new Character(); // TODO: Initialize to an appropriate value
            XmlDocument creator = new XmlDocument(); // TODO: Initialize to an appropriate value
            expected.addSkill(new Skill("Skill", 20));
            expected.addAttribute(new CharacterSystemLibrary.Classes.Attribute("Attribute"));
            expected.addStat(new Stat("Stat", 20));
            XmlNode node = expected.toXml(creator);

            Character actual = new Character();
            actual.fromXml(node);

            Assert.AreEqual(expected.Skills.Count, actual.Skills.Count);
            Assert.AreEqual(expected.Attributes.Count, actual.Attributes.Count);
            Assert.AreEqual(expected.Stats.Count, actual.Stats.Count);
        }
    }
}
