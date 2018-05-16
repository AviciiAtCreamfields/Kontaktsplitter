using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Kontaktsplitter
{
    /// <summary>
    /// Interaktionslogik für AddTitleView.xaml
    /// </summary>
    public partial class AddTitleView : Window
    {
        public const string XmlPath = @"../../XML/Titles.xml";
        public const string XmlParent = "titles";
        public const string XmlChild = "title";
        public ContactModel datencontext;


        //Initialisieren der AddTitleView
        public AddTitleView(ContactModel cont)
        {
            datencontext = cont;
            GetNodeInnertextDoc();
            InitializeComponent();
        }


        //Titel zur XML Datei hinzufuegen, sodass sie persisten vorhanden sind
        public void addTitleToXML(string title)
        {

            GetNodeInnertextDoc();

            if (datencontext.TitlesList.Contains(title))
            {
                // TODO: Fehler ausgeben "Titel ist bereits vorhanden" 
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);
            XmlNode newNode = doc.CreateNode(XmlNodeType.Element, XmlChild, null);
            newNode.InnerText = title.Trim();
            doc.DocumentElement.AppendChild(newNode);
            doc.Save(XmlPath);

            datencontext.TitlesList.Add(title);
            XML_List.Items.Refresh();
        }

        //Liste der Titel aus dem XML Dokument herauslesen
        private void GetNodeInnertextDoc()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);
            XmlNodeList allNodes = doc.GetElementsByTagName(XmlChild);
            datencontext.TitlesList.Clear();
            foreach (XmlNode allNode in allNodes)
            {
                datencontext.TitlesList.Add(allNode.InnerText);
            }

          
        }

        //sobald der Hinzufuegen Button geklickt wird, wird der eingegebene Titel zur XML hinzu gefuegt
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addTitleToXML(title_textbox.Text);
            title_textbox.Text = "";
        }


        //Um leere eingaben zu verhindern, wird der Hinzufuegen Button erst nach der Eingabe freigeschalten
        private void title_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ok_button.IsEnabled = true;
        }

    
        //Um Titel zu loeschen, folgt auf den klick des Loeschbuttons ein Event, welches das ausgewaehlte Element loescht
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (XML_List.SelectedItems != null)
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(XmlPath);
                XmlNodeList allNodes = doc.GetElementsByTagName(XmlChild);

                foreach (string itemSelected in XML_List.SelectedItems)
                {
                    datencontext.TitlesList.Remove(itemSelected);
                    datencontext.TitlesList = datencontext.TitlesList;
                    foreach (XmlNode node in allNodes)
                    {
                        if (node.InnerText.Equals(itemSelected))
                        {
                            XmlNode parent = node.ParentNode;
                            parent.RemoveChild(node);
                            
                            doc.Save(XmlPath);
                            break;
                        }
                    }
                }
                XML_List.Items.Refresh();

            }
        }
    }
}