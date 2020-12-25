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
        internal static void ListItems()
        {
            var items = ReadItems();
            if (items.Count == 0)
                Console.WriteLine("No Items Available");
            else
                PrintEntities(items);
        }

        internal static void ListMaterials()
        {
            var materials = ReadMaterials();
            if (materials.Count == 0)
                Console.WriteLine("No Materials Available");
            else
                PrintEntities(materials);
        }

        internal static void PrintEntities<TEntity>(List<TEntity> entities, int tabCount = 0)
        {
            string tabs = new string('\t', tabCount);
            for (int i = 1; i <= entities.Count; i++)
            {
                Console.WriteLine($"{tabs}{i}) {entities[i - 1]}");
            }
        }

        internal static List<Item> ReadItems()
        {
            using var db = new ShopContext();
            return new DataService(db).GetItems();
        }

        internal static List<Material> ReadMaterials()
        {
            using var db = new ShopContext();
            return new DataService(db).GetMaterials();
        }

        internal static List<Color> ReadColorsByMaterialId(Guid id)
        {
            using var db = new ShopContext();
            return new DataService(db).GetColorsByMaterialId(id);
        }

        internal static string ConvertToMoney(decimal amount)
        {
            return amount.ToString("C", CultureInfo.CurrentCulture);
        }

        internal static void Menuing(IEnumerable<string> menuOptions, Action<int> menuActions)
        {
            bool cont = true;
            do
            {
                var exitOption = PrintMenu(menuOptions);
                Int32.TryParse(Console.ReadLine(), out int input);

                if (input == exitOption)
                {
                    cont = false;
                }
                else if (input > 0 && input < exitOption)
                {
                    menuActions(input);
                }
            } while (cont);
        }

        private static int PrintMenu(IEnumerable<string> options)
        {
            Console.WriteLine("\nPick a number for your action");
            int i = 1;
            foreach (var option in options)
            {
                Console.WriteLine($"{i}> {option}");
                i++;
            }
            return i - 1;
        }

        internal static bool EntitySelection<TEntity>(List<TEntity> entities, out TEntity entity, int tabCount = 0) where TEntity : Entity
        {
            if (entities.Count == 0)
            {
                entity = null;
                return false;
            }
            do
            {
                CommonClientFunctions.PrintEntities(entities, tabCount);
                Console.WriteLine($"{entities.Count + 1}) Cancel");
                var success = Int32.TryParse(Console.ReadLine(), out int input);

                if (input > 0 && input <= entities.Count && success)
                {
                    entity = entities[input - 1];
                    return true;
                }
                else if (input == entities.Count + 1)
                {
                    entity = null;
                    return true;
                }
            } while (true);
        }

        internal static List<SizeOption> ReadSizeOptionsByItemId(Guid id)
        {
            using var db = new ShopContext();
            return new DataService(db).GetSizeOptionsByItemId(id);
        }

        internal static List<MaterialCount> ReadMaterialCountsBySizeOptionId(Guid id)
        {
            using var db = new ShopContext();
            return new DataService(db).GetMaterialCountsBySizeOptionId(id);
        }
    }
}