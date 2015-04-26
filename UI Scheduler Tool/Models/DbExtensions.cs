using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;

namespace UI_Scheduler_Tool.Models.Extensions
{
    public static class DbExtensions
    {
        public static TEntity UniqueWhere<TEntity>(this DbSet<TEntity> set, TEntity item, Func<TEntity, bool> expression) where TEntity : class
        {
            // adapted from: https://aangaero.wordpress.com/2013/02/11/avoid-adding-duplicate-records-in-asp-net-using-entity-framework-5/
            var cacheItem = set.FirstOrDefault(expression);
            if (cacheItem == null)
            {
                cacheItem = set.Local.FirstOrDefault(expression);
            }
            return cacheItem == null ? item : cacheItem;
        }

        public static DbSet<TEntity> AddUniqueWhere<TEntity>(this DbSet<TEntity> set, TEntity item, Func<TEntity, bool> expression) where TEntity : class
        {
            set.Add(set.UniqueWhere(item, expression));
            return set;
        }
    }
}