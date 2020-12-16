using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopManager.Entities
{
    public class Material : Entity
    {
        public string FriendlyName { get; set; }

        public string FullMaterialName { get; set; }
        public List<Color> Colors { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\tName: {FriendlyName}\n\tFull Name: {FullMaterialName}");
            foreach (var color in Colors)
                sb.AppendLine($"\t\t{color}");

            return sb.ToString(); ;
        }
    }
}