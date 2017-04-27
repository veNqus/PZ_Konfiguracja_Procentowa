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

        decimal[,,] tablica = new decimal[20, 100, 10000];



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
            wypełnij_tablice();
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
            int podmacierz = Convert.ToInt32(submatrix);
            for (int i=Convert.ToInt32(od);i <= Convert.ToInt32(do_poziomu);i++)
            {
                suma += (tablica[podmacierz, poziom, i]) * (tablica[podmacierz, poziom, i]);

            }
            listBox.Items.Add(suma);    

            return suma;

        }

        public void wypełnij_tablice()
        {
            int last_submatrix=1;
            StreamReader reader = new StreamReader("Ni-even.out");
            string reg = @"((EIGENVECTORS,SUBMATRIX)\s+\d+)";
            Regex reg_S = new Regex(reg, RegexOptions.IgnoreCase);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Match Match__submatrix = reg_S.Match(line);
                if(Match__submatrix.Success)
                {
                    string podmacierz = Match__submatrix.ToString().Substring(22, 3);
                    last_submatrix = Convert.ToInt32(podmacierz);
                }
                // wyszukanie poziomu
                //string regexstring = @"( *\( +\d_+"+@", +\d+"+@"\) +-*\d.\d+\/)+";
                string test = @" \(\d+,\d+" +@"\)\d+)";
                string regexstring = (@"( *\( +\d+\, +\d+\) +-*\d.\d+\/)+");

                Regex parts = new Regex(regexstring, RegexOptions.IgnoreCase);
                                     
                Match match = parts.Match(line);

                      
                if (match.Success)
                {
                    for (int k=0;k<6;k++)
                    {
                        try
                        {
                            string poziom = match.ToString().Substring(2+(k*22), 3);
                            string numer = match.ToString().Substring(6+(k*22), 3);
                            string wartosc = match.ToString().Substring(12+(k*22), 9);
                            int poziom_int = Convert.ToInt32(poziom);
                            int numer_int = Convert.ToInt32(numer);
                            decimal wartosc_decimal = decimal.Parse(wartosc, System.Globalization.CultureInfo.InvariantCulture);
                            //    decimal wartosc_decimal = (decimal)wartosc_double;
                            tablica[last_submatrix, poziom_int, numer_int] = wartosc_decimal;
                        }
                        catch
                        {
                            break;
                        }
                        
                    }
                    string tekst = match.ToString();
                    
                            //listBox.Items.Add(test2);

                 }                       
              }
                   
                }

            }
        }


  