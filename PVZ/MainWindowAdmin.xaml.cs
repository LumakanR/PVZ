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
    /// Логика взаимодействия для MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        public MainWindowAdmin()
        {
            InitializeComponent();
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна регистрации заказа
            RegisterOrders registerOrdersWindow = new RegisterOrders();

            // Отображение окна
            registerOrdersWindow.Show();

            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна учета склада
            InventoryWindow inventoryWindow = new InventoryWindow();

            // Отображение окна учета склада
            inventoryWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна отчетов
            ReportWindow reportWindow = new ReportWindow();

            // Отображение окна отчетов
            reportWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна отчетов
            ChangePasswordWindow reportWindow = new ChangePasswordWindow();

            // Отображение окна отчетов
            reportWindow.Show();

            // Закрытие текущего окна
            this.Hide();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра окна отчетов
            AddUserWindow reportWindow = new AddUserWindow();

            // Отображение окна отчетов
            reportWindow.Show();

            // Закрытие текущего окна
            this.Hide();
        }
    }
}
