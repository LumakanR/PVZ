using System;
using System.Windows;

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
            int cellNumber = dbConnector.GetNextCellNumber();

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
            if (dbConnector.IsCellAvailable(cellNumber))
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

                // Добавление заказа в базу данных
                DBConnector.AddOrder(order);

                // Обновление txtIdOrder
                txtIdOrder.Text = orderNumber.ToString();

                // Обновление всех данных
                ShowAllData();
            }
            else
            {
                // Отображение окна ошибки, если ячейка занята
                MessageBox.Show("Данная ячейка уже занята", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private int GetNextCellNumber()
        {
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

        private void radioManual_Checked(object sender, RoutedEventArgs e)
        {
            // Установить поля для ручного ввода в режим редактирования
            txtPhoneNumber.IsReadOnly = false;
            txtRack.IsReadOnly = false;
            txtCell.IsReadOnly = false;
        }

        private void radioAuto_Checked(object sender, RoutedEventArgs e)
        {
            // Установить поля для автоматического ввода в режим только для чтения
            txtPhoneNumber.IsReadOnly = false;
            txtRack.IsReadOnly = true;
            txtCell.IsReadOnly = true;
        }
    }
}