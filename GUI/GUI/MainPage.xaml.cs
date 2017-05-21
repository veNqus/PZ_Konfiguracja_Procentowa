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
        public MainPage(string linkOut, string linkMes)
        {
            InitializeComponent();
            this.linkMes = linkMes;
            this.linkOut = linkOut;
            reader = new StreamReader(@linkOut);

            string reg = @"((ITERATION NO.)\s+)" + 1;
            Regex reg_S = new Regex(reg, RegexOptions.IgnoreCase);
            string reg2 = @"((ITERATION NO)\s+)" + 1;
            Regex reg_S2 = new Regex(reg, RegexOptions.IgnoreCase);
            string line;
            Regex reg3 = new Regex(@" +\d+ +\d+");
            bool flag = false;
            while ((line = reader.ReadLine()) != null)
            {
                Match Match__submatrix = reg_S.Match(line);
                if (Match__submatrix.Success || flag)
                {
                    flag = true;
                    do
                    {
                        line = reader.ReadLine();
                        Match Match__submatrix3 = reg3.Match(line);
                        if (Match__submatrix3.Success)
                            textBoxOut.Items.Add(line);
                        Match Match__submatrix2 = reg_S2.Match(line);
                        if (Match__submatrix2.Success)
                        {
                            flag = false;
                        }
                    } while (flag);

                }
            }
        }

        StreamReader reader;
        string linkOut;
        string linkMes;

        private void buttonPrzeszukajOut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Out File (*.out)|*.out";
            if (openFileDialog.ShowDialog() == true)
            {

                //Regex parts = new Regex(@"( *\( +\d+\, +\d+\) +-*\d.\d+\/)+");
                linkOut = openFileDialog.FileName;
                reader = new StreamReader(@linkOut);
                
                string reg = @"((ITERATION NO.)\s+)" + 1; 
                Regex reg_S = new Regex(reg, RegexOptions.IgnoreCase);
                string reg2 = @"((ITERATION NO)\s+)" + 1;
                Regex reg_S2 = new Regex(reg, RegexOptions.IgnoreCase);
                string line;
                Regex reg3 = new Regex(@" +\d+ +\d+");
                bool flag = false;
                while ((line = reader.ReadLine()) != null)
                {
                    Match Match__submatrix = reg_S.Match(line);
                    if (Match__submatrix.Success || flag)
                    {
                        flag = true;
                        do
                        {
                            line = reader.ReadLine();
                            Match Match__submatrix3 = reg3.Match(line);
                            if (Match__submatrix3.Success)
                                textBoxOut.Items.Add(line);
                            Match Match__submatrix2 = reg_S2.Match(line);
                            if (Match__submatrix2.Success)
                            {
                                flag = false;
                            }
                        } while (flag);

                    }
                }
            }
        }

        private void buttonPrzeszukajMes_Click(object sender, RoutedEventArgs e)
        {
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Mes File (*.mes)|*.mes";
                if (openFileDialog.ShowDialog() == true)
                {
                    linkMes = openFileDialog.FileName;
                }
            }
        }
        private void textBoxOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(linkOut != "" && linkMes != "")
            {
                Regex reg = new Regex(@" *\d+ +\d+");
                string linia = textBoxOut.SelectedItem.ToString();
                Match matchKlik = reg.Match(linia);
                string cokolwiek = matchKlik.ToString().Substring(0, 2);
                int cokolwiekWartosc = Convert.ToInt32(cokolwiek);
                string cokolwiek2 = matchKlik.ToString().Substring(3, 3);
                int cokolwiekWartosc2 = Convert.ToInt32(cokolwiek2);

                EndPage endPage = new EndPage(cokolwiekWartosc, cokolwiekWartosc2, linkOut, linkMes);
                this.NavigationService.Navigate(endPage);

                // REGEX NA POZNIEJ  +\d+ +\d+ +
            }
            else
                MessageBox.Show("najpierw wczytaj plik OUT oraz MES");
        }
    }
}
