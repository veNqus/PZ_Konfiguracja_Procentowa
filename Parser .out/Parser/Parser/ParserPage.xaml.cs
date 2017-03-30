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
            int value_od = Convert.ToInt32(value1blok.Text);
            int value_do = Convert.ToInt32(value2blok.Text);
            int value_next_submatrix = value_submatrix + 1;
            int flag = 0;
            bool flag2 = false;
            string line;
            StreamReader reader = new StreamReader("Ni-even.out");
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
                            listBox.Items.Add(match);
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
    }
}
