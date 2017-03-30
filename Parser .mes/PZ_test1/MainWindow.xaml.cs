using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PZ_test1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
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
                        plik_mes.Text += "\n" + wynik_configuration + "\n";
                    }
                    else if (match_submatrix.Success)
                    {
                        wynik_submatrix = match_submatrix.ToString();
                        nr_submatrix = wynik_submatrix.Substring(10, 2);
                        min_submatrix = wynik_submatrix.Substring(19, 4);
                        max_submatrix = wynik_submatrix.Substring(26, 4);

                        plik_mes.Text += "submatrix" + nr_submatrix + " " + min_submatrix + " " + max_submatrix + "\n";
                    }
                }



            }
        }
    }
}
