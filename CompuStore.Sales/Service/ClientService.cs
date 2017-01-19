using System;
using System.Collections.ObjectModel;
using CompuStore.Sales.Model;

namespace CompuStore.Sales.Service
{
    public class ClientService : IClientService
    {
        public ObservableCollection<Client> GetAllClients()
        {
            ObservableCollection<Client> clients = new ObservableCollection<Client>();
            clients.Add(new Client() { Name = "Ahmed Mohamed", Phone = "123456789", Address = "Assiut City, Egypt", ID = 1, Notes = "N/A" });
            clients.Add(new Client() { Name = "Mohamed Galal", Phone = "598576398", Address = "Alexandria City, Egypt", ID = 1, Notes = "Premium Client" });
            clients.Add(new Client() { Name = "Ali Sayed",     Phone = "475636875", Address = "Cairo City, Egypt", ID = 1, Notes = "Underrated Client" });
            return clients;
        }
    }
}
