using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontaktsplitter
{
    public class Contact
    {
        private Country _country = Country.DE;
        private List<string> _listboxItems = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public string Input { get; set; }
        public string Salutation { get; set; }
        public string LetterSalutation { get; set; }
        public string Title { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public Country Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }

        }

        public List<string> ListboxItems
        {
            get => _listboxItems;
            set
            {
                _listboxItems = value;
                OnPropertyChanged("ListBoxItems");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    public enum Country{DE,EN}
    public enum Gender {m,f,x}
}
