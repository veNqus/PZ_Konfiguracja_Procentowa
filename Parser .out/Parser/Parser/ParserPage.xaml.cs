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
 
       

      //  Regex parts = new Regex(@"( *\( +\b(value1)\, +\d+\) +-*\d.\d+\/)+");
        
        public ParserPage()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int value0 = Convert.ToInt32(value0blok.Text);
            int value1 = Convert.ToInt32(value1blok.Text);
            int value2 = Convert.ToInt32(value2blok.Text);
            for(int i=value1;i<=value2;i++)
            {
                StreamReader reader = new StreamReader("Ni-even.txt");
                string regexstring = @"( *\( +" + value0 + @", +" + i + @"\) +-*\d.\d+\/)+";
                Regex parts = new Regex(regexstring, RegexOptions.IgnoreCase);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = parts.Match(line);
                    if (match.Success)
                    {
                        listBox.Items.Add(match);
                    }
                }
            }

        }
    }
}
