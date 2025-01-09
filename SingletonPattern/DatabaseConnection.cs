using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonPattern
{
    public class DatabaseConnection : IDisposable
    {
        //Lazy Initialization ensures that the object is created only when it is needed, 
        //improving performance and reducing memory usage by delaying the instantiation.
        private static readonly Lazy<DatabaseConnection> instance = new Lazy<DatabaseConnection>(() => new DatabaseConnection());

        private SqlConnection sqlConnection;

        private string connectionString = "Server=localhost; Database=TestDB; User Id=sa; Password=YourPassword; Connection Timeout=30;";

        public static DatabaseConnection GetInstance()
        {
            return instance.Value;
        }

        private DatabaseConnection()
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                Console.WriteLine("Database connection is successfull!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection error :  " + ex.Message);
            }
        }

        //Connection Pooling improves performance and prevents resource exhaustion 
        //by reusing existing database connections instead of opening a new one for each request.

        // The SQL Health Check method verifies the connectivity and health of the database server.
        // It attempts to establish a connection to the SQL database and checks for successful communication.

        int attempt = 0;
        int maxRetryCount = 3;
        public SqlConnection GetConnection()
        {
            SqlConnection connection = null;

            while (attempt < maxRetryCount)
            {
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        throw new Exception("The database connection did not open.");
                    }

                    Console.WriteLine("Database connection successful!");
                    return connection;
                }
                catch (Exception ex)
                {
                    attempt++;
                    Console.WriteLine("Connection error : " + ex.Message + ". Trying again..");
                    System.Threading.Thread.Sleep(1000);
                }
            }

            throw new Exception("Database connection failed.");
        }

        public void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
                Console.WriteLine("Database connection is closed.");
            }
        }


        // The IDisposable interface is implemented to allow for proper resource management.
        // It provides a mechanism for releasing unmanaged resources (e.g., file handles, database connections)
        // when they are no longer needed, ensuring that resources are cleaned up promptly to prevent memory leaks.

        public void Dispose()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
                Console.WriteLine("Database connection is closed and disposed.");
            }
        }
    }
}
