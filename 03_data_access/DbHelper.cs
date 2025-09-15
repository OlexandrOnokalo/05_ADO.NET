using System.Data;
using System.Data.SqlClient;

namespace _02_CRUD_Interface
{
    public class DbHelper
    {
        public string ConnectionString { get; set; }

        public DbHelper(string cs)
        {
            this.ConnectionString = cs;
        }

        public DbHelper()
        {
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        public DataTable GetProducts()
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM Products", conn);
            var dt = new DataTable();
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        public DataTable GetEmployees()
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM Employees", conn);
            var dt = new DataTable();
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        public DataTable GetClients()
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM Clients", conn);
            var dt = new DataTable();
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        public DataTable GetSalles()
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("SELECT * FROM Salles", conn);
            var dt = new DataTable();
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }
    }
}
