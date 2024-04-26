using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

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

        //Нажатие на кнопку "Итоги дня"
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Visible;
            MonthResault.Visibility = Visibility.Hidden;
            GeneralResualt.Visibility = Visibility.Hidden;

            ordersReceived1.Text = Convert.ToString(dbConnector.GetDayReceived());
            ordersIssued1.Text = Convert.ToString(dbConnector.GetDayIssued());
            ordersInStorage1.Text = Convert.ToString(dbConnector.GetDayInStorage());
            cellsFree.Text = Convert.ToString(dbConnector.GetCellsFree());
            cellsOccupied.Text = Convert.ToString(dbConnector.GetCellsOccupied());
            shelfLifeExpires.Text = Convert.ToString(dbConnector.GetCellsShelfLifeExpires());
        }


        // Нажатие на кнопку "Итоги месяца"
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResault.Visibility = Visibility.Visible;
            GeneralResualt.Visibility = Visibility.Hidden;

        }

        //Нажатие на кнопку "Общие итоги ПВЗ"
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResault.Visibility = Visibility.Hidden;
            GeneralResualt.Visibility = Visibility.Visible;
        }
    }
}
