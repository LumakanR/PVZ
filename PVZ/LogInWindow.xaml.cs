﻿using System;
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
    /// Логика взаимодействия для LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private DBConnector dbConnector;
        public LogInWindow()
        {
            // Инициализация экземпляра DBConnector
            dbConnector = new DBConnector();
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            // Проверяем, является ли текущий пользователь менеджером
            bool isManager = DBConnector.ValidateUser(username, password) && username == "admin" && password == "admin";
            if (isManager)
            {

                MainWindowAdmin mainWindow = new MainWindowAdmin();
                mainWindow.Show();
                Close();
            }
            else if (DBConnector.ValidateUser(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Введите корректные данные для входа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
