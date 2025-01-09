using Microsoft.Data.SqlClient;
using SingletonPattern;
using System.Data.Common;


DatabaseConnection databaseConnection = DatabaseConnection.GetInstance();

//we can create one more than connection with one instance
using (var connection1 = databaseConnection.GetConnection())
using (var connection2 = databaseConnection.GetConnection())
{
    var command1 = new SqlCommand("SELECT GETDATE()", connection1);
    var command2 = new SqlCommand("SELECT GETDATE()", connection2);

    Console.WriteLine("Connection 1: " + command1.ExecuteScalar());
    Console.WriteLine("Connection 2: " + command2.ExecuteScalar());
}

databaseConnection.CloseConnection();
