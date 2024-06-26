﻿using System.Windows;

namespace PVZ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
    }
}
