using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace CompuStore.Clients.Model
{
    [Table("ClientPayment")]
    public class ClientPayment : BindableBase
    {
        private int number;
        private DateTime date;
        private decimal money;
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public int ID
        {
            get;set;
        }        
        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }        
        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }        
        public decimal Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }        
        public int ClientID
        {
            get; set;
        }
        public ClientPayment()
        {
            Date = DateTime.Today;
        }
    }
}
