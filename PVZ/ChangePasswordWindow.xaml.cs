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
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private DBConnector dbConnector;
        public ChangePasswordWindow()
        {
            // Инициализация экземпляра DBConnector
            dbConnector = new DBConnector();

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string currentPassword = txtPassword.Text;
            string newPassword = txtNewPassword.Text;

            // Проверяем, является ли текущий пользователь менеджером
            bool isManager = DBConnector.ValidateUser(username, currentPassword) && username == "1" && currentPassword == "1"; // Замените "manager" и "2" на логин и пароль менеджера

            if (isManager)
            {
                // Выполняем смену пароля для менеджера
                dbConnector.ChangePassword(username, newPassword);
                MessageBox.Show("Пароль успешно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Создание экземпляра окна отчетов
                MainWindow reportWindow = new MainWindow();

                // Отображение окна отчетов
                reportWindow.Show();

                // Закрытие текущего окна
                this.Close();
            }
            else if (DBConnector.ValidateUser(username, currentPassword))
            {
                // Если текущий пользователь не менеджер, но валиден, то выполняем смену его пароля
                dbConnector.ChangePassword(username, newPassword);
                MessageBox.Show("Пароль успешно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Создание экземпляра окна отчетов
                MainWindow reportWindow = new MainWindow();

                // Отображение окна отчетов
                reportWindow.Show();

                // Закрытие текущего окна
                this.Close();
            }
            else
            {
                // Если введены неверные учетные данные, выводим сообщение об ошибке
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
