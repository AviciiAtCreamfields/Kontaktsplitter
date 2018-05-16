using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Kontaktsplitter
{
    public class ContactModel:INotifyPropertyChanged
    {
        private Country _country = Country.DE;
        private List<string> _listboxItems = new List<string>();
        private string _salutation;
        private string _title;
        private string _lastName;
        private Gender _gender;
        private string _letterSalutation;
        private string _input;
        private string _firstName;
        public event PropertyChangedEventHandler PropertyChanged;

        //Eingabe String Property
        public string Input {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged("Input");
            }
        }


        //Anrede Property
        public string Salutation
        {
            get => _salutation;
            set
            {
                _salutation = value;
                OnPropertyChanged("Salutation");
            }
        }


        //Generierte Briefanrede
        public string LetterSalutation {
            get => _letterSalutation;
            set
            {
                _letterSalutation = value;
                OnPropertyChanged("LetterSalutation");
            }
        }

        // Titel String Property
        public string Title {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }


        // Geschlecht 
        public Gender Gender {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

        //Vorname Property
        public string FirstName {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        //Nachname Property
        public string LastName {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }


        //Laenderkennung um zu erkennen, welche Sprache zu verwenden ist
        public Country Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }

        }


        //Nicht zuordnenbare Elemente, welche die Eingabe enthalten hat
        public List<string> ListViewItems
        {
            get => _listboxItems;
            set
            {
                _listboxItems = value;
                OnPropertyChanged("ListViewItems");
            }
        }

        [XmlIgnore]
        public List<string> _TitlesList = new List<string>();

        private string _Error;


        // Liste der Titel die im XML stehen
        public List<string> TitlesList
        {
            get => _TitlesList;
            set
            {
                _TitlesList = value;
                OnPropertyChanged("TitlesList");
            }
        }


        //Bei fehler Ausgaben steht der Text hier und wird in der Oberflaeche angezeigt
        public string Error {
            get => _Error;
            set
            {
                _Error = value;
                OnPropertyChanged("Error");
            }
        }


        //Sobald sich was in den Properties aendert wird die Oberlaeche aktualisiert
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    //Moegliche Laender die erkannt werden
    public enum Country{DE,EN}

    //Auswahl der Geschlechterkennung
    public enum Gender {x,f,m,KeineAngabe}


}
