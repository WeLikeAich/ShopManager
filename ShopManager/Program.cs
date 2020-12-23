using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShopManager.Clients;
using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace ShopManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PreCheck();

            Console.WriteLine("Welcome to Shop Manager");
            bool cont = true;
            do
            {
                Menu();
                Int32.TryParse(Console.ReadLine(), out int input);
                if (input == 0)
                {
                    break;
                }
                IClient client = ClientFactory.GenorateClient(input);

                if (client != null)
                {
                    client.Run();
                }
            }
            while (cont);
        }

        private static void PreCheck()
        {
            Console.WriteLine("Loading...");
            using (var db = new ShopContext())
            {
                db.Database.Migrate();
            }
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo newCulture = new CultureInfo(currentCulture.Name);
            newCulture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        private static void Menu()
        {
            Console.WriteLine("Pick a number for your action");
            Console.WriteLine("1> Items");
            Console.WriteLine("2> Materials");
            Console.WriteLine("3> Analysis");
            Console.WriteLine("0> Exit");
        }
    }
}