using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Infrastructure
{
    public static class Manager
    {
        public static SqlConnection Connection = new SqlConnection(@"server=.\sqlexpress; database=inventory; integrated security=true;");
    }
}
