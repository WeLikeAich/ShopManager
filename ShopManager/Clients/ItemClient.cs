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
            CommonClientFunctions.Menuing(MenuOptionsItem(), (actionInput) =>
            {
                switch (actionInput)
                {
                    case 1:
                        CreateItem();
                        break;

                    case 2:
                        CommonClientFunctions.ListItems();
                        break;

                    case 3:
                        UpdateItem();
                        break;

                    case 4:
                        DeleteItem();
                        break;
                }
            });
        }

        private void CreateItem()
        {
            List<Material> materials = CommonClientFunctions.ReadMaterials();
            if (materials.Count == 0)
            {
                Console.WriteLine("There must be materials created prior to making an item");
                return;
            }
            Console.Write("Enter Item Name> ");
            string name = Console.ReadLine();

            Console.Write("Enter Item Description> ");
            string description = Console.ReadLine();

            List<SizeOption> sizeOptions = new List<SizeOption>();

            Console.WriteLine("Enter Sizing Information.");
            do
            {
                var sizeOption = CreateSizeOption(materials);
                if (sizeOption is null)
                    return;
                sizeOptions.Add(sizeOption);

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

        private void UpdateItem()
        {
            Console.WriteLine("Pick an Item to Edit");
            var items = CommonClientFunctions.ReadItems();
            var hasElements = CommonClientFunctions.EntitySelection(items, out Item item);
            if (!hasElements)
            {
                Console.WriteLine("No Items Available");
                return;
            }
            if (item is null)
                return;

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

            CommonClientFunctions.Menuing(MenuOptionsUpdateItemSizeOption(), (actionInput) =>
            {
                switch (actionInput)
                {
                    case 1:
                        CreateSizeOption(item);
                        break;

                    case 2:
                        UpdateSizeOption(item);
                        break;

                    case 3:
                        DeleteSizeOption(item.SizeOptions);
                        break;
                }
            });
        }

        private void CreateSizeOption(Item item)
        {
            List<Material> materials = CommonClientFunctions.ReadMaterials();
            var sizeOption = CreateSizeOption(materials);
            sizeOption.ItemId = item.Id;
            item.SizeOptions.Add(sizeOption);
            using var db = new ShopContext();
            new DataService(db).Create(sizeOption);
        }

        public void UpdateSizeOption(Item item)
        {
            var options = item.SizeOptions;
            var hasElements = CommonClientFunctions.EntitySelection(options, out SizeOption option, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Size Options Available");
                return;
            }
            if (option is null)
                return;

            Console.WriteLine($"Size ({option.Size})> ");
            var newSize = Console.ReadLine();

            decimal newPrice;
            string priceInput;
            do
            {
                newPrice = -1;
                Console.WriteLine($"Price ({option.Price})> ");
                priceInput = Console.ReadLine().Replace("$", "");

                if (String.IsNullOrWhiteSpace(priceInput))
                    break;
                Decimal.TryParse(priceInput, out newPrice);
            } while (newPrice <= 0);

            int newTTM;
            string timeInput;
            do
            {
                newTTM = -1;
                Console.WriteLine($"Time To Make In Hours. Must be Non Zero. ({option.TimeToMakeInHours})> ");
                timeInput = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(timeInput))
                    break;
                Int32.TryParse(timeInput, out newTTM);
            } while (newTTM <= 0);

            option.Size = String.IsNullOrWhiteSpace(newSize) ? option.Size : newSize;
            option.Price = newPrice == -1 ? option.Price : newPrice;
            option.TimeToMakeInHours = newTTM == -1 ? option.TimeToMakeInHours : newTTM;

            using (var db = new ShopContext())
            {
                new DataService(db).Update(option);
            }

            CommonClientFunctions.Menuing(MenuOptionsUpdateSizeOptionMaterialCount(), (actionInput) =>
            {
                switch (actionInput)
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
                }
            });
        }

        private void CreateMaterialCount(SizeOption option)
        {
            List<Material> materials = CommonClientFunctions.ReadMaterials();

            var materialCount = CreateMaterialCount(materials);
            if (materialCount is null)
                return;

            materialCount.SizeOptionId = option.Id;
            option.MaterialCounts.Add(materialCount);
            using var db = new ShopContext();
            new DataService(db).Create(materialCount);
        }

        private void EditMaterialCount(SizeOption option)
        {
            var materialCounts = option.MaterialCounts;

            var hasElements = CommonClientFunctions.EntitySelection(materialCounts, out MaterialCount materialCount, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Material Counts Available");
                return;
            }
            if (materialCount is null)
                return;

            Console.WriteLine($"Count ({materialCount.MaterialUnitCount})> ");
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
            var hasElements = CommonClientFunctions.EntitySelection(materialCounts, out MaterialCount materialCount, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Material Counts Available");
                return;
            }
            if (materialCount is null)
                return;

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(materialCount);
            }
        }

        private void DeleteSizeOption(List<SizeOption> options)
        {
            var hasElements = CommonClientFunctions.EntitySelection(options, out SizeOption option, 1);
            if (!hasElements)
            {
                Console.WriteLine("No Size Options Available");
                return;
            }
            if (option is null)
                return;

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(option);
            }
        }

        private void DeleteItem()
        {
            Console.WriteLine("Pick an Item to Delete");
            var items = CommonClientFunctions.ReadItems();

            var hasElements = CommonClientFunctions.EntitySelection(items, out Item item);
            if (!hasElements)
            {
                Console.WriteLine("No Items Available");
                return;
            }
            if (item is null)
                return;

            using (var db = new ShopContext())
            {
                new DataService(db).Delete(item);
            }
        }

        private SizeOption CreateSizeOption(List<Material> materials)
        {
            Console.Write("Enter Sizing (Dimensions: \"18x24\") or (Size: \"Medium\")> ");
            string size = Console.ReadLine();

            decimal price;
            string priceInput;
            do
            {
                Console.Write("Enter Sizing Price> ");
                priceInput = Console.ReadLine().Replace("$", "");
                Decimal.TryParse(priceInput, out price);
            } while (price <= 0);

            int time;
            do
            {
                Console.Write("Enter Time To Make rounded up to the nearest non zero hour (2 hours and 15 minutes is \"3\" hours)> ");
                Int32.TryParse(Console.ReadLine(), out time);
            } while (time <= 0);

            List<MaterialCount> materialCounts = new List<MaterialCount>();
            do
            {
                var materialCount = CreateMaterialCount(materials);
                if (materialCount is null)
                    return null;
                materialCounts.Add(materialCount);

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
            var hasElements = CommonClientFunctions.EntitySelection(materials, out Material material);
            if (!hasElements)
            {
                Console.WriteLine("No Items Available");
                return null;
            }
            if (material is null)
                return null;

            int newUnitCount;
            do
            {
                Console.Write("Enter the number of units of material needed as a whole number> ");
                Int32.TryParse(Console.ReadLine(), out newUnitCount);
            } while (newUnitCount <= 0);
            return new MaterialCount()
            {
                MaterialId = material.Id,
                MaterialUnitCount = newUnitCount
            };
        }

        private IEnumerable<string> MenuOptionsItem()
        {
            yield return "Create an Item";
            yield return "List Items";
            yield return "Update Items";
            yield return "Delete Items";
            yield return "Main Menu";
        }

        private IEnumerable<string> MenuOptionsUpdateItemSizeOption()
        {
            yield return "Add Sizing Option";
            yield return "Edit Sizing Option";
            yield return "Delete Sizing Option";
            yield return "Finish";
        }

        private IEnumerable<string> MenuOptionsUpdateSizeOptionMaterialCount()
        {
            yield return "Add New Material Count";
            yield return "Edit Material Count";
            yield return "Delete Material Count";
            yield return "Finish";
        }
    }
}