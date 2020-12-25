using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShopManager.Clients;
using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ShopManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PreCheck();

            Console.WriteLine("Welcome to Shop Manager");
            CommonClientFunctions.Menuing(MainMenuOptions(), (actionInput) =>
            {
                IClient client = ClientFactory.GenorateClient(actionInput);

                if (client != null)
                {
                    client.Run();
                }
            });
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

        private static IEnumerable<string> MainMenuOptions()
        {
            yield return "Items";
            yield return "Materials";
            yield return "Analysis";
            yield return "Exit";
        }
    }
}