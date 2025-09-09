using Microsoft.Data.SqlClient;
using System.Text;

namespace _01_ADO.NET_Intro_Connected_Mode
{
    internal class Program
    {
        //#1 Відобразити інформацію про всіх покупців
        static void ShowAllClients(SqlConnection connection)
        {
            string cmdText = $@"select * from Clients;";
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],10} ");
                Console.WriteLine();
            }

            reader.Close();
        }

        //#2 Відобразити інформацію про всіх продавців
        static void ShowAllEmployees(SqlConnection connection)
        {
            string cmdText = $@"select * from Employees;";
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],10} ");
                Console.WriteLine();
            }

            reader.Close();
        }

        //#3 Відобразити інформацію про продажі, які виконав певний продавець по імені та прізвищу
        static void ShowSalesByEmployee(SqlConnection connection, string employeeFullName)
        {
            string cmdText = $@"
                select *
                from Salles s
                join Products p on s.ProductId = p.Id
                join Employees e on s.EmployeeId = e.Id
                join Clients c on s.ClientId = c.Id
                where e.FullName = '{employeeFullName}';
            ";
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"\nПродажі продавця: {employeeFullName}");

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],10} ");
                Console.WriteLine();
            }

            reader.Close();
        }

        //#4 Відобразити інформацію про продажі на суму більше зазначеної
        static void ShowSalesAboveAmount(SqlConnection connection, decimal amount)
        {
            string cmdText = $@"
                select *
                from Salles s
                join Products p on s.ProductId = p.Id
                join Employees e on s.EmployeeId = e.Id
                join Clients c on s.ClientId = c.Id
                where (s.Price * s.Quantity) > {amount};
            ";
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"\nПродажі з сумою більше {amount}");

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],10} ");
                Console.WriteLine();
            }

            reader.Close();
        }

        //#5 Показати найдорожчу та найдешевшу покупку певного покупця по імені та прізвищу
        static void ShowMinMaxPurchaseByClient(SqlConnection connection, string clientFullName)
        {
            string minCmdText = $@"
                select top 1 *
                from Salles
                where ClientId = (select Id from Clients where FullName = '{clientFullName}')
                order by (Price * Quantity) asc;
            ";
            SqlCommand minCmd = new SqlCommand(minCmdText, connection);
            SqlDataReader minReader = minCmd.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"\nНайдешевша покупка клієнта: {clientFullName}");
            for (int i = 0; i < minReader.FieldCount; i++)
                Console.Write($" {minReader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (minReader.Read())
            {
                for (int i = 0; i < minReader.FieldCount; i++)
                    Console.Write($" {minReader[i],10} ");
                Console.WriteLine();
            }
            minReader.Close();

            string maxCmdText = $@"
                select top 1 *
                from Salles
                where ClientId = (select Id from Clients where FullName = '{clientFullName}')
                order by (Price * Quantity) desc;
            ";
            SqlCommand maxCmd = new SqlCommand(maxCmdText, connection);
            SqlDataReader maxReader = maxCmd.ExecuteReader();

            Console.WriteLine($"\nНайдорожча покупка клієнта: {clientFullName}");
            for (int i = 0; i < maxReader.FieldCount; i++)
                Console.Write($" {maxReader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (maxReader.Read())
            {
                for (int i = 0; i < maxReader.FieldCount; i++)
                    Console.Write($" {maxReader[i],10} ");
                Console.WriteLine();
            }
            maxReader.Close();
        }

        //#6 Показати найпершу продажу певного продавця по імені та прізвищу
        static void ShowFirstSaleByEmployee(SqlConnection connection, string employeeFullName)
        {
            
            string cmdText = $@"
                select top 1 *
                from Salles s
                join Employees e on s.EmployeeId = e.Id
                where e.FullName = '{employeeFullName}'
                order by s.BuyDate asc, s.Id asc;
            ";
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"\nПерша продажа продавця: {employeeFullName}");

            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($" {reader.GetName(i),10}");
            Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($" {reader[i],10} ");
                Console.WriteLine();
            }

            reader.Close();
        }

        static void Main(string[] args)
        {
            string connectionString = @"Data Source = PULSE\SQLEXPRESS; 
                                        Initial Catalog = SportShop;
                                        Integrated Security = true; TrustServerCertificate=True;
";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Підключення успішне!");

            
            ShowAllClients(sqlConnection);                                              // #1
            ShowAllEmployees(sqlConnection);                                            // #2
            ShowSalesByEmployee(sqlConnection, "Ярощук Іван Петрович");                  // #3
            ShowSalesAboveAmount(sqlConnection, 1000m);                                 // #4
            ShowMinMaxPurchaseByClient(sqlConnection, "Петрук Степан Романович");       // #5
            ShowFirstSaleByEmployee(sqlConnection, "Ярощук Іван Петрович");             // #6

            sqlConnection.Close();
        }
    }
}
