using Microsoft.Data.SqlClient;
using System.Text;

namespace SportShopConsole
{
    public class Sale
    {   public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public DateTime BuyDate { get; set; }
    }

    public class SaleInfo
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Price * Quantity;
        public string EmployeeName { get; set; } = "";
        public string ClientName { get; set; } = "";
        public DateTime BuyDate { get; set; }

        public override string ToString()
        {
            return $"{Id}\t{BuyDate:yyyy-MM-dd}\t{ProductName}\t{Quantity}×{Price} = {Total}  \tПродавець: {EmployeeName}\tКлієнт: {ClientName}";
        }
    }

    public class SportShopDb : IDisposable
    {
        private readonly SqlConnection sqlConnection;

        public SportShopDb(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public void AddSale(Sale sale)
        {
            using (var transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    using (var cmdCheck = new SqlCommand("SELECT Quantity FROM Products WHERE Id = @pid", sqlConnection))
                    {
                        cmdCheck.Transaction = transaction;
                        cmdCheck.Parameters.AddWithValue("@pid", sale.ProductId);
                        object obj = cmdCheck.ExecuteScalar();
                        if (obj == null)
                            throw new InvalidOperationException($"Product with Id={sale.ProductId} not found.");
                        int qtyAvailable = Convert.ToInt32(obj);
                        if (qtyAvailable < sale.Quantity)
                            throw new InvalidOperationException($"Not enough quantity for product {sale.ProductId}. Available: {qtyAvailable}, requested: {sale.Quantity}.");
                    }

                    using (var cmdInsert = new SqlCommand(@"
                        INSERT INTO Salles(ProductId, Price, Quantity, EmployeeId, ClientId, BuyDate)
                        VALUES(@pid, @price, @qty, @emp, @cli, @date)", sqlConnection))
                    {
                        cmdInsert.Transaction = transaction;
                        cmdInsert.Parameters.AddWithValue("@pid", sale.ProductId);
                        cmdInsert.Parameters.AddWithValue("@price", sale.Price);
                        cmdInsert.Parameters.AddWithValue("@qty", sale.Quantity);
                        cmdInsert.Parameters.AddWithValue("@emp", sale.EmployeeId);
                        cmdInsert.Parameters.AddWithValue("@cli", sale.ClientId);
                        cmdInsert.Parameters.AddWithValue("@date", sale.BuyDate.Date);
                        cmdInsert.ExecuteNonQuery();
                    }

                    using (var cmdUpdate = new SqlCommand("UPDATE Products SET Quantity = Quantity - @qty WHERE Id = @pid", sqlConnection))
                    {
                        cmdUpdate.Transaction = transaction;
                        cmdUpdate.Parameters.AddWithValue("@qty", sale.Quantity);
                        cmdUpdate.Parameters.AddWithValue("@pid", sale.ProductId);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Console.WriteLine("Добавлено продажу та оновлено кількість товару.");
                }
                catch
                {
                    try { transaction.Rollback(); } catch { }
                    throw;
                }
            }
        }

        public List<SaleInfo> GetSalesByPeriod(DateTime start, DateTime end)
        {
            var result = new List<SaleInfo>();
            string cmdText = @"
                SELECT s.Id, p.Name AS ProductName, s.Quantity, s.Price, e.FullName AS EmployeeName, c.FullName AS ClientName, s.BuyDate
                FROM Salles s
                JOIN Products p ON p.Id = s.ProductId
                JOIN Employees e ON e.Id = s.EmployeeId
                JOIN Clients c ON c.Id = s.ClientId
                WHERE s.BuyDate BETWEEN @start AND @end
                ORDER BY s.BuyDate, s.Id";
            using (var cmd = new SqlCommand(cmdText, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@start", start.Date);
                cmd.Parameters.AddWithValue("@end", end.Date);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var si = new SaleInfo
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                            ClientName = reader.GetString(reader.GetOrdinal("ClientName")),
                            BuyDate = reader.GetDateTime(reader.GetOrdinal("BuyDate"))
                        };
                        result.Add(si);
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string connectionString = @"Data Source=DESKTOP-1LCG8OH\SQLEXPRESS;
                                        Initial Catalog=SportShop;
                                        Integrated Security=True;
                                        TrustServerCertificate=True;";
            try
            {
                using (var db = new SportShopDb(connectionString))
                {
                    var newSale = new Sale
                    {
                        ProductId = 5,
                        Price = 1500m,
                        Quantity = 1,
                        EmployeeId = 1,
                        ClientId = 1,
                        BuyDate = DateTime.Today
                    };
                    try
                    {
                        db.AddSale(newSale);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Помилка при додаванні продажу: " + ex.Message);
                    }
                    DateTime from = new DateTime(2023, 1, 1);
                    DateTime to = DateTime.Today;
                    var sales = db.GetSalesByPeriod(from, to);
                    Console.WriteLine("\nПродажі за період {0:yyyy-MM-dd} — {1:yyyy-MM-dd}:", from, to);
                    foreach (var s in sales)
                    {
                        Console.WriteLine(s.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Критична помилка: " + e.Message);
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
