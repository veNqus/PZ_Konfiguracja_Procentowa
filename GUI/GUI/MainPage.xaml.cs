using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Text.RegularExpressions;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonPrzeszukajOut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Out File (*.out)|*.out";
            if (openFileDialog.ShowDialog() == true)
            {
                Regex parts = new Regex(@"( *\( +\d+\, +\d+\) +-*\d.\d+\/)+");
                string link = openFileDialog.FileName;
                //StreamReader reader = new StreamReader(File.ReadAllText(openFileDialog.FileName));
                //StreamReader reader = new StreamReader(@"../../../../Pliki/Ni-even.out");
                StreamReader reader = new StreamReader(@link); 
                
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = parts.Match(line);
                    if (match.Success)
                    {
                        textBoxOut.Items.Add(match);
                    }
                }
            }
        }

        private void buttonPrzeszukajMes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Mes File (*.mes)|*.mes";
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxMes.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void textBoxOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(textBoxOut.SelectedItem.ToString());
        }
    }
}
