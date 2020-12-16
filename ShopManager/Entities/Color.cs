using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace ShopManager.Entities
{
    [Table("colors")]
    public class Color : Entity
    {
        [Required]
        public string Name { get; set; }

        public string ColorCode { get; set; }

        public decimal Cost { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Name: {Name} - Color Code: {ColorCode} - Cost: {Cost.ToString("C", CultureInfo.CurrentCulture)}");

            return sb.ToString();
        }

        public Guid MaterialId { get; set; }
    }
}