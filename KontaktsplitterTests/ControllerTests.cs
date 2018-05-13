using System.Collections.Generic;
using System.Linq;
using Kontaktsplitter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KontaktsplitterTests
{
    [TestClass()]
    public class ControllerTests
    {
        public Controller Controllertest = new Controller();
        public List<string> Testinputlist = new List<string>
        {
            "Herr",
            "Prof.",
            "Max",
            "Mustermann"
        };
        public string Input = "Herr Prof. Max Mustermann";
        public List<string> TitleList = new List<string>
        {
            "Dr.",
            "Prof.",
            "Doktor"
        };
        public Dictionary<string, List<string>> GenderTestDictionary = new Dictionary<string, List<string>>();

        [TestMethod()]
        public void SetLastAndFirstnameTest()
        {

            var erg = Controllertest.SetLastAndFirstname(Testinputlist, Input);
            Assert.AreEqual("Herr Prof. Max", erg);
        }

        [TestMethod()]
        public void SetGenderSalutationAndCountryTest()
        {
            var expectedGenderResult = "Prof. Max Mustermann";
            Dictionary<string, List<string>> genderTestDictionary = new Dictionary<string, List<string>>();
            var tempList = new List<string> { "Male", "DE" };
            genderTestDictionary.Add("Herr", tempList);
            var resultGenderSalutationAndCountry = Controllertest.SetGenderSalutationAndCountry(genderTestDictionary, Input);
            
            Assert.AreEqual(expectedGenderResult, resultGenderSalutationAndCountry);
        }

        [TestMethod()]
        public void InitTest()
        {
            List<string> testtitlesList = new List<string>();
            Dictionary<string, List<string>> genderTestDictionary = new Dictionary<string, List<string>>();
            var tempList = new List<string> { "Male", "DE" };
            genderTestDictionary.Add("Herr", tempList);
            Dictionary<string, List<string>> genderDictionary = new Dictionary<string, List<string>>();
            Controllertest.init(out testtitlesList, out genderDictionary);

            Assert.AreEqual(genderTestDictionary.Keys.First(), genderDictionary.Keys.First());
            Assert.AreEqual(TitleList.First(), testtitlesList.First());
            Assert.AreEqual(TitleList.Last(), testtitlesList.Last());

        }

        [TestMethod()]
        public void SetTitleTest()
        {
            var titleErgebnis = Controllertest.SetTitle(Input, TitleList);

            Assert.AreEqual("Herr  Max Mustermann", titleErgebnis);
        }
       
    }
}