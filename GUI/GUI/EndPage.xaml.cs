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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for EndPage.xaml
    /// </summary>
    public partial class EndPage : Page
    {
        int par1;
        int par2;
        string linkOut;
        string linkMes;
        public EndPage()
        {
            InitializeComponent();
        }
        public EndPage(int par1, int par2, string linkOut, string linkMes)
        {
            InitializeComponent();
            this.par1 = par1;
            this.par2 = par2;
            this.linkOut = linkOut;
            this.linkMes = linkMes;


        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WyniktextBox.Text = par1.ToString() + "\n" + par2.ToString() + "\n" + linkOut + "\n" + linkMes;
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Plik Tekstowy|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter f = new StreamWriter(dialog.FileName);
                f.Write(WyniktextBox.Text);
                f.Close();
            }
        }

        private void WsteczButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            this.NavigationService.Navigate(mainPage);

        }
    }
}
