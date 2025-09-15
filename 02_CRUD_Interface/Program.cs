using _03_data_access.Models;
using System.Globalization;
using System.Text;


namespace _02_CRUD_Interface
{

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
