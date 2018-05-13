using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kontaktsplitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontaktsplitter.Tests
{
    [TestClass()]
    public class ControllerTests
    {
        Controller controllertest = new Controller();
        List<string> testinputlist = new List<string>
        {
            "Herr",
            "Prof.",
            "Max",
            "Mustermann"
        };
        string input = "Herr Prof. Max Mustermann";
        List<string> titleList= new List<string>
        {
            "Dr.",
            "Prof.",
            "Doktor"
        };
        Dictionary<string, List<string>> genderTestDictionary = new Dictionary<string, List<string>>();

        [TestMethod()]
        public void SetLastAndFirstnameTest()
        {

            var erg = controllertest.SetLastAndFirstname(testinputlist, input);
            Assert.AreEqual("Herr Prof. Max", erg);
        }

        [TestMethod()]
        public void SetGenderSalutationAndCountryTest()
        {
            //controllertest.SetGenderSalutationAndCountry();
            Assert.Fail();
        }

        [TestMethod()]
        public void initTest()
        {
            List<string> testtitlesList = new List<string>();
            Dictionary<string, List<string>> genderTestDictionary = new Dictionary<string, List<string>>();
            var tempList = new List<string> {"Male", "DE"};
            genderTestDictionary.Add("Herr", tempList);
            Dictionary<string, List<string>> genderDictionary = new Dictionary<string, List<string>>();
            controllertest.init(out testtitlesList, out genderDictionary);

            Assert.AreEqual(genderTestDictionary.Keys.First(), genderDictionary.Keys.First());
            Assert.AreEqual(titleList.First(), testtitlesList.First());
            Assert.AreEqual(titleList.Last(), testtitlesList.Last());

        }
    }
}