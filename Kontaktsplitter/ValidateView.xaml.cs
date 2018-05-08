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

        public ValidateView(ContactModel cont)
        {
            //DataContext = cont;
            context = cont;
            
            InitializeComponent();
        }

        private void ListboxFolder1_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void ListboxFolder1_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem!=null)
                {
                    // Find the data behind the ListViewItem
                    string contact = (string)listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", contact);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
               
            }
        }
        // Helper to search up the VisualTree
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

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                string contact = e.Data.GetData("myFormat") as string;
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(context.Salutation);
                stringBuilder.Append(" ");
                stringBuilder.Append(contact);
                var cont = (ContactModel) DataContext;
                cont.Salutation = stringBuilder.ToString();
            }

        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.Property.Name);
            base.OnPropertyChanged(e);
        }

        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
