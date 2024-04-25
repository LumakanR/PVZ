using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

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

        public static string AddClient(string clientPhoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
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
                if (dbConnector.IsCellAvailable(order.CellID))
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

                string query = "SELECT Orders.OrderID, Orders.ArrivedDate, Orders.Status, Orders.CellNumber FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryItem item = new InventoryItem
                            {
                                OrderID = reader.GetInt32(0),
                                ArrivedDate = reader.GetDateTime(1),
                                Status = reader.GetString(2),
                                CellNumber = reader.GetInt32(3)
                            };

                            inventoryData.Add(item);
                        }
                    }
                }
            }

            return inventoryData;
        }
        public bool DeliverOrder(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Проверяем, существует ли заказ с указанным номером
                    string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE OrderID = @OrderId";
                    using (SqlCommand checkOrderCommand = new SqlCommand(checkOrderQuery, connection))
                    {
                        checkOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                        int orderCount = Convert.ToInt32(checkOrderCommand.ExecuteScalar());

                        // Если заказ с указанным номером не найден, возвращаем false
                        if (orderCount == 0)
                        {
                            return false;
                        }
                    }

                    // Удаляем заказ из базы данных
                    string deleteOrderQuery = "DELETE FROM Orders WHERE OrderID = @OrderId";
                    using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection))
                    {
                        deleteOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
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

                    string query = "SELECT OrderID, ArrivedDate, Status, CellNumber FROM Orders";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InventoryItem item = new InventoryItem
                                {
                                    OrderID = reader.GetInt32(0),
                                    ArrivedDate = reader.GetDateTime(1),
                                    Status = reader.GetString(2),
                                    CellNumber = reader.GetInt32(3)
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
    }
}