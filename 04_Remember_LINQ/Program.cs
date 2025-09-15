namespace _04_Remember_LINQ
{
//    Тема: LINQ To Objects
//Завдання 1:
//Дано цілочисельну послідовність.Витягти з неї всі позитивні числа, відсортувавши їх по зростанню.
//Завдання 2:
//Дано колекцію цілих чисел.Знайти кількість позитивних двозначних елементів, а також їх середнє арифметичне.
//Завдання 3:
//Дано цілочисельну колекцію, яка зберігає список років. Витягти з неї всі високосні роки, відсортувавши їх по зростанню.
//Завдання 4:
//Дано колекцію цілих чисел. Знайти максимальне парне значення.
//Завдання 5:
//Дано колекцію непустих рядків. Отримати колекцію рядків, додавши вкінець до кожної три знаки оклику.
//Завдання 6:
//Дано певний символ і строкова колекція. Отримати колекцію строк, які мають відповідний символ.
//Завдання 7:
//Дано колекцію непустих рядків. Згрупувати всі елементи по кількості символів.

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#1");
            List<int> numbers = new List<int> { 5, -13, 18, -59, 36, 55 };
            var positiveNum = from n in numbers 
                              where n > 0 
                              orderby n ascending
                              select n;

            foreach (var num in positiveNum)
            {
                Console.WriteLine(num);
            }
            Console.WriteLine();




            Console.WriteLine("#2");

            var countN = (from n in numbers
                          where n > 0 && n > 9 && n < 100
                          select n).Count();
            var avrg = (from n in numbers
                       where n > 0 && n > 9 && n < 100
                       select n).Average();

            Console.WriteLine($"Count = {countN}, average = {avrg}");
            Console.WriteLine();


            Console.WriteLine("#3");
            List<int> years = new List<int> { 1995, 2000, 2005, 2010, 2015, 2020, 2021, 2022, 2023, 2024 };

            var years2 = from n in years
                         where n % 4 == 0 && n % 100 != 0
                         orderby n ascending
                         select n;
            foreach (var num in years2)
            {
                Console.WriteLine(num);
            }
            Console.WriteLine();

            Console.WriteLine("#4");

            var parne = (from n in numbers
                         where n%2==0
                         select n).Max();
            Console.WriteLine($"Parne = {parne}");
            Console.WriteLine();

            Console.WriteLine("#5");

            List<string> strings = new List<string>() { "helloc", "world", "C#", "c++", "python" };
            var modifiedStrings = "!!!";
            var ras = strings.Select(word => word += modifiedStrings);
            foreach (var s in ras)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();

            Console.WriteLine("#6");
            var symb = "c";

            var result = strings.Where(word => word.Contains(symb)).ToList();

            foreach (var word in result)
            {
                Console.WriteLine(word);
            }

            Console.WriteLine();

            Console.WriteLine("#7");

            

            var grouped = strings
                 .GroupBy(word => word.Length);

            foreach (var group in grouped) 
            { 
                Console.WriteLine($"{group.Key}");
                foreach (var s in group) Console.WriteLine(s);            }



        }
    }
}
