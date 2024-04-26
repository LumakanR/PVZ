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
        public List<Inventory> InventoryData { get; set; }


        public SeriesCollection OrderSeries { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public class OrderData
        {
            public string Month { get; set; }
            public int OrdersReceived { get; set; }
        }

        public Dashboard()
        {
            InitializeComponent();
            dbConnector = new DBConnector();
            DataContext = new ChartViewModel();
            OrderDataAccess dataAccess = new OrderDataAccess();
            List<OrderData> orderData = dataAccess.GetReceivedOrdersByMonth();

            OrderSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Orders Received",
                    Values = new ChartValues<int>(orderData.Select(x => x.OrdersReceived))
                }
            };

            Labels = orderData.Select(x => x.Month).ToArray();
            Formatter = value => value.ToString("N0");

            DataContext = this;
        }

        //Нажатие на кнопку "Итоги дня"
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Visible;
            MonthResaultEmployees.Visibility = Visibility.Hidden;
            MonthResaultDays.Visibility = Visibility.Hidden;
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
            MonthResaultEmployees.Visibility = Visibility.Hidden;
            MonthResaultDays.Visibility = Visibility.Visible;
            GeneralResualt.Visibility = Visibility.Hidden;
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResaultEmployees.Visibility = Visibility.Visible;
            MonthResaultDays.Visibility = Visibility.Hidden;
            GeneralResualt.Visibility = Visibility.Hidden;
        }

        private void Days_Click(object sender, RoutedEventArgs e)
        {
            DayResualt.Visibility = Visibility.Hidden;
            MonthResaultEmployees.Visibility = Visibility.Hidden;
            MonthResaultDays.Visibility = Visibility.Visible;
            GeneralResualt.Visibility = Visibility.Hidden;
        }

        private void Serch_Click(object sender, RoutedEventArgs e)
        {
            InventoryData = dbConnector.GetInventoryDataDays(Convert.ToDateTime(SelecteMonth));
            InventoryListView3.DataContext = this;
        }
    }
}
