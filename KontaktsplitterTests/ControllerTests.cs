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

        // Teststring geparst
        public List<string> Testinputlist = new List<string>
        {
            "Herr",
            "Prof.",
            "Max",
            "Mustermann"
        };

        // Teststring
        public string Input = "Herr Prof. Max Mustermann";

        // Test Title Liste, statt XML
        public List<string> TitleList = new List<string>
        {
            "Dr.",
            "Prof.",
            "Doktor"
        };

        // Test Gender Liste, statt XML
        public Dictionary<string, List<string>> GenderTestDictionary = new Dictionary<string, List<string>>();

        // Prüft Beispieltextes in der SetLastAndFirstname-Methode
        // Bei mehr als 2 Wörter in der Eingabe wird der Nachname erkannt und entfernt
        [TestMethod()]
        public void SetLastAndFirstnameTest()
        {
            var erg = Controllertest.SetLastAndFirstname(Testinputlist, Input);
            Assert.AreEqual("Herr Prof. Max", erg);
        }

        // Prüft ob nach Eingabe des Beispieltextes das Herr als Anrede erkannt 
        // und korrekt entfernt wird
        [TestMethod()]
        public void SetGenderSalutationAndCountryTest()
        {
            var expectedResult = "Prof. Max Mustermann";
            Dictionary<string, List<string>> genderTestDictionary = new Dictionary<string, List<string>>();
            var tempList = new List<string> { "Male", "DE" };
            genderTestDictionary.Add("Herr", tempList);
            var resultGenderSalutationAndCountry = Controllertest.SetGenderSalutationAndCountry(genderTestDictionary, Input);
            
            Assert.AreEqual(expectedResult, resultGenderSalutationAndCountry);
        }

        // Prüft, ob die testTitleList und das genderDictionary
        // durch die Init-Methode mit den zu erwartenden Werten
        // gefüllt wird
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

        // Prüft das korrekte entfernen des Titels aus dem Beispieltext
        [TestMethod()]
        public void SetTitleTest()
        {
            var titleErgebnis = Controllertest.SetTitle(Input, TitleList);

            Assert.AreEqual("Herr  Max Mustermann", titleErgebnis);
        }
    }
}