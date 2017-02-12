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
        private int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
        private int number;
        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }
        private decimal money;
        public decimal Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }
        private int clientID;
        public int ClientID
        {
            get { return clientID; }
            set { SetProperty(ref clientID, value); }
        }
        public ClientPayment()
        {
            Date = DateTime.Today;
        }
    }
}
