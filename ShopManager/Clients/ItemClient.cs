using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopManager.Clients
{
    public class ItemClient : IClient
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
                        CreateItem();
                        break;

                    case 2:
                        ListItems();
                        break;

                    case 3:
                        UpdateItem();
                        break;

                    case 4:
                        DeleteItem();
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
            Console.WriteLine("1> Create an Item");
            Console.WriteLine("2> List Items");
            Console.WriteLine("3> Update Item");
            Console.WriteLine("4> Delete Item");
            Console.WriteLine("0> Finish");
        }

        private void CreateItem()
        {
            Console.Write("Enter Item Name> ");
            string name = Console.ReadLine();

            Console.Write("Enter Item Description> ");
            string description = Console.ReadLine();

            List<SizeOption> sizeOptions = new List<SizeOption>();

            Console.WriteLine("Enter Sizing Information.");
            do
            {
                sizeOptions.Add(CreateSizeOption());

                Console.Write("Enter another sizing option? (\"yes\" or \"no\")> ");
                string cont = Console.ReadLine();

                if (!cont.ToLowerInvariant().Equals("yes"))
                    break;
            } while (true);

            Item newItem = new Item()
            {
                Name = name,
                Description = description,
                SizeOptions = sizeOptions
            };

            using (var db = new ShopContext())
            {
                new DataService(db).Create(newItem);
            }
        }

        private void ListItems()
        {
            PrintItems(ReadItems());
        }

        private void UpdateItem()
        {
            Console.WriteLine("Pick an Item to Edit");
            var items = ReadItems();
            PrintItems(items);
            Int32.TryParse(Console.ReadLine(), out int input);

            var item = items[input - 1];
            Console.WriteLine("Update Item:");
            Console.WriteLine($"Name ({item.Name})> ");
            var newName = Console.ReadLine();
            Console.WriteLine($"Name ({item.Description})> ");
            var newDescription = Console.ReadLine();

            item.Name = String.IsNullOrWhiteSpace(newName) ? item.Name : newName;
            item.Description = String.IsNullOrWhiteSpace(newDescription) ? item.Description : newDescription;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(item);
            }

            bool cont = true;
            do
            {
                Console.WriteLine("1> Add Sizing Option");
                Console.WriteLine("2> Edit Sizing Option");
                Console.WriteLine("3> Delete Sizing Option");
                Console.WriteLine("0> Finish");

                Int32.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        CreateSizeOption(item);
                        break;

                    case 2:
                        EditSizeOption(item);
                        break;

                    case 3:
                        DeleteSizeOption(item.SizeOptions);
                        break;

                    case 0:
                        cont = false;
                        break;
                }
            } while (cont);
        }

        private void CreateSizeOption(Item item)
        {
            var sizeOption = CreateSizeOption();
            sizeOption.ItemId = item.Id;
            item.SizeOptions.Add(sizeOption);
            using var db = new ShopContext();
            new DataService(db).Create(sizeOption);
        }

        public void EditSizeOption(Item item)
        {
            int i = 0;
            var options = item.SizeOptions;
            foreach (var sizeOption in options)
            {
                Console.WriteLine($"\t{i + 1}) {sizeOption}");
                i++;
            }

            Int32.TryParse(Console.ReadLine(), out int input);
            input -= 1;
            var option = options[input];

            Console.WriteLine($"Size ({option.Size})> ");
            var newSize = Console.ReadLine();

            Console.WriteLine($"Price ({option.Price})> ");
            var pInput = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(pInput))
                pInput = "-1";
            Decimal.TryParse(pInput, out decimal newPrice);

            Console.WriteLine($"Time To Make In Hours ({option.TimeToMakeInHours})> ");
            pInput = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(pInput))
                pInput = "-1";
            Int32.TryParse(pInput, out int newTTM);

            option.Size = String.IsNullOrWhiteSpace(newSize) ? option.Size : newSize;
            option.Price = newPrice == -1 ? option.Price : newPrice;
            option.TimeToMakeInHours = newTTM == -1 ? option.TimeToMakeInHours : newTTM;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(option);
            }

            bool cont = true;
            do
            {
                Console.WriteLine("1> Add New Material Count");
                Console.WriteLine("2> Edit Material Count");
                Console.WriteLine("3> Delete Material Count");
                Console.WriteLine("0> Finish");

                Int32.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        CreateMaterialCount(option);
                        break;

                    case 2:
                        EditMaterialCount(option);
                        break;

                    case 3:
                        DeleteMaterialCount(option.MaterialCounts);
                        break;

                    case 0:
                        cont = false;
                        break;
                }
            } while (cont);
        }

        private void CreateMaterialCount(SizeOption option)
        {
            List<Material> materials = ReadMaterials();

            var materialCount = CreateMaterialCount(materials);
            materialCount.SizeOptionId = option.Id;
            option.MaterialCounts.Add(materialCount);
            using var db = new ShopContext();
            new DataService(db).Create(materialCount);
        }

        private void EditMaterialCount(SizeOption option)
        {
            int i = 0;
            var materialCounts = option.MaterialCounts;
            foreach (var mc in materialCounts)
            {
                Console.WriteLine($"\t{i + 1}) {mc}");
                i++;
            }

            Int32.TryParse(Console.ReadLine(), out int input);
            input -= 1;

            var materialCount = materialCounts[input];

            Console.WriteLine($"Cost ({materialCount.MaterialUnitCount})> ");
            var pInput = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(pInput))
                pInput = "-1";
            Int32.TryParse(pInput, out int newCount);

            materialCount.MaterialUnitCount = newCount == -1 ? materialCount.MaterialUnitCount : newCount;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(materialCount);
            }
        }

        private void DeleteMaterialCount(List<MaterialCount> materialCounts)
        {
            for (int i = 0; i < materialCounts.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}){materialCounts[i]}");
                i++;
            }
            Console.WriteLine($"\t{0}) Cancel");

            Int32.TryParse(Console.ReadLine(), out int input);

            if (input == 0)
                return;
            using (var db = new ShopContext())
            {
                new DataService(db).Delete(materialCounts[input - 1]);
            }
        }

        private void DeleteSizeOption(List<SizeOption> sizeOptions)
        {
            for (int i = 0; i < sizeOptions.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}){sizeOptions[i]}");
            }
            Console.WriteLine($"\t{0}) Cancel");

            Int32.TryParse(Console.ReadLine(), out int input);

            if (input == 0)
                return;
            using (var db = new ShopContext())
            {
                new DataService(db).Delete(sizeOptions[input - 1]);
            }
        }

        private void DeleteItem()
        {
            Console.WriteLine("Pick an Item to Delete");
            var items = ReadItems();
            PrintItems(items);
            Int32.TryParse(Console.ReadLine(), out int input);

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(items[input - 1]);
            }
        }

        private SizeOption CreateSizeOption()
        {
            Console.Write("Enter Sizing (Dimensions: 18\"x24\") or (Size: Medium)> ");
            string size = Console.ReadLine();

            Console.Write("Enter Sizing Price without the \"$\"> ");
            Decimal.TryParse(Console.ReadLine(), out decimal price);

            Console.Write("Enter Time To Make rounded up to the nearest hour (2 hours and 15 minutes is \"3\" hours)> ");
            Int32.TryParse(Console.ReadLine(), out int time);

            List<MaterialCount> materialCounts = new List<MaterialCount>();
            List<Material> materials = ReadMaterials();
            do
            {
                materialCounts.Add(CreateMaterialCount(materials));

                Console.Write("Enter another material count? (\"yes\" or \"no\")> ");
                string cont = Console.ReadLine();

                if (!cont.ToLowerInvariant().Equals("yes"))
                    break;
            } while (true);

            return new SizeOption()
            {
                Size = size,
                Price = price,
                TimeToMakeInHours = time,
                MaterialCounts = materialCounts
            };
        }

        private MaterialCount CreateMaterialCount(List<Material> materials)
        {
            Console.WriteLine("Pick a material> ");
            PrintMaterials(materials);
            Int32.TryParse(Console.ReadLine(), out int input);

            Material material = materials[input - 1];
            Console.Write("Enter the number of units of material needed as a whole number> ");
            Int32.TryParse(Console.ReadLine(), out int newUnitCount);

            return new MaterialCount()
            {
                MaterialId = material.Id,
                MaterialUnitCount = newUnitCount
            };
        }

        private void PrintMaterials(List<Material> materials)
        {
            for (int i = 1; i <= materials.Count; i++)
            {
                Console.WriteLine($"{i}) {materials[i - 1]}");
            }
        }

        private List<Material> ReadMaterials()
        {
            using var db = new ShopContext();
            return new DataService(db).GetMaterials();
        }

        private void PrintItems(List<Item> items)
        {
            for (int i = 1; i <= items.Count; i++)
            {
                Console.WriteLine($"{i}) {items[i - 1]}");
            }
        }

        private List<Item> ReadItems()
        {
            using var db = new ShopContext();
            return new DataService(db).GetItems();
        }
    }
}