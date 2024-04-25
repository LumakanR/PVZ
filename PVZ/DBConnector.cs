using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PVZ
{
    internal class DBConnector
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-NOO2N4A;Integrated Security=True");
        private const string ConnectionString = "Data Source=DESKTOP-NOO2N4A;Initial Catalog=PVZ_CHEMP;Integrated Security=True";

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getSqlConnection()
        {
            return sqlConnection;
        }

        public static bool ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public void ChangePassword(string username, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Обновляем пароль пользователя в базе данных
                string query = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@Username", username);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем, существует ли пользователь с таким именем
                if (UserExists(username))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Добавляем нового пользователя в базу данных
                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    command.ExecuteNonQuery();
                }
            }
        }

        private bool UserExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем, существует ли пользователь с указанным именем
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static string AddClient(string clientPhoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DBConnector dbConnector = new DBConnector();
                connection.Open();
                // Вставляем новую запись в таблицу Clients
                string queryInsert = "INSERT INTO Clients (ClientPhoneNumber) VALUES (@ClientPhoneNumber)";

                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                {
                    commandInsert.Parameters.AddWithValue("@ClientPhoneNumber", clientPhoneNumber);
                    commandInsert.ExecuteNonQuery();

                    return clientPhoneNumber;
                }
            }
        }

        public static int AddRack(int rackID, int cellID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string queryInsert = "INSERT INTO StorageRacks (RackID, CellID, isAvailable) VALUES (@RackID, @CellID, @isAvailable)";

                using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                {
                    commandInsert.Parameters.AddWithValue("@RackID", rackID);
                    commandInsert.Parameters.AddWithValue("@CellID", cellID);
                    commandInsert.Parameters.AddWithValue("@isAvailable", 0);
                    commandInsert.ExecuteNonQuery();

                    return rackID;
                }
            }
        }

        public static void AddOrder(Order order)
        {
            // Создаем новый экземпляр DBConnector
            DBConnector dbConnector = new DBConnector();

            // Создаем нового клиента и получаем его ClientID
            string clientPhoneNumber = AddClient(order.ClientPhoneNumber);
            int rackID = AddRack(order.RackID, order.CellID);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем доступность ячейки
                if (dbConnector.IsCellAvailable(order.CellID) || clientPhoneNumber == "null")
                {
                    // Добавляем заказ с полученным ClientID, OrderID и номером ячейки
                    string query = "INSERT INTO Orders (OrderNumber, ArrivedDate, Status, ClientPhoneNumber, RackID, CellID) " +
                                   "VALUES (@OrderNumber, @ArrivedDate, @Status, @ClientPhoneNumber, @RackID, @CellID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderNumber", order.OrderID); // Передаем OrderNumber
                        command.Parameters.AddWithValue("@ArrivedDate", order.ArrivedDate);
                        command.Parameters.AddWithValue("@Status", order.Status);
                        command.Parameters.AddWithValue("@ClientPhoneNumber", clientPhoneNumber);
                        command.Parameters.AddWithValue("@RackID", rackID);
                        command.Parameters.AddWithValue("@CellID", order.CellID);
                        command.ExecuteNonQuery();
                    }
                }
                else if (clientPhoneNumber == null)
                {
                    // Отобразить окно ошибки, если ячейка занята
                    MessageBox.Show("Данный телефон уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Отобразить окно ошибки, если ячейка занята
                    MessageBox.Show("Данная ячейка уже занята", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public bool IsCellAvailable(int cellNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем, существует ли указанная ячейка
                string query = "SELECT COUNT(*) FROM Orders WHERE CellID = @CellID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CellID", cellNumber);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0; // Возвращаем true, если ячейка свободна
                }
            }
        }

        public bool IsPhoneAvailable(string cellNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем, существует ли указанная ячейка
                string query = "SELECT COUNT(*) FROM Orders WHERE ClientPhoneNumber = @ClientPhoneNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientPhoneNumber", cellNumber);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0; // Возвращаем true, если ячейка свободна
                }
            }
        }

        public int GetLastOrderId()
        {
            using (SqlConnection connection = new SqlConnection(DBConnector.ConnectionString))
            {
                connection.Open();

                string query = "SELECT ISNULL(MAX(OrderID), 0) FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<InventoryItem> GetInventoryData()
        {
            List<InventoryItem> inventoryData = new List<InventoryItem>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Orders.OrderNumber, Orders.ArrivedDate, Orders.Status, Orders.ClientPhoneNumber, Orders.RackID, Orders.CellID  FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryItem item = new InventoryItem
                            {
                                OrderNumber = reader.GetString(0),
                                ArrivedDate = reader.GetDateTime(1),
                                Status = reader.GetString(2),
                                ClientPhoneNumber = reader.GetString(3),
                                RackID = reader.GetInt32(4),
                                CellID = reader.GetInt32(5),
                            };

                            inventoryData.Add(item);
                        }
                    }
                }
            }

            return inventoryData;
        }
        public bool DeliverOrder(string phoneNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Проверяем, существует ли заказ с указанным номером
                    string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE ClientPhoneNumber = @ClientPhoneNumber";
                    using (SqlCommand checkOrderCommand = new SqlCommand(checkOrderQuery, connection))
                    {
                        checkOrderCommand.Parameters.AddWithValue("@ClientPhoneNumber", phoneNumber);
                        int orderCount = Convert.ToInt32(checkOrderCommand.ExecuteScalar());

                        // Если заказ с указанным номером не найден, возвращаем false
                        if (orderCount == 0)
                        {
                            return false;
                        }
                    }

                    // Удаляем заказ из базы данных
                    string deleteOrderQuery = "DELETE FROM Orders WHERE ClientPhoneNumber = @ClientPhoneNumber";
                    using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection))
                    {
                        deleteOrderCommand.Parameters.AddWithValue("@ClientPhoneNumber", phoneNumber);
                        deleteOrderCommand.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибок, если необходимо
                Console.WriteLine("Ошибка при выдаче заказа: " + ex.Message);
                return false;
            }
        }

        public List<InventoryItem> GetInventoryDataReports()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT OrderNumber, ArrivedDate, Status, ClientPhoneNumber, RackID, CellID FROM Orders";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InventoryItem item = new InventoryItem
                                {
                                    OrderNumber = reader.GetString(0),
                                    ArrivedDate = reader.GetDateTime(1),
                                    Status = reader.GetString(2),
                                    ClientPhoneNumber = reader.GetString(3),
                                    RackID = reader.GetInt32(4),
                                    CellID = reader.GetInt32(5),
                                };
                                inventoryItems.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении данных инвентаря: " + ex.Message);
            }

            return inventoryItems;
        }

        public int GetNextCellNumber()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT ISNULL(MAX(CellID), 0) + 1 FROM StorageRacks";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }
}
