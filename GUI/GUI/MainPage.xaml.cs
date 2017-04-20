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

        StreamReader reader;
        string link;

        private void buttonPrzeszukajOut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Out File (*.out)|*.out";
            if (openFileDialog.ShowDialog() == true)
            {

                //Regex parts = new Regex(@"( *\( +\d+\, +\d+\) +-*\d.\d+\/)+");
                link = openFileDialog.FileName;
                //StreamReader reader = new StreamReader(File.ReadAllText(openFileDialog.FileName));
                //StreamReader reader = new StreamReader(@"../../../../Pliki/Ni-even.out");
                reader = new StreamReader(@link); 
                
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //Match match = parts.Match(line);
                    //if (match.Success)
                    //{
                    //textBoxOut.Items.Add(match);
                    //}
                    textBoxOut.Items.Add(line);
                }
            }
        }

        private void buttonPrzeszukajMes_Click(object sender, RoutedEventArgs e)
        {
            /*
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Mes File (*.mes)|*.mes";
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxMes.Text = File.ReadAllText(openFileDialog.FileName);
            }
            */
            {
                string wynik_configuration = "";
                string wynik_submatrix = "";
                string nr_submatrix, min_submatrix, max_submatrix;
                int conf_length;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Mes File (*.mes)|*.mes";
                if (openFileDialog.ShowDialog() == true)
                {

                    string link = openFileDialog.FileName;
                    string pattern_configuration = @"(configuration)(\s\b\w{1,4}\b){1,4}";
                    string pattern_submatrix = @"((submatrix)\s+\d+\s+(terms)\s+\d+\s(-)\s+\d+)";

                    Regex reg_configuration = new Regex(pattern_configuration, RegexOptions.IgnoreCase);
                    Regex reg_submatrix = new Regex(pattern_submatrix, RegexOptions.IgnoreCase);

                    StreamReader reader = new StreamReader(@link);

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Match match_configuration = reg_configuration.Match(line);
                        Match match_submatrix = reg_submatrix.Match(line);
                        if (match_configuration.Success)
                        {
                            wynik_configuration = match_configuration.ToString();
                            conf_length = wynik_configuration.Length;
                            wynik_configuration = wynik_configuration.Substring(14, conf_length - 14);
                            textBoxMes.Text += "\n" + wynik_configuration + "\n";
                        }
                        else if (match_submatrix.Success)
                        {
                            wynik_submatrix = match_submatrix.ToString();
                            nr_submatrix = wynik_submatrix.Substring(10, 2);
                            min_submatrix = wynik_submatrix.Substring(19, 4);
                            max_submatrix = wynik_submatrix.Substring(26, 4);

                            textBoxMes.Text += "submatrix" + nr_submatrix + " " + min_submatrix + " " + max_submatrix + "\n";
                        }
                    }



                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBoxOut.Items.Clear();
            int value_submatrix = Convert.ToInt32(value0blok.Text);
            int value_poziom = Convert.ToInt32(poziom.Text);
            int value_od = Convert.ToInt32(value1blok.Text);
            int value_do = Convert.ToInt32(value2blok.Text);
            int value_next_submatrix = value_submatrix + 1;
            int flag = 0;
            bool flag2 = false;
            string line;
            reader = new StreamReader(@link);
            string reg = @"((EIGENVECTORS,SUBMATRIX)\s+)" + value_submatrix;
            Regex reg_S = new Regex(reg, RegexOptions.IgnoreCase);

            while ((line = reader.ReadLine()) != null)
            {
                Match Match2 = reg_S.Match(line);
                if (Match2.Success || flag2)
                {
                    flag2 = true;

                    for (int i = value_od; i <= value_do; i++)
                    {
                        string reg2 = @"((EIGENVECTORS,SUBMATRIX)\s+)" + value_next_submatrix;
                        string regexstring = @"( *\( +" + value_poziom + @", +" + i + @"\) +-*\d.\d+\/)+";
                        Regex parts = new Regex(regexstring, RegexOptions.IgnoreCase);

                        Regex reg_s2 = new Regex(reg2, RegexOptions.IgnoreCase);

                        Match match = parts.Match(line);

                        Match Match3 = reg_s2.Match(line);
                        if (match.Success)
                        {
                            textBoxOut.Items.Add(match);
                        }
                        if (Match3.Success)
                        {
                            flag2 = false;
                            flag = 1;

                        }

                    }
                    if (flag == 1)
                    {
                        break;
                    }
                }

            }
        }

        private void textBoxOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Regex reg = new Regex(@" +\d+ +\d+ +");
            string linia =textBoxOut.SelectedItem.ToString();
            Match matchKlik = reg.Match(linia);
            MessageBox.Show(linia + "\n" + matchKlik.ToString());
            
            // REGEX NA POZNIEJ  +\d+ +\d+ +
        }
    }
}
