using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;

namespace Kontaktsplitter
{
    public class Controller
    {
        private readonly ContactModel _contactModelModel;
        private readonly InputView _inputView;
        private readonly AddTitleView _addTitleView;
        private ValidateView _validateView;
        private string EN = "EN";
        private const string MALE = "Male";

        public Controller()
        {
            _contactModelModel = new ContactModel();
            _inputView = new InputView(this);
            _addTitleView = new AddTitleView(_contactModelModel);


            _inputView.DataContext = _contactModelModel;
            _addTitleView.DataContext = _contactModelModel;

            _inputView.Show();
        }

        public void TEST()
        {
            WriteToHistory(_contactModelModel);
        }


        public void ParsString()
        {
            _contactModelModel.Error = string.Empty;
            ClearProperties();
            if (_contactModelModel.Input.Any(char.IsDigit))
            {
                // TODO Fehler

                _contactModelModel.Error = "Eingabe enthält unerlaubte Zahlen!";
                return;
            }


            var inputList = _contactModelModel.Input.Split(' ').ToList();
            inputList.RemoveAll(p => p.Equals(""));

            init(out var titels, out var genderDict);

            var input = SetLastAndFirstname(inputList, _contactModelModel.Input.Trim());


            input = SetGenderSalutationAndCountry(genderDict, input);


            input = SetTitle(input, titels);


            NotMatchingStrings(input);

            SetGender();
            CloseWindowAndRemoveInput();
        }

        public string SetLastAndFirstname(List<string> inputList, string input)
        {
            //Set LastName and possibly Firstname and delete from input
            
            string lastname;
            if (inputList.Count >= 2 && !input.Contains(","))
            {
                lastname = input.Split(' ').Last();
                _contactModelModel.LastName = lastname;
                input = input.Replace(lastname, "").Trim();
            }
            else if (inputList.Count >= 2 && inputList[inputList.Count - 2].Contains(","))
            {
                lastname = inputList[inputList.Count - 2].Replace(",", "");
                string firstname = inputList[inputList.Count - 1];
                _contactModelModel.LastName = lastname;
                _contactModelModel.FirstName = firstname;
                input = input.Replace(lastname, "");
                input = input.Replace(firstname, "");
                input = input.Replace(",", "");
            }

            return input;
        }

        public void NotMatchingStrings(string input)
        {
// Nicht zu geordente Wörter klassifzieren
            var unmatchedStrings = input.Split(' ');
            unmatchedStrings = unmatchedStrings.Where(val => val != "").ToArray();
            if (unmatchedStrings.Length == 1)
            {
                string firstname = unmatchedStrings[0];
                _contactModelModel.FirstName = firstname;
                unmatchedStrings = unmatchedStrings.Where(val => val != firstname).ToArray();
            }

            //Nicht zuordnenbare Strings als Listviewitems erzeugen
            foreach (string s in unmatchedStrings)
            {
                _contactModelModel.ListViewItems.Add(s);
            }
        }

        public string SetTitle(string input, List<string> titels)
        {
//Titel setzen
            var test = input.Split(' ');
            foreach (string item in test.ToList())
            {
                if (titels.Contains(item))
                {
                    _contactModelModel.Title += item + " ";
                    input = input.Replace(item, "");
                }
            }

            return input.Trim();
        }

        public string SetGenderSalutationAndCountry(Dictionary<string, List<string>> genderDict, string input)
        {
//Anrede, Geschlecht und Land setzen
            foreach (var item in genderDict)
            {
                if (input.Contains(item.Key))
                {
                    genderDict.TryGetValue(item.Key, out var genderValue);
                    if (genderValue != null && genderValue.First().Equals(MALE))
                    {
                        _contactModelModel.Salutation = item.Key;
                        _contactModelModel.Gender = Gender.m;

                        if (genderValue.Last().Equals(EN))
                        {
                            _contactModelModel.Country = Country.EN;
                        }
                        else if (genderValue.Last().Equals("DE"))
                        {
                            _contactModelModel.Country = Country.DE;
                        }
                    }
                    else
                    {
                        _contactModelModel.Salutation = item.Key;
                        _contactModelModel.Gender = Gender.f;

                        if (genderValue.Last().Equals(EN))
                        {
                            _contactModelModel.Country = Country.EN;
                        }
                        else
                        {
                            _contactModelModel.Country = Country.DE;
                        }
                    }

                    input = input.Replace(item.Key, "");
                }
            }

            
            return input.Trim();
        }

        private void ClearProperties()
        {
            _contactModelModel.Country = Country.DE; // Default
            _contactModelModel.FirstName = string.Empty;
            _contactModelModel.LastName = string.Empty;
            _contactModelModel.Title = string.Empty;
            _contactModelModel.Salutation = string.Empty;
            _contactModelModel.Gender = Gender.x; // Default 
            _contactModelModel.LetterSalutation = string.Empty;
        }


        private void WriteToHistory(ContactModel contact)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"../../XML/Contacts.xml");
            XmlNode contactNode = doc.CreateNode(XmlNodeType.Element, "Contact", null);
            XmlNode newNode2 = doc.CreateNode(XmlNodeType.Element, "Input", null);
            newNode2.InnerText = contact.Input;
            XmlNode newNode3 = doc.CreateNode(XmlNodeType.Element, "Title", null);
            newNode3.InnerText = contact.Title;
            XmlNode newNode4 = doc.CreateNode(XmlNodeType.Element, "Salutation", null);
            newNode4.InnerText = contact.Salutation;
            XmlNode newNode5 = doc.CreateNode(XmlNodeType.Element, "LetterSalutation", null);
            newNode5.InnerText = contact.LetterSalutation;
            XmlNode newNode6 = doc.CreateNode(XmlNodeType.Element, "Gender", null);
            newNode6.InnerText = contact.Gender.ToString();
            XmlNode newNode7 = doc.CreateNode(XmlNodeType.Element, "FirstName", null);
            newNode7.InnerText = contact.FirstName;
            XmlNode newNode8 = doc.CreateNode(XmlNodeType.Element, "LastName", null);
            newNode8.InnerText = contact.LastName;
            XmlNode newNode9 = doc.CreateNode(XmlNodeType.Element, "Country", null);
            newNode9.InnerText = contact.Country.ToString();

            var xmlNodes = new List<XmlNode>
            {
                newNode2,
                newNode3,
                newNode4,
                newNode5,
                newNode6,
                newNode7,
                newNode8,
                newNode9,
            };

            foreach (var xmlNode in xmlNodes)
            {
                contactNode.AppendChild(xmlNode);
            }

            doc.DocumentElement?.AppendChild(contactNode);
            doc.Save(@"../../XML/Contacts.xml");
            _contactModelModel.Input = string.Empty;
            _validateView.Close();
        }

        private void CloseWindowAndRemoveInput()
        {
            _validateView = new ValidateView(this, _contactModelModel) { DataContext = _contactModelModel };
            _validateView.ShowDialog();
            _contactModelModel.ListViewItems.Clear();
        }

        private void SetGender()
        {
            switch (_contactModelModel.Gender)
            {
                case Gender.m:
                    {
                        _contactModelModel.LetterSalutation =
                            _contactModelModel.Country == Country.EN ? "Dear Mr "+_contactModelModel.Title : "Sehr geeherter Herr " + _contactModelModel.Title;
                        break;
                    }
                case Gender.f:
                    {
                        if (_contactModelModel.Country == Country.EN)
                        {
                            _contactModelModel.LetterSalutation = "Dear Mrs " + _contactModelModel.Title;
                        }
                        else
                        {
                            _contactModelModel.LetterSalutation = "Sehr geeherte Frau " + _contactModelModel.Title;
                        }

                        break;
                    }
                case Gender.x:
                    {
                        if (_contactModelModel.Country == Country.EN)
                        {
                            _contactModelModel.LetterSalutation = "Dear Sirs " + _contactModelModel.Title;
                        }
                        else
                        {
                            _contactModelModel.LetterSalutation = "Sehr geeherte Damen und Herren " + _contactModelModel.Title;
                        }

                        break;
                    }
            }
        }

        public void init(out List<string> titels, out Dictionary<string, List<string>> genderDict)
        {
            titels = new List<string>();
            XmlDocument titleDoc = new XmlDocument();
            titleDoc.Load(@"../../XML/Titles.xml");
            XmlElement root = titleDoc.DocumentElement;

            XmlDocument genderDoc = new XmlDocument();
            genderDoc.Load(@"../../XML/Gender.xml");
            XmlElement genderRoot = genderDoc.DocumentElement;
            genderDict = new Dictionary<string, List<string>>();
            if (genderRoot != null)
                foreach (XmlNode node in genderRoot)
                {
                    var list = new List<string>();
                    if (node.Attributes != null)
                    {
                        list.Add(node.Attributes["ID"].InnerText);
                        list.Add(node.Attributes["Country"].InnerText);
                        if (node.Attributes != null) genderDict.Add(node.InnerText, list);
                    }
                }

            foreach (XmlNode childNode in root.ChildNodes)
            {
                if (childNode.Attributes != null) titels.Add(childNode.InnerText);
            }
        }
        
        public void addTitle()
        {
            _addTitleView.ShowDialog();
        }
    }
}