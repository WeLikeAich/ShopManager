using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopManager.Clients
{
    public class MaterialClient : IClient
    {
        public void Run()
        {
            bool cont = true;
            do
            {
                Menu();
                Int32.TryParse(Console.ReadLine(), out int input);
                switch (input)
                {
                    case 1:
                        CreateMaterial();
                        break;

                    case 2:
                        CommonClientFunctions.ListMaterials();
                        break;

                    case 3:
                        UpdateMaterials();
                        break;

                    case 4:
                        DeleteMaterial();
                        break;

                    case 0:
                        cont = false;
                        break;
                }
            } while (cont);
        }

        private void Menu()
        {
            Console.WriteLine("Pick a number for your action");
            Console.WriteLine("1> Create Material");
            Console.WriteLine("2> List Materials");
            Console.WriteLine("3> Update Material");
            Console.WriteLine("4> Delete Material");

            Console.WriteLine("0> Main Menu");
        }

        private void CreateMaterial()
        {
            Console.Write("Enter The Full Material Name (as would be listed in a catalog)> ");
            string newFullName = Console.ReadLine();

            Console.Write("Enter A Short Friendly Name for the material (used > ");
            string newFriendlyName = Console.ReadLine();

            List<Color> colors = new List<Color>();

            Console.WriteLine("Enter Color Information.");
            do
            {
                colors.Add(GenerateNewColor());

                Console.Write("Enter another color option? (\"yes\" or \"no\")> ");
                string cont = Console.ReadLine();

                if (!cont.ToLowerInvariant().Equals("yes"))
                    break;
            } while (true);

            Material newMaterial = new Material()
            {
                FriendlyName = newFullName,
                FullMaterialName = newFriendlyName,
                Colors = colors
            };

            using (var db = new ShopContext())
            {
                new DataService(db).Create(newMaterial);
            }
        }

        private void UpdateMaterials()
        {
            Console.WriteLine("Pick an Item to Edit");
            var materials = CommonClientFunctions.ReadMaterials();
            CommonClientFunctions.PrintMaterials(materials);
            Int32.TryParse(Console.ReadLine(), out int input);

            Material material = materials[input - 1];

            Console.WriteLine("Update Material:");
            Console.Write($"Name ({material.FullMaterialName})> ");
            var newFullMaterialName = Console.ReadLine();
            Console.Write($"Name ({material.FriendlyName})> ");
            var newFriendlyName = Console.ReadLine();

            material.FullMaterialName = String.IsNullOrWhiteSpace(newFullMaterialName) ? material.FullMaterialName : newFullMaterialName;
            material.FriendlyName = String.IsNullOrWhiteSpace(newFriendlyName) ? material.FriendlyName : newFriendlyName;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(material);
            }

            bool cont = true;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1> Add Color Option");
                Console.WriteLine("2> Edit Color Option");
                Console.WriteLine("3> Delete Color Option");
                Console.WriteLine("0> Back");

                Int32.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        CreateColor(material);
                        break;

                    case 2:
                        UpdateColors(material);
                        break;

                    case 3:
                        DeleteColor(material.Colors);
                        break;

                    case 0:
                        cont = false;
                        break;
                }
            } while (cont);
        }

        private void CreateColor(Material material)
        {
            var color = GenerateNewColor();
            color.MaterialId = material.Id;
            material.Colors.Add(color);
            using (var db = new ShopContext())
            {
                new DataService(db).Create(color);
            }
        }

        private void DeleteMaterial()
        {
            Console.WriteLine("Pick an Item to Delete");
            var materials = CommonClientFunctions.ReadMaterials();
            CommonClientFunctions.PrintMaterials(materials);
            Int32.TryParse(Console.ReadLine(), out int input);

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(materials[input - 1]);
            }
        }

        private void DeleteColor(List<Color> colors)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}){colors[i]}");
                i++;
            }

            Int32.TryParse(Console.ReadLine(), out int input);
            using (var db = new ShopContext())
            {
                new DataService(db).Delete(colors[input - 1]);
            }
        }

        private void UpdateColors(Material material)
        {
            int i = 0;
            var colors = material.Colors;
            foreach (var color in colors)
            {
                Console.WriteLine($"\t{i + 1}) {color}");
                i++;
            }

            int input = -1;

            while (input == -1)
                Int32.TryParse(Console.ReadLine(), out input);
            input -= 1;
            Console.WriteLine($"Name ({colors[input].Name})> ");
            var newColorName = Console.ReadLine();

            Console.WriteLine($"Color Code ({colors[input].ColorCode})> ");
            var newColorCode = Console.ReadLine();

            Console.WriteLine($"Cost ({colors[input].Cost})> ");
            var pInput = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(pInput))
                pInput = "-1";
            Int32.TryParse(pInput, out int newCost);

            colors[input].Name = String.IsNullOrWhiteSpace(newColorName) ? colors[input].Name : newColorName;
            colors[input].ColorCode = String.IsNullOrWhiteSpace(newColorCode) ? colors[input].ColorCode : newColorCode;
            colors[input].Cost = newCost == -1 ? colors[input].Cost : newCost;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(colors[input]);
            }
        }

        private Color GenerateNewColor()
        {
            Console.Write("Enter Color Name> ");
            string newColorName = Console.ReadLine();

            Console.Write("Enter Color Code> ");
            string newColorCode = Console.ReadLine();

            Console.Write("Enter Color Cost> ");
            Decimal.TryParse(Console.ReadLine(), out decimal cost);

            return new Color()
            {
                Name = newColorName,
                ColorCode = newColorCode,
                Cost = cost,
            };
        }
    }
}