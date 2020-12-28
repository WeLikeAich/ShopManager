﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopManager.Entities
{
    public class MaterialCount : Entity
    {
        public decimal MaterialUnitCount { get; set; }
        public Material Material { get; set; }
        public string Description { get; set; }
        public Guid MaterialId { get; set; }

        public Guid SizeOptionId { get; set; }

        public override string ToString()
        {
            return $"Material Name: {Material.FriendlyName} - Material Unit Count: {MaterialUnitCount} - Description: {Description}";
        }
    }
}