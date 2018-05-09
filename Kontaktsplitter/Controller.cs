using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kontaktsplitter
{
    public class Controller
    {
        private readonly ContactModel _contactModelModel;
        private InputView inputView;
        private AddTitleView addTitleView;
        private ValidateView validateView;

        public Controller()
        {
            _contactModelModel = new ContactModel();
            inputView = new InputView(this);
            addTitleView = new AddTitleView();
            validateView = new ValidateView(this, _contactModelModel);

            inputView.DataContext = _contactModelModel;
            addTitleView.DataContext = _contactModelModel;
            validateView.DataContext = _contactModelModel;
            inputView.Show();

        }

        public void TEST()
        {
            var foo = 1;
        }
    

    public void ParsString()
        {
            var inputList =_contactModelModel.Input.Split(' ');
            var titels = new List<string>();
            XmlDocument titelDoc = new XmlDocument();
            titelDoc.Load(@"C:\Users\Nils Lohmiller\Desktop\titel.xml");
            XmlElement root = titelDoc.DocumentElement;
            foreach (XmlNode childNode in root.ChildNodes)
            {
                if (childNode.Attributes != null) titels.Add(childNode.InnerText);
            }
            foreach (string s in inputList)
            {
                if (titels.Contains(s))
                {
                    _contactModelModel.Title += s;
                }
                else
                {
                    _contactModelModel.ListViewItems.Add(s);
                }
            }



            validateView.ShowDialog();
        }

        private void HandleError()
        {

        }

        private void HandleDb()
        {

        }

        public void addTitle()
        {
            addTitleView.ShowDialog();
        }
    }

}
