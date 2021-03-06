﻿using Microsoft.EntityFrameworkCore;
using ShopManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopManager.Services
{
    public class DataService
    {
        private readonly ShopContext _db;

        public DataService(ShopContext db)
        {
            _db = db;
        }

        public void Create<TEntity>(TEntity entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }

        public List<Item> GetItems()
        {
            return _db.Items.Include(i => i.SizeOptions)
                                 .ThenInclude(so => so.MaterialCounts)
                                 .ThenInclude(mc => mc.Material)
                                 .ThenInclude(m => m.Colors)
                                 .ToList();
        }

        public List<Material> GetMaterials()
        {
            return _db.Materials.Include(m => m.Colors).ToList();
        }

        public Material GetMaterialById(Guid id)
        {
            return _db.Materials.Include(m => m.Colors).FirstOrDefault(m => m.Id == id);
        }

        public List<Color> GetColorsByMaterialId(Guid id)
        {
            return _db.Colors.Where(c => c.MaterialId == id).ToList();
        }

        public void Delete<TType>(TType entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
            {
                _db.Attach(entity);
            }
            _db.Remove(entity);
            _db.SaveChanges();
        }

        public void Update<TEntity>(TEntity entity)
        {
            _db.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        internal List<SizeOption> GetSizeOptionsByItemId(Guid id)
        {
            return _db.SizeOptions.Where(so => so.ItemId == id).Include(so => so.MaterialCounts).ThenInclude(mc => mc.Material).ToList();
        }

        internal List<MaterialCount> GetMaterialCountsBySizeOptionId(Guid id)
        {
            return _db.MaterialCounts.Where(mc => mc.SizeOptionId == id).Include(mc => mc.Material).ToList();
        }
    }
}