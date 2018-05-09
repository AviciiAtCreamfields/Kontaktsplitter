using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
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
    /// Interaktionslogik für AddTitleView.xaml
    /// </summary>
    public partial class AddTitleView : Window
    {
        public const string XML_NAME = "Titles.xml";
        public const string XML_PARENT = "titles";
        public const string XML_CHILD = "title";



        public AddTitleView()
        {
      
            InitializeComponent();
        }

        public void addTitleToXML(String title)
        {
            if (!File.Exists(XML_NAME))
            {
                XmlWriter xmlWriter = XmlWriter.Create(XML_NAME);
                xmlWriter.WriteStartElement(XML_PARENT);
                xmlWriter.WriteStartElement(XML_CHILD);
                xmlWriter.WriteString(title);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XML_NAME);
                bool redundantTitle = parseXml(title, doc);

                if (redundantTitle == true)
                {
                    // TODO: Fehler ausgeben "Titel ist bereits vorhanden" 
                    return;
                }

                XmlNode newNode = doc.CreateNode(XmlNodeType.Element, XML_CHILD, null);
                newNode.InnerText = title;
                doc.DocumentElement.AppendChild(newNode);
                doc.Save(XML_NAME);
            }

        }
        
        //Gibt true zurück falls ein Knoten mit dem input als Wert gefunden wurde, andernfalls false
        public bool parseXml(String input, XmlDocument doc)
        {
            XmlNodeList allNodes = doc.GetElementsByTagName(XML_CHILD);
            foreach (XmlNode node in allNodes)
            {

                if (node.InnerText.Equals(input))
                {
                    return true;
                }
            }

            return false;
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addTitleToXML(title_textbox.Text);
            title_textbox.Text = "";
        }

        private void title_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ok_button.IsEnabled = true;
        }
    }
}
