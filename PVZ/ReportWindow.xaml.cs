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
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();

            // Загрузка данных о товарах на складе и привязка их к ListView
            LoadInventoryData();
        }

        // Метод для загрузки данных о товарах на складе и отображения их в ListView
        private void LoadInventoryData()
        {
            try
            {
                // Создание экземпляра DBConnector
                DBConnector dbConnector = new DBConnector();

                // Получение данных о товарах на складе
                List<InventoryItem> inventoryItems = dbConnector.GetInventoryDataReports();

                // Привязка данных к ListView
                InventoryListView.ItemsSource = inventoryItems;
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке данных
                MessageBox.Show($"Ошибка при загрузке данных о товарах на складе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра главного окна
            MainWindow mainWindow = new MainWindow();

            // Отображение главного окна
            mainWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }
    }
}
