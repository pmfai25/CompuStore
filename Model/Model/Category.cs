using Dapper.Contrib.Extensions;
using Prism.Mvvm;
namespace Model
{
    [Table("Category")]
    public class Category:BindableBase
    {
        public int ID { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
    }
}
