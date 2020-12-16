using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopManager.Entities
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}