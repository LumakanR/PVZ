using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public partial class RegisterOrders : Window
    {
        private DBConnector dbConnector;

        public RegisterOrders()
        {
            InitializeComponent();

            // Инициализация экземпляра DBConnector
            dbConnector = new DBConnector();

            // Отображение всех данных при инициализации
            ShowAllData();
        }

        private void ShowAllData()
        {
            // Установка начального значения txtStatus
            txtStatus.Text = "Поступил";

            // Получение номера ячейки
            int cellNumber = GetNextCellNumber();

            // Отображение номера ячейки в поле txtCell
            txtCell.Text = cellNumber.ToString();

            // Отображение текущей даты
            txtData.Text = DateTime.Today.ToShortDateString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string date = txtData.Text;
            string status = txtStatus.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string orderNumber = txtIdOrder.Text;
            int rackNumber = int.Parse(txtRack.Text);
            int cellNumber = int.Parse(txtCell.Text);

            // Проверка доступности ячейки
            if (IsCellAvailable(cellNumber))
            {
                // Создание объекта Order
                Order order = new Order
                {
                    OrderID = orderNumber,
                    ArrivedDate = DateTime.Parse(date),
                    Status = status,
                    ClientPhoneNumber = phoneNumber,
                    CellID = cellNumber,
                    RackID = rackNumber
                };


                // Добавление заказа в базу данных с увеличенным OrderID и номером ячейки
                DBConnector.AddOrder(order);

                // Обновление txtIdOrder
                txtIdOrder.Text = orderNumber.ToString();

                // Обновление всех данных
                ShowAllData();
                // Сброс пользовательского номера ячейки
                txtCustomCell.Clear();
            }
            else
            {
                // Отображение окна ошибки, если ячейка занята
                MessageBox.Show("Данная ячейка уже занята", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private int GetNextCellNumber()
        {
            // Получение следующего доступного номера ячейки
            return 123;
        }

        private bool IsCellAvailable(int cellNumber)
        {
            // Проверка, свободна ли ячейка
            return dbConnector.IsCellAvailable(cellNumber);
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
