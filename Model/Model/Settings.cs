using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Settings")]
    public class Settings
    {
        public int ID { get; set; }
        //public int Trials { get; set; }
        public string Serial { get; set; }
    }
}
