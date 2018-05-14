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

        public string Input {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged("Input");
            }
        }

        public string Salutation
        {
            get => _salutation;
            set
            {
                _salutation = value;
                OnPropertyChanged("Salutation");
            }
        }

        public string LetterSalutation {
            get => _letterSalutation;
            set
            {
                _letterSalutation = value;
                OnPropertyChanged("LetterSalutation");
            }
        }

        public string Title {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public Gender Gender {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

       
        public string FirstName {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public Country Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }

        }

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

        public List<string> TitlesList
        {
            get => _TitlesList;
            set
            {
                _TitlesList = value;
                OnPropertyChanged("TitlesList");
            }
        }

        public string Error {
            get => _Error;
            set
            {
                _Error = value;
                OnPropertyChanged("Error");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }


    public enum Country{DE,EN}
    public enum Gender {x,f,m}


}
