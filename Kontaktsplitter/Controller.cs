using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
             validateView = new ValidateView(_contactModelModel);

            inputView.DataContext = _contactModelModel;
            addTitleView.DataContext = _contactModelModel;
            validateView.DataContext = _contactModelModel;
            inputView.Show();

        }

        public void ParsString()
        {
            var inputList =_contactModelModel.Input.Split(' ');
            foreach (string s in inputList)
            {
                _contactModelModel.ListViewItems.Add(s);
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
