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
            CommonClientFunctions.Menuing(MenuOptionsMaterial(), (actionInput) =>
            {
                switch (actionInput)
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
                }
            });
        }

        private void CreateMaterial()
        {
            Console.Write("Enter The Full Material Name (as would be listed in a catalog)> ");
            string newFullName = Console.ReadLine();

            Console.Write("Enter A Short Friendly Name for the material> ");
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
            Console.WriteLine("Pick a Material to Edit");
            var materials = CommonClientFunctions.ReadMaterials();
            var hasElements = CommonClientFunctions.EntitySelection(materials, out Material material);
            if (!hasElements)
            {
                Console.WriteLine("No Materials Available");
                return;
            }
            if (material is null)
                return;

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

            CommonClientFunctions.Menuing(MenuOptionsUpdateMaterials(), (actionInput) =>
            {
                switch (actionInput)
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
                }
            });
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
            Console.WriteLine("Pick a Material to Delete");
            var materials = CommonClientFunctions.ReadMaterials();
            var hasElements = CommonClientFunctions.EntitySelection(materials, out Material material);
            if (!hasElements)
            {
                Console.WriteLine("No Materials Available");
                return;
            }
            if (material is null)
                return;

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(material);
            }
        }

        private void DeleteColor(List<Color> colors)
        {
            var hasElements = CommonClientFunctions.EntitySelection(colors, out Color color, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Colors Available");
                return;
            }
            if (color is null)
                return;
            using (var db = new ShopContext())
            {
                new DataService(db).Delete(color);
            }
        }

        private void UpdateColors(Material material)
        {
            var colors = material.Colors;

            var hasElements = CommonClientFunctions.EntitySelection(colors, out Color color, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Colors Available");
                return;
            }
            if (color is null)
                return;

            Console.Write($"Name ({color.Name})> ");
            var newColorName = Console.ReadLine();

            Console.Write($"Color Code ({color.ColorCode})> ");
            var newColorCode = Console.ReadLine();

            Console.Write($"Cost ({color.Cost})> ");
            var pInput = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(pInput))
                pInput = "-1";
            Int32.TryParse(pInput, out int newCost);

            color.Name = String.IsNullOrWhiteSpace(newColorName) ? color.Name : newColorName;
            color.ColorCode = String.IsNullOrWhiteSpace(newColorCode) ? color.ColorCode : newColorCode;
            color.Cost = newCost == -1 ? color.Cost : newCost;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(color);
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

        private IEnumerable<string> MenuOptionsMaterial()
        {
            yield return "Create Material";
            yield return "List Materials";
            yield return "Update Material";
            yield return "Delete Material";
            yield return "Main Menu";
        }

        private IEnumerable<string> MenuOptionsUpdateMaterials()
        {
            yield return "Add Color Option";
            yield return "Edit Color Option";
            yield return "Delete Color Option";
            yield return "Finish";
        }
    }
}