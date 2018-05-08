using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontaktsplitter
{
    public class Controller
    {
        private readonly Contact _contact;

        public Controller()
        {
             _contact = new Contact();
            var inputView = new InputView(this);
            var addTitleView = new AddTitleView();
            var validateView = new ValidateView();

            inputView.DataContext = _contact;
            addTitleView.DataContext = _contact;
            validateView.DataContext = _contact;
            inputView.Show();

        }

        public void ParsString()
        {
            var inputList =_contact.Input.Split(' ');


        }

        private void HandleError()
        {

        }

        private void HandleDb()
        {

        }
    }

}
