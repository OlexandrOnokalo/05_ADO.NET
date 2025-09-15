using _02_CRUD_Interface;
using System.Data;
using System.Text;


namespace _03_data_access.Models
{
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
}
