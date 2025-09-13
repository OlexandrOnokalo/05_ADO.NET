using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Text;


namespace _02_CRUD_Interface
{
    public class DbHelper
    {
        public string ConnectionString { get; set; }

        public DbHelper(string cs)
        {
            this.ConnectionString = cs;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }
    }

    public class SalesService
    {
        private DbHelper db;

        public SalesService(DbHelper db)
        {
            this.db = db;
        }

        public void AddSale(int productId, decimal price, int quantity, int employeeId, int clientId, DateTime buyDate)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"INSERT INTO Salles(ProductId, Price, Quantity, EmployeeId, ClientId, BuyDate)
                                                VALUES (@productId, @price, @quantity, @employeeId, @clientId, @buyDate)";
                            cmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;
                            cmd.Parameters.Add("@price", SqlDbType.Money).Value = price;
                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
                            cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = employeeId;
                            cmd.Parameters.Add("@clientId", SqlDbType.Int).Value = clientId;
                            cmd.Parameters.Add("@buyDate", SqlDbType.Date).Value = buyDate.Date;
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        public void ShowSalesByPeriod(DateTime from, DateTime to)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        select s.Id, p.Name as ProductName, s.Price, s.Quantity, e.FullName as EmployeeName, c.FullName as ClientName, s.BuyDate
                        from Salles s
                        join Products p on s.ProductId = p.Id
                        join Employees e on s.EmployeeId = e.Id
                        join Clients c on s.ClientId = c.Id
                        where s.BuyDate >= @from and s.BuyDate <= @to
                        order by s.BuyDate";
                    cmd.Parameters.Add("@from", SqlDbType.Date).Value = from.Date;
                    cmd.Parameters.Add("@to", SqlDbType.Date).Value = to.Date;

                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine();
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine($"Продажі за період {from:yyyy-MM-dd} — {to:yyyy-MM-dd}:");
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Немає продажів за вказаний період.");
                            return;
                        }
                        Console.WriteLine("Id | Товар | Ціна | Кількість | Продавець | Клієнт | Дата");
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var prod = reader.GetString(1);
                            var price = reader.GetDecimal(2);
                            var qty = reader.GetInt32(3);
                            var emp = reader.GetString(4);
                            var client = reader.GetString(5);
                            var date = reader.GetDateTime(6).ToString("yyyy-MM-dd");
                            Console.WriteLine($"{id} | {prod} | {price} | {qty} | {emp} | {client} | {date}");
                        }
                    }
                }
            }
        }

        public void ShowLastPurchaseByClient(string firstName, string lastName)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    string pattern1 = $"%{firstName}% {lastName}%";
                    string pattern2 = $"%{lastName}% {firstName}%";
                    cmd.CommandText = @"
                        select top 1 s.Id, p.Name as ProductName, s.Price, s.Quantity, e.FullName as EmployeeName, c.FullName as ClientName, s.BuyDate
                        from Salles s
                        join Clients c on s.ClientId = c.Id
                        join Products p on s.ProductId = p.Id
                        join Employees e on s.EmployeeId = e.Id
                        where c.FullName like @pattern1 or c.FullName like @pattern2
                        order by s.BuyDate desc";
                    cmd.Parameters.Add("@pattern1", SqlDbType.NVarChar, 200).Value = pattern1;
                    cmd.Parameters.Add("@pattern2", SqlDbType.NVarChar, 200).Value = pattern2;

                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        if (!reader.Read())
                        {
                            Console.WriteLine("Покупки для вказаного клієнта не знайдено.");
                            return;
                        }
                        var id = reader.GetInt32(0);
                        var prod = reader.GetString(1);
                        var price = reader.GetDecimal(2);
                        var qty = reader.GetInt32(3);
                        var emp = reader.GetString(4);
                        var client = reader.GetString(5);
                        var date = reader.GetDateTime(6).ToString("yyyy-MM-dd");

                        Console.WriteLine();
                        Console.WriteLine($"Остання покупка клієнта {client}:");
                        Console.WriteLine($"SaleId: {id}, Товар: {prod}, Ціна: {price}, Кількість: {qty}, Продавець: {emp}, Дата: {date}");
                    }
                }
            }
        }

        public void ShowTopSeller()
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        select top 1 e.Id, e.FullName, sum(s.Price * s.Quantity) as TotalSales
                        from Salles s
                        join Employees e on s.EmployeeId = e.Id
                        group by e.Id, e.FullName
                        order by TotalSales desc";
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        if (!reader.Read())
                        {
                            Console.WriteLine("Немає даних про продажі.");
                            return;
                        }
                        var id = reader.GetInt32(0);
                        var fullName = reader.GetString(1);
                        var total = reader.GetDecimal(2);
                        Console.WriteLine($"Продавець з найбільшою сумою продажів: Id={id}, {fullName}, Сума = {total}");
                    }
                }
            }
        }
    }

    public class PersonService
    {
        private DbHelper db;

        public PersonService(DbHelper db)
        {
            this.db = db;
        }

        public bool DeletePerson(bool isEmployee, int id)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                using (var checkCmd = conn.CreateCommand())
                {
                    if (isEmployee)
                    {
                        checkCmd.CommandText = "select count(1) from Salles where EmployeeId = @id";
                    }
                    else
                    {
                        checkCmd.CommandText = "select count(1) from Salles where ClientId = @id";
                    }
                    checkCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var countObj = checkCmd.ExecuteScalar();
                    int count = 0;
                    if (countObj != null)
                    {
                        count = Convert.ToInt32(countObj);
                    }
                    if (count > 0)
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine("Неможливо видалити: на цю особу посилаються записи в Salles.");
                        return false;
                    }
                }

                using (var delCmd = conn.CreateCommand())
                {
                    if (isEmployee)
                    {
                        delCmd.CommandText = "delete from Employees where Id = @id";
                    }
                    else
                    {
                        delCmd.CommandText = "delete from Clients where Id = @id";
                    }
                    delCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var affected = delCmd.ExecuteNonQuery();
                    return affected > 0;
                }
            }
        }
    }

    public class Program
    {
        private const string connectionString = @"Server=PULSE\SQLEXPRESS;Database=SportShop;Trusted_Connection=True;TrustServerCertificate=True;";

        public static void Main(string[] args)
        {


            var db = new DbHelper(connectionString);
            var salesService = new SalesService(db);
            var personService = new PersonService(db);
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Демонстрація: SportShop ADO.NET ===");
            try
            {
                Console.WriteLine();
                Console.WriteLine("1) Додавання нової продажі (інтерактивно)");
                Console.Write("Введіть ProductId: ");
                int productId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Введіть Ціну (наприклад 150.00): ");
                decimal price = decimal.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

                Console.Write("Введіть Кількість: ");
                int qty = int.Parse(Console.ReadLine() ?? "1");

                Console.Write("Введіть Id працівника: ");
                int empId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Введіть Id клієнта: ");
                int clientId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Дата покупки (yyyy-MM-dd), пусто = сьогодні: ");
                var dateInput = Console.ReadLine();
                DateTime buyDate = string.IsNullOrWhiteSpace(dateInput) ? DateTime.Today : DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                salesService.AddSale(productId, price, qty, empId, clientId, buyDate);
                Console.WriteLine("Продаж додано.");
                Console.WriteLine();

                Console.WriteLine("2) Показати всі продажі за період");
                Console.Write("From (yyyy-MM-dd): ");
                DateTime from = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Console.Write("To (yyyy-MM-dd): ");
                DateTime to = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                salesService.ShowSalesByPeriod(from, to);

                Console.WriteLine();
                Console.WriteLine("3) Показати останню покупку певного клієнта (введіть ім'я та прізвище)");
                Console.Write("Ім'я: ");
                var firstName = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Прізвище: ");
                var lastName = Console.ReadLine()?.Trim() ?? "";
                salesService.ShowLastPurchaseByClient(firstName, lastName);

                Console.WriteLine();
                Console.WriteLine("4) Видалити працівника або клієнта по id");
                Console.Write("Видалити працівника? (y = працівник, n = клієнт): ");
                var isEmployeeInput = Console.ReadLine()?.Trim().ToLower() ?? "n";
                bool isEmployee = (isEmployeeInput == "y");
                Console.Write("Введіть id для видалення: ");
                int delId;
                if (int.TryParse(Console.ReadLine(), out delId))
                {
                    var deleted = personService.DeletePerson(isEmployee, delId);
                    Console.WriteLine(deleted ? "Успішно видалено." : "Не вдалося видалити (можливо є посилання в таблиці Salles).");
                }
                else
                {
                    Console.WriteLine("Невірний id, пропускаємо видалення.");
                }

                Console.WriteLine();
                Console.WriteLine("5) Показати продавця, загальна сума продажу якого найбільша");
                salesService.ShowTopSeller();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Готово. Натисніть Enter для виходу.");
            Console.ReadLine();
        }
    }
}
