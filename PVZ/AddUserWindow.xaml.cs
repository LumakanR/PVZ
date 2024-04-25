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
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private DBConnector dbConnector;
        public AddUserWindow()
        {
            // Инициализация экземпляра DBConnector
            dbConnector = new DBConnector();
            InitializeComponent();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные пользователя из текстовых полей
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Проверяем, что оба поля заполнены
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните оба поля: Логин и Пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Добавляем пользователя
            dbConnector.AddUser(username, password);

            // Очищаем текстовые поля после добавления пользователя
            txtUsername.Clear();
            txtPassword.Clear();

            // Создание экземпляра окна отчетов
            MainWindowAdmin reportWindow = new MainWindowAdmin();

            // Отображение окна отчетов
            reportWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }
    }
}
