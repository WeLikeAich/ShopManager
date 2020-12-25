using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace ShopManager.Entities
{
    [Table("size_options")]
    public class SizeOption : Entity
    {
        [Required]
        public string Size { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int TimeToMakeInHours { get; set; }

        public List<MaterialCount> MaterialCounts { get; set; } = new List<MaterialCount>();

        public Guid ItemId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Size: {Size} - Price: {Price.ToString("C", CultureInfo.CurrentCulture)} - Turnaround Time: {TimeToMakeInHours}h");
            foreach (MaterialCount materialCount in MaterialCounts)
            {
                sb.AppendLine($"\t\t\t{materialCount}");
            }
            return sb.ToString();
        }
    }
}