using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShopManager.Clients;
using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;

namespace ShopManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new ShopContext())
            {
                PreCheck(db.Database);
            }

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

        private static void PreCheck(DatabaseFacade database)
        {
            Console.WriteLine("Loading...");
            database.Migrate();
        }

        private static void Menu()
        {
            Console.WriteLine("Pick a number for your action");
            Console.WriteLine("1> Items");
            Console.WriteLine("2> Materials");
            Console.WriteLine("3> Create a quote/invoice");
            Console.WriteLine("4> Run Profit Margin Analysis");
            Console.WriteLine("0> Exit");
        }
    }
}