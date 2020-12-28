using Microsoft.EntityFrameworkCore;
using ShopManager.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.Services
{
    public class AnalysisService
    {
        private readonly ShopContext _db;

        public AnalysisService()
        {
        }

        public AnalysisService(ShopContext db)
        {
            _db = db;
        }

        public decimal CalculateCost(List<Tuple<decimal, decimal>> count)
        {
            decimal revenue = 0;
            foreach (var tuple in count)
            {
                revenue += tuple.Item1 * tuple.Item2;
            }

            return revenue;
        }

        public decimal CalculateProfit(SizeOption sizeOption, List<Tuple<decimal, decimal>> count)
        {
            return sizeOption.Price - CalculateCost(count);
        }

        public decimal ValuePerHour(int hours, decimal value)
        {
            return value / hours;
        }

        public void GenerateInvoice(List<Item> itemList, string invoiceName)
        {
        }
    }
}