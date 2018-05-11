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
        private string EN="EN";
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
            if (_contactModelModel.Input.Any(char.IsDigit))
            {
                // TODO Fehler
            }
            var inputList = _contactModelModel.Input.Split(' ').ToList();
            inputList.RemoveAll(p => p.Equals(""));
            init(out var titels, out var genderDict);

            CheckForlastAndFirstName(titels, inputList, genderDict);

           
            //Sting parsen ud auf titel und Anrede überprüfen
            foreach (string s in inputList)
            {
                if (titels.Contains(s))
                {
                    _contactModelModel.Title += s+" ";
                }
                else if (genderDict.ContainsKey(s))
                {
                    genderDict.TryGetValue(s, out var genderValue);
                    if (genderValue != null && genderValue.First().Equals(MALE))
                    {
                        _contactModelModel.Salutation = s;
                        
                        _contactModelModel.Gender = Gender.m;
                        if (genderValue.Last().Equals(EN))
                        {
                            _contactModelModel.Country = Country.EN;
                        }
                        else
                        {
                            _contactModelModel.Country = Country.DE;
                        }
                    }
                    else
                    {
                        _contactModelModel.Salutation = s;
                        
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
                }
                else
                {
                    _contactModelModel.ListViewItems.Add(s);
                }
            }

            SetGender();
            CloseWindowAndRemoveInput();
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

            doc.DocumentElement.AppendChild(contactNode);
            doc.Save(@"../../XML/Contacts.xml");
            _contactModelModel.Input = string.Empty;
            _validateView.Close();
        }

        private void CloseWindowAndRemoveInput()
        {
            
            _validateView = new ValidateView(this, _contactModelModel) {DataContext = _contactModelModel};
            _validateView.ShowDialog();
            _contactModelModel.ListViewItems.Clear();
        }

        private void CheckForlastAndFirstName(List<string> titels, List<string> inputList, Dictionary<string, List<string>> genderDict)
        {
            if (titels.Contains(inputList.Last()))
            {
                //Errorhandler
            }
            else if (inputList.Count == 2 && !titels.Contains(inputList.First()) && !genderDict.ContainsKey(inputList.First()))
            {
                _contactModelModel.FirstName = inputList.First();
                _contactModelModel.LastName = inputList.Last();
                inputList.Clear();
            }
            else if (inputList.Count >= 2 && inputList[inputList.Count - 2].Contains(","))
            {
                _contactModelModel.LastName = inputList[inputList.Count - 2].Replace(",", "");
                _contactModelModel.FirstName = inputList[inputList.Count - 1];
                inputList.RemoveAt(inputList.Count - 2);
                inputList.RemoveAt(inputList.Count - 1);
            }
            else if (inputList.Count == 1)
            {
                //Error
            }
            else
            {
                _contactModelModel.LastName = inputList.Last();
                inputList.RemoveAt(inputList.Count - 1);
            }
        }

        private void SetGender()
        {
            switch (_contactModelModel.Gender)
            {
                case Gender.m:
                {
                    _contactModelModel.LetterSalutation = _contactModelModel.Country == Country.EN ? "Dear Mr" : "Sehr geeherter Herr";
                    break;
                }
                case Gender.f:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Mrs";
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation = "Sehr geeherte Frau";
                    }
                        break;
                }
                case Gender.x:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Sirs";
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation = "Sehr geeherte Damen und Herren";
                    }
                        break;
                }
            }
        }

        private void init(out List<string> titels, out Dictionary<string, List<string>> genderDict)
        {
            titels = new List<string>();
            XmlDocument titleDoc = new XmlDocument();
            titleDoc.Load("Titles.xml");
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

        private void HandleError()
        {
        }

        private void HandleDb()
        {
        }

        public void addTitle()
        {
            _addTitleView.ShowDialog();
        }
    }
}