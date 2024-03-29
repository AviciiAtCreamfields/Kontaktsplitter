﻿using System;
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

        //Initialisieren der Inputview
        public InputView(Controller cont)
        {
            _controller = cont;
            InitializeComponent();
        }

        //Sobald der OK Button geklickt wird, wird die eingabe geparst in der ParsString methode
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ParsString();
        }


        //Um Titel hinzu zu fuegen wird eine neue View geoeffnet
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _controller.AddTitle();
        }

       
        //Damit die Eingabe auch durch druecken von Enter direkt geparst werden kann wird dieses Event gefeuert, welches dann die ParsString methode aufruft.
        private void TextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var cont = (ContactModel) DataContext;
                cont.Input = this.TextBox.Text;
                _controller.ParsString();
            }
        }
    }
}
