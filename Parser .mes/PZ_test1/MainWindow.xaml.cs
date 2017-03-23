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
            string wynik = "";
            string wynik_submatrix = "";
            int conf_length;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Mes File (*.mes)|*.mes";
            if (openFileDialog.ShowDialog() == true)
            {
                //plik_mes.Text = File.ReadAllTextopenFileDialog.FileName);
                string pattern_configuration = @"(configuration)(\s\b\w{1,4}\b){1,4}";
                string pattern_submatrix = @"(submatrix)\s+\d+(terms)\s+\d+\s(-)\s+\d+";
                Regex reg = new Regex(pattern_configuration, RegexOptions.IgnoreCase);
                Match match = reg.Match(File.ReadAllText(openFileDialog.FileName));
              //  Regex reg_submatrix = new Regex(pattern_submatrix, RegexOptions.IgnoreCase);
              //  Match match_submatrix = reg_submatrix.Match(File.ReadAllText(openFileDialog.FileName));              
                int matchCount = 0;
                while (match.Success)
                {
                    wynik = match.ToString();
                    conf_length = wynik.Length;
                    wynik=wynik.Substring(14,conf_length-14);
                    plik_mes.Text += "Match" + (++matchCount) + " " + wynik + "\n";
                  //  while(match_submatrix.Success)
                   
                    match = match.NextMatch();
                }

            }
        }
    }
}
