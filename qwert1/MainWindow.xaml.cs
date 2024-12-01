
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
using qwert1.Properties;
using qwert1.pages;
using System.Diagnostics.Eventing.Reader;


namespace qwert1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Autho ());
        }
        
       

        private void Назад(object sender, TextCompositionEventArgs e)
        {

        }

        private void ButtonB_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }
        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack) 
            
                ButtonB.Visibility = Visibility.Visible;
            else
                ButtonB.Visibility = Visibility.Hidden;
            
        }

        private void FrmMain_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
