using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using MessageBox = System.Windows.MessageBox;

namespace Kontaktsplitter
{
    public class Controller
    {
        private const string MALE = "Male";
        private readonly ContactModel _contactModelModel;
        private ValidateView _validateView;
        private readonly string EN = "EN";

        public Controller()
        {
            _contactModelModel = new ContactModel();
            var inputView = new InputView(this);

            inputView.DataContext = _contactModelModel;

            inputView.Show();
        }

        // Datensatz wird nach der manuellen Zuordnung abgespeichert
        public void Save()
        {
            WriteToHistory(_contactModelModel);
        }

        // Die eingegebenen Kontaktdaten werden bearbeitet
        public void ParsString()
        {
            _contactModelModel.Error = string.Empty;

            ClearProperties();
            if (_contactModelModel.Input.Length > 100)
            {
                // Gibt den Fehlertext an der Oberflaeche aus
                _contactModelModel.Error = "Ihre Eingabe ist zu lang!";
                return;
            }

            _contactModelModel.Input = _contactModelModel.Input.Trim();

            if (_contactModelModel.Input.Any(char.IsDigit))
            {
                // Gibt den Fehlertext an der Oberflaeche aus
                _contactModelModel.Error = "Eingabe enthält unerlaubte Zahlen!";
                return;
            }

            if (_contactModelModel.Input == string.Empty)
            {
                // Gibt den Fehlertext an der Oberflaeche aus
                _contactModelModel.Error = "Keine Eingabe Vorhanden";
                return;
            }
            
            var inputList = _contactModelModel.Input.Split(' ').ToList();
            inputList.RemoveAll(p => p.Equals(""));
            if (inputList.Count == 1)
            {
                var dialogResult = MessageBox.Show("Sind sie sicher dass sie nur einen Namen eingeben wollen?",
                    "Hinweis", MessageBoxButton.YesNo);
                if (dialogResult == (MessageBoxResult) DialogResult.No) return;
            }

            init(out var titels, out var genderDict);

            var input = SetLastAndFirstname(inputList, _contactModelModel.Input.Trim());


            input = SetGenderSalutationAndCountry(genderDict, input);


            input = SetTitle(input, titels);


            NotMatchingStrings(input);

            SetGender();
            CloseWindowAndRemoveInput();
        }

        //Setzt das letzte wort auf den Nachnamen, falls kein Komma enthalten ist. Ansonten wird vor dem Komma als Nachname behandelt und nach dem Komma als Vorname
        public string SetLastAndFirstname(List<string> inputList, string input)
        {
            string lastname;
            if (inputList.Count >= 2 && !input.Contains(","))
            {
                lastname = input.Split(' ').Last();
                _contactModelModel.LastName = lastname;
                input = ReplaceLastOccurence(input, lastname, "").Trim();
            }
            else if (inputList.Count >= 2 && inputList[inputList.Count - 2].Contains(","))
            {
                lastname = inputList[inputList.Count - 2].Replace(",", "");
                var firstname = inputList[inputList.Count - 1];
                _contactModelModel.LastName = lastname;
                _contactModelModel.FirstName = firstname;
                input = ReplaceFirstOccurrence(input, lastname, "").Trim();
                input = ReplaceLastOccurence(input, firstname, "").Trim();
                input = input.Replace(",", "");
            }

            return input;
        }

        // Ersetzt das erste Vorkommen eines Strings in einem String 
        public string ReplaceFirstOccurrence (string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        // Ersetzt das letzte Vorkommen eines Strings in einem String
        public string ReplaceLastOccurence (string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);
            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        // Wird am Ende des Parsens aufgerufen, falls nur noch ein Wort übrig handelt es sich um den Vornamen. Ansonten wird für jedes Wort ein Listenitem erstellt, welches manuell zugeordnet werden kann
        public void NotMatchingStrings(string input)
        {
            var unmatchedStrings = input.Split(' ');
            unmatchedStrings = unmatchedStrings.Where(val => val != "").ToArray();
            if (unmatchedStrings.Length == 1)
            {
                var firstname = unmatchedStrings[0];
                _contactModelModel.FirstName = firstname;
                unmatchedStrings = unmatchedStrings.Where(val => val != firstname).ToArray();
            }

            //Nicht zuordnenbare Strings als Listviewitems erzeugen
            foreach (var s in unmatchedStrings) _contactModelModel.ListViewItems.Add(s);
        }

        // Setzt die Titel basierend auf einer Liste von Titel und entfernt diese aus dem String der Wörter, die noch zugeordnet werden müssen
        public string SetTitle(string input, List<string> titels)
        {
            var inputList = input.Split(' ');
            foreach (var item in inputList.ToList())
                if (titels.Contains(item))
                {
                    _contactModelModel.Title += item + " ";
                    input = ReplaceFirstOccurrence(input, item, "");
                }

            return input.Trim();
        }

        // Setzt die Anrede, Geschlecht und das Land basierend auf den Daten aus dem Dictionary. 
        public string SetGenderSalutationAndCountry(Dictionary<string, List<string>> genderDict, string input)
        {
            var tempsplit = input.Split(' ');
            foreach (var item in genderDict)
                if (tempsplit.Contains(item.Key))
                {
                    genderDict.TryGetValue(item.Key, out var genderValue);
                    if (genderValue != null && genderValue.First().Equals(MALE))
                    {
                        _contactModelModel.Salutation = item.Key;
                        _contactModelModel.Gender = Gender.m;

                        if (genderValue.Last().Equals(EN))
                            _contactModelModel.Country = Country.EN;
                        else if (genderValue.Last().Equals("DE")) _contactModelModel.Country = Country.DE;
                    }
                    else
                    {
                        _contactModelModel.Salutation = item.Key;
                        _contactModelModel.Gender = Gender.f;

                        if (genderValue.Last().Equals(EN))
                            _contactModelModel.Country = Country.EN;
                        else
                            _contactModelModel.Country = Country.DE;
                    }

                    
                    input = ReplaceFirstOccurrence(input, item.Key, "");
                }


            return input.Trim();
        }

        // Leert alle Properties bzw. setzt diese auf den Defaultwert zurück
        private void ClearProperties()
        {
            _contactModelModel.Country = Country.DE; // Default
            _contactModelModel.FirstName = string.Empty;
            _contactModelModel.LastName = string.Empty;
            _contactModelModel.Title = string.Empty;
            _contactModelModel.Salutation = string.Empty;
            _contactModelModel.Gender = Gender.KeineAngabe; // Default 
            _contactModelModel.LetterSalutation = string.Empty;
        }

        // Schreibt den derzeitigen Datensatz als XML Knoten in die Contacts.xml
        public void WriteToHistory(ContactModel contact)
        {
            var doc = new XmlDocument();
            doc.Load(@"../../XML/Contacts.xml");
            var contactNode = doc.CreateNode(XmlNodeType.Element, "Contact", null);
            var newNode2 = doc.CreateNode(XmlNodeType.Element, "Input", null);
            newNode2.InnerText = contact.Input;
            var newNode3 = doc.CreateNode(XmlNodeType.Element, "Title", null);
            newNode3.InnerText = contact.Title;
            var newNode4 = doc.CreateNode(XmlNodeType.Element, "Salutation", null);
            newNode4.InnerText = contact.Salutation;
            var newNode5 = doc.CreateNode(XmlNodeType.Element, "LetterSalutation", null);
            newNode5.InnerText = contact.LetterSalutation;
            var newNode6 = doc.CreateNode(XmlNodeType.Element, "Gender", null);
            newNode6.InnerText = contact.Gender.ToString();
            var newNode7 = doc.CreateNode(XmlNodeType.Element, "FirstName", null);
            newNode7.InnerText = contact.FirstName;
            var newNode8 = doc.CreateNode(XmlNodeType.Element, "LastName", null);
            newNode8.InnerText = contact.LastName;
            var newNode9 = doc.CreateNode(XmlNodeType.Element, "Country", null);
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
                newNode9
            };

            foreach (var xmlNode in xmlNodes) contactNode.AppendChild(xmlNode);

            doc.DocumentElement?.AppendChild(contactNode);
            doc.Save(@"../../XML/Contacts.xml");
            _contactModelModel.Input = string.Empty;
            _validateView.Close();
        }

        // Schließt das Validierungsfenster und entfernt den Input
        private void CloseWindowAndRemoveInput()
        {
            _validateView = new ValidateView(this, _contactModelModel) {DataContext = _contactModelModel};
            _validateView.Gender.ItemsSource = Enum.GetValues(typeof(Gender));
            _validateView.Gender.SelectedItem = _contactModelModel.Gender;
            _validateView.ShowDialog();
            _contactModelModel.ListViewItems.Clear();
        }

        // Setzt basierend auf dem Geschlecht, welches im Modell hinterlegt ist, die passende Anrede und Briefanrede in der gewünschten Sprache
        private void SetGender()
        {
            switch (_contactModelModel.Gender)
            {
                case Gender.m:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Mr " + _contactModelModel.Title;
                        _contactModelModel.Salutation = "Mr";
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation = "Sehr geeherter Herr " + _contactModelModel.Title;
                        _contactModelModel.Salutation = "Herr";
                    }

                    break;
                }
                case Gender.f:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Mrs " + _contactModelModel.Title;
                        _contactModelModel.Salutation = "Mrs";
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation = "Sehr geeherte Frau " + _contactModelModel.Title;
                        _contactModelModel.Salutation = "Frau";
                    }

                    break;
                }
                case Gender.x:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Sirs " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation =
                            "Sehr geeherte Damen und Herren " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }

                    break;
                }
                case Gender.KeineAngabe:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Sirs " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation =
                            "Sehr geeherte Damen und Herren " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }

                    break;
                }
            }
        }

        // Lädt die XML Dateien in Listen bzw. Dictionaries, damit diese benutzt werden können
        public void init(out List<string> titels, out Dictionary<string, List<string>> genderDict)
        {
            titels = new List<string>();
            var titleDoc = new XmlDocument();
            titleDoc.Load(@"../../XML/Titles.xml");
            var root = titleDoc.DocumentElement;

            var genderDoc = new XmlDocument();
            genderDoc.Load(@"../../XML/Gender.xml");
            var genderRoot = genderDoc.DocumentElement;
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
                if (childNode.Attributes != null)
                    titels.Add(childNode.InnerText);
        }

        // Button click event für das hinzufügen eines Titels. Zeigt die entsprechende Oberfläche an
        public void AddTitle()
        {
            var addTitleView = new AddTitleView(_contactModelModel);
            addTitleView.DataContext = _contactModelModel;
            addTitleView.ShowDialog();
        }

        // Ruft die setGender Methode nochmals auf, um den Input der Felder neu zu laden
        public void ReloadInput()
        {
            SetGender();
        }

        // Bei ändern der Anrede werden das Geschlecht und die Briefanrede angepasst
        public void ReloadInputAnrede()
        {
            switch (_contactModelModel.Salutation)
            {
                case "Herr":
                {
                    _contactModelModel.Gender = Gender.m;
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Mr " + _contactModelModel.Title;
                        
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation = "Sehr geeherter Herr " + _contactModelModel.Title;
                        
                    }
                        break;
                }
                case "Frau":
                {
                    _contactModelModel.Gender = Gender.f;

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
                default:
                {
                    if (_contactModelModel.Country == Country.EN)
                    {
                        _contactModelModel.LetterSalutation = "Dear Sirs " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }
                    else
                    {
                        _contactModelModel.LetterSalutation =
                            "Sehr geeherte Damen und Herren " + _contactModelModel.Title;
                        _contactModelModel.Salutation = string.Empty;
                    }
                        break;
                }
            }
        }
    }
}