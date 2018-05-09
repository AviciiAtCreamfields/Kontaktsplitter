using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontaktsplitter
{
    public class ContactModel:INotifyPropertyChanged
    {
        private Country _country = Country.DE;
        private List<string> _listboxItems = new List<string>();
        private string _salutation="default";
        private string _title;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Input { get; set; }
        public string Salutation
        {
            get => _salutation;
            set
            {
                _salutation = value;
                OnPropertyChanged("Salutation");
            }
        }
        public string LetterSalutation { get; set; }
        public string Title {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
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

        public List<string> ListViewItems
        {
            get => _listboxItems;
            set
            {
                _listboxItems = value;
                OnPropertyChanged("ListViewItems");
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
