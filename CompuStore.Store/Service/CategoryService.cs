using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Store.Model;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
namespace CompuStore.Store.Service
{
    public class CategoryService : ICategoryService
    {
        SqlConnection Connection;
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
        public CategoryService(SqlConnection connection)
        {
            Connection = connection;
        }
    }
}
