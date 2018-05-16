using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Kontaktsplitter
{
    /// <summary>
    /// Interaktionslogik für ValidateView.xaml
    /// </summary>
    public partial class ValidateView : Window
    {
        private Point startPoint;
        private ContactModel context;
        private Controller _controller;
        private string _Contact;

        //Initialisieren der Validateview
        public ValidateView(Controller controller, ContactModel cont)
        {
            _controller = controller;
            context = cont;
            
            InitializeComponent();
        }

        //Merken der Startposition waehrend des Drag n Drop Vorgangs
        private void ListboxFolder1_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            startPoint = e.GetPosition(null);
        }

        //Merken der aktuellen Mausposition waehrend des Drag n Drop Vorgangs
        private void ListboxFolder1_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Holen des zu dragenden Listviewitems
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem!=null)
                {
                    //Finden der Daten die im Listviewitem stehen
                     _Contact = (string)listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem);

                    // Initialisieren des Drag n Drop Vorgangs
                    DataObject dragData = new DataObject(DataFormats.Text, _Contact);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
               
            }
        }
        // Helper um im Visual Tree zu suchen
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }


        //Sobald das Element losgelassen wird auf einer Textbox
        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string contact = e.Data.GetData(DataFormats.Text) as string;
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(context.Salutation);
                stringBuilder.Append(" ");
                stringBuilder.Append(contact);
                var cont = (ContactModel) this.DataContext;
                var check = cont.Salutation; 
            }

        }

        //DragDrop Effect ausloesen
        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.Text) ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        //sobald der Uebernehmen Button geklickt wird, wird die Savem Methode aufgerufen und die Daten persistiert.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _controller.Save();

        }

        //Sobald sich einer der Werte in einer Textbox aendert werden die anderen Werte angepasst.
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
                var cont = (ContactModel)DataContext;
                cont.ListViewItems.Remove(_Contact);
            
                ListView.Items.Refresh();
            
        }

        //Wenn die NArede oder der Titel geaendert werden muss das geschlecht und die Briefanrede geaendert werden
        private void Anrede_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var erg = (TextBox) sender;
            if (erg.Name == "Anrede")
            {
                _controller.ReloadInputAnrede();
            }
            else if(erg.Name == "TitleBox")
            {
                _controller.ReloadInput();
            }
            
            
        }

        //Wenn sich die Geschlechtsangabe aendert werden die davon abhaengigen felder veraendert
        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            context.Gender = (Gender) Gender.SelectedItem;
            _controller.ReloadInput();
        }
    }
}
