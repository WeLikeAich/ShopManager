﻿using ShopManager.Entities;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShopManager.Clients
{
    internal class AnalysisClient : IClient
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
                        ItemsReport();
                        break;

                    case 0:
                        cont = false;
                        break;
                }
            } while (cont);
        }

        private void ItemsReport()
        {
            var items = CommonClientFunctions.ReadItems();

            foreach (var item in items)
            {
                Console.WriteLine($"Item: {item.Name}");
                foreach (var sizeOption in item.SizeOptions)
                {
                    Console.WriteLine($"\tSize Option: {sizeOption.Size} - " +
                        $"{sizeOption.Price.ToString("C", CultureInfo.CurrentCulture)} - " +
                        $"{sizeOption.TimeToMakeInHours} hours");

                    //lowest cost materials
                    var minMaterialCost = new List<Tuple<int, decimal>>();
                    var maxMaterialCost = new List<Tuple<int, decimal>>();
                    var avgMaterialCost = new List<Tuple<int, decimal>>();

                    foreach (var materialCount in sizeOption.MaterialCounts)
                    {
                        var materialColorCosts = materialCount.Material.Colors.Select(c => c.Cost);
                        minMaterialCost.Add(new Tuple<int, decimal>(materialCount.MaterialUnitCount, materialColorCosts.Min()));
                        maxMaterialCost.Add(new Tuple<int, decimal>(materialCount.MaterialUnitCount, materialColorCosts.Max()));
                        avgMaterialCost.Add(new Tuple<int, decimal>(materialCount.MaterialUnitCount, materialColorCosts.Average()));
                    }
                    var anserv = new AnalysisService();

                    var highestProfit = anserv.CalculateProfit(sizeOption, minMaterialCost);
                    var lowestProfit = anserv.CalculateProfit(sizeOption, maxMaterialCost);
                    var averageProfit = anserv.CalculateProfit(sizeOption, avgMaterialCost);

                    Console.WriteLine($"\t\tMinimum Profit: {CommonClientFunctions.ConvertToMoney(lowestProfit)} - " +
                        $"Minimum Profit Per Hour: {CommonClientFunctions.ConvertToMoney(anserv.ValuePerHour(sizeOption.TimeToMakeInHours, lowestProfit))}");

                    Console.WriteLine($"\t\tMaximum Profit: {CommonClientFunctions.ConvertToMoney(highestProfit)} - " +
                        $"Maximum Profit Per Hour: {CommonClientFunctions.ConvertToMoney(anserv.ValuePerHour(sizeOption.TimeToMakeInHours, highestProfit))}");

                    Console.WriteLine($"\t\tAverage Profit: {CommonClientFunctions.ConvertToMoney(averageProfit)} - " +
                        $"Average Profit Per Hour: {CommonClientFunctions.ConvertToMoney(anserv.ValuePerHour(sizeOption.TimeToMakeInHours, averageProfit))}");
                }
            }
        }

        private void Menu()
        {
            Console.WriteLine("Pick a number for your action");
            Console.WriteLine("1> Items Report");

            Console.WriteLine("0> Main Menu");
        }
    }
}