using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PortalVideojuegos
{
    public class DatabaseHelper
    {
        private readonly string connectionString = "Server=localhost;Database=catalogo_videojuegos;Uid=root;Pwd=1234;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public MySqlConnection OpenConnection()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            return conn;
        }

        public void CloseConnection(MySqlConnection conn)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}