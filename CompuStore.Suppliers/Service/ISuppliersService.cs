using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Suppliers.Model;
namespace CompuStore.Suppliers.Service
{
    public interface ISuppliersService
    {
        bool Add(Supplier supplier);
        bool Update(Supplier supplier);
        bool Delete(Supplier supplier);
        IEnumerable<SuppliersDetails> SearchBy(string name);
        IEnumerable<SuppliersDetails> GetAll();
    }
}
