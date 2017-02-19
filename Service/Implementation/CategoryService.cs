using System.Collections.Generic;
using Model;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        IDbConnection Connection;
        public bool Add(Category category)
        {
            return Connection.Insert(category) != 0;
        }

        public bool Delete(Category category)
        {
            return Connection.Delete(category);
        }

        public IEnumerable<Category> GetAll()
        {
            return Connection.Query<Category>("Select * from Category");
        }

        public bool IsDeletable(Category category)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", category.ID);
            int? x = (int?)Connection.QuerySingleOrDefault("Select top 1 1 from Item where CategoryID=@CategoryID", args);
            return x.HasValue && x.Value != 0;
        }

        public bool Update(Category category)
        {
            return Connection.Update(category);
        }

        public List<string> GetNamesOfItemsForCategory(Category category)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", category.ID);
            return Connection.Query<string>("Select Name from Item where CategoryID=@CategoryID", args).AsList();
        }

        public CategoryService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
