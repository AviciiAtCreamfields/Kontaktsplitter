using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kontaktsplitter
{
    /// <summary>
    /// Interaktionslogik für InputView.xaml
    /// </summary>
    public partial class InputView : Window
    {
        private readonly Controller _controller;
        public InputView(Controller cont)
        {
            _controller = cont;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ParsString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _controller.addTitle();
        }
    }
}
