using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ShopManager.Clients
{
    internal static class CommonClientFunctions
    {
        internal static void ListItems() => PrintItems(ReadItems());

        internal static void ListMaterials() => PrintMaterials(ReadMaterials());

        internal static void PrintItems(List<Item> items)
        {
            for (int i = 1; i <= items.Count; i++)
            {
                Console.WriteLine($"{i}) {items[i - 1]}");
            }
        }

        internal static List<Item> ReadItems()
        {
            using var db = new ShopContext();
            return new DataService(db).GetItems();
        }

        internal static void PrintMaterials(List<Material> materials)
        {
            for (int i = 1; i <= materials.Count; i++)
            {
                Console.WriteLine($"{i}) {materials[i - 1]}");
            }
        }

        internal static List<Material> ReadMaterials()
        {
            using var db = new ShopContext();
            return new DataService(db).GetMaterials();
        }

        internal static void PrintSizeOption(List<SizeOption> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}) {options[i]}");
            }
        }

        internal static void PrintMaterialCounts(List<MaterialCount> materialCounts)
        {
            for (int i = 0; i < materialCounts.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}) {materialCounts[i]}");
            }
        }

        internal static string ConvertToMoney(decimal amount)
        {
            return amount.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}