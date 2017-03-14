using System.Windows.Controls;

namespace CompuStore.Clients.Views
{
    /// <summary>
    /// Interaction logic for ClientSaleEdit
    /// </summary>
    public partial class ClientSaleEdit : UserControl
    {
        public ClientSaleEdit()
        {
            InitializeComponent();
            this.Loaded += (s, e) => txtSearch.Focus();
        }
    }
}
