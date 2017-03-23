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
        int value1 = 1;
        int value2 = 2;
            Regex parts = new Regex(@"( *\( +\d+\, +\d+\) +-*\d.\d+\/)+"); 
      //  Regex parts = new Regex(@"( *\( +\b(value1)\, +\d+\) +-*\d.\d+\/)+");
        StreamReader reader = new StreamReader("PlikTestowy.txt.txt");
        public ParserPage()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

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
