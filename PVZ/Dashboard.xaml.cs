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
using System.Windows.Shapes;

namespace PVZ
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private DBConnector dbConnector;

        public Dashboard()
        {
            InitializeComponent();
            dbConnector = new DBConnector();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Visible;
            MonthResault.Visibility = Visibility.Hidden;
            GeneralResualt.Visibility = Visibility.Hidden;

            ordersReceived1.Text = Convert.ToString(dbConnector.GetDayReceived());
            ordersIssued1.Text = Convert.ToString(dbConnector.GetDayIssued());
            ordersInStorage1.Text = Convert.ToString(dbConnector.GetDayInStorage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResault.Visibility = Visibility.Visible;
            GeneralResualt.Visibility = Visibility.Hidden;

            ordersReceived2.Text = Convert.ToString(dbConnector.GetMonthReceived());
            ordersIssued2.Text = Convert.ToString(dbConnector.GetMonthIssued());
            //ordersInStorage2.Text = Convert.ToString(dbConnector.GetMonthInStorage());
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResault.Visibility = Visibility.Hidden;
            GeneralResualt.Visibility = Visibility.Visible;
        }
    }
}
