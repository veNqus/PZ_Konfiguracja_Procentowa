using System;
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
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace Parser
{
    /// <summary>
    /// Interaction logic for ParserPage.xaml
    /// </summary>
    public partial class ParserPage : Page
    {



        //  Regex parts = new Regex(@"( *\( +\b(value_od)\, +\d+\) +-*\d.\d+\/)+");

        public ParserPage()
        {
            InitializeComponent();
        }
        /*
         W OUT pobieramy M i N -> Podmacierzm poziom
         W MES szukamy czy ma podmacierz, sumujemy z zakresów, i wyliczamy procent 
        */

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int value_submatrix = Convert.ToInt32(value0blok.Text);
            int value_poziom = Convert.ToInt32(poziom.Text);


            int value_next_submatrix = value_submatrix + 1;
            decimal cala_suma = 0;

            string wynik_configuration = "";
            string wynik_submatrix = "";
            string nr_submatrix="", min_submatrix="", max_submatrix="";
            string last_conf = "";
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

                StreamReader reader_mes = new StreamReader(@link);

                string line_mes;

                while ((line_mes = reader_mes.ReadLine()) != null)
                {
                    Match match_configuration = reg_configuration.Match(line_mes);
                    Match match_submatrix = reg_submatrix.Match(line_mes);
                    if (match_configuration.Success)
                    {
                        wynik_configuration = match_configuration.ToString();
                        conf_length = wynik_configuration.Length;
                        last_conf = wynik_configuration = wynik_configuration.Substring(14, conf_length - 14);
                        // plik_mes.Text += "\n" + wynik_configuration + "\n";
                    }
                    else if (match_submatrix.Success)
                    {
                        wynik_submatrix = match_submatrix.ToString();

                        nr_submatrix = wynik_submatrix.Substring(10, 2);
                        min_submatrix = wynik_submatrix.Substring(19, 4);
                        max_submatrix = wynik_submatrix.Substring(26, 4);
                        if(Convert.ToInt32(nr_submatrix) == value_submatrix)
                        {
                            // tu jest OK w parser OUT jest błąd na zatydzień do sprawdzenia
                            listBox.Items.Add(last_conf);
                            cala_suma += szukaj_w_out(nr_submatrix, value_poziom, min_submatrix, max_submatrix);
                            
                        }

                        //plik_mes.Text += "submatrix" + nr_submatrix + " " + min_submatrix + " " + max_submatrix + "\n";
                    }
                }
                MessageBox.Show(cala_suma.ToString());



            }
            

            // tu odaplamy mesa
           // mes_parser(value_submatrix, value_poziom, value_od, value_do);


        }
        public decimal szukaj_w_out(string submatrix, int poziom, string od, string do_poziomu)
        {
            decimal suma = 0;
            decimal duza_suma = 0;
            int flag = 0;
            bool flag2 = false;
            string line;
           
            int sub_matrix = Convert.ToInt32(submatrix) + 1;
            StreamReader reader = new StreamReader("Ni-even.out");
            // wyszukanie właściwej podmacierzy
            string reg = @"((EIGENVECTORS,SUBMATRIX)\s+)" + submatrix;
            Regex reg_S = new Regex(reg, RegexOptions.IgnoreCase);

            while ((line = reader.ReadLine()) != null)
            {
                
                Match Match__submatrix = reg_S.Match(line);
                if (Match__submatrix.Success || flag2)
                {
                    flag2 = true;

                    for (int i = Convert.ToInt32(od); i <= Convert.ToInt32(do_poziomu); i++)
                    {


                        // wyszukanie podmacierzy następnej
                        string reg2 = @"((EIGENVECTORS,SUBMATRIX)\s+)" + sub_matrix;
                        // wyszukanie poziomu
                        string regexstring = @"( *\( +" + poziom + @", +" + i + @"\) +-*\d.\d+\/)+";
                        Regex parts = new Regex(regexstring, RegexOptions.IgnoreCase);

                        Regex reg_s2 = new Regex(reg2, RegexOptions.IgnoreCase);

                        Match match = parts.Match(line);

                        Match Match__next_submatrix = reg_s2.Match(line);
                        if (match.Success)
                        {
                            string wartosc = match.ToString().Substring(12,9);
                            double wartosc_double = double.Parse(wartosc, System.Globalization.CultureInfo.InvariantCulture);
                            decimal wartosc_decimal = (decimal)wartosc_double;
                            suma += wartosc_decimal * wartosc_decimal;
                            //listBox.Items.Add(test2);

                        }
                        if (Match__next_submatrix.Success)
                        {
                            
                            flag2 = false;
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 1)
                    {
                        duza_suma += suma;
                        listBox.Items.Add(suma);
                        suma = 0;
                        break;
                    }
                }

            }
            return duza_suma;

        }
    }
}
