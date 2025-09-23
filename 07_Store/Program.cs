using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _07_Store
{

    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new StoreDbContext();
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("test");


        }
    }
}
