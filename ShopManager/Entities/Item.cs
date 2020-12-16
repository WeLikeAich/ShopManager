using ShopManager.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopManager.Entities
{
    [Table("items")]
    public class Item : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<SizeOption> SizeOptions { get; set; } = new List<SizeOption>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\tName: {Name}\n\tDescription: {Description}");
            foreach (var item in SizeOptions)
                sb.AppendLine($"\t\t{item}");

            return sb.ToString(); ;
        }
    }
}