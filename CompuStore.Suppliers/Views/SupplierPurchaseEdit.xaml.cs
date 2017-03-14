using System.Windows.Controls;

namespace CompuStore.Suppliers.Views
{
    /// <summary>
    /// Interaction logic for SupplierPurchaseEdit
    /// </summary>
    public partial class SupplierPurchaseEdit : UserControl
    {
        public SupplierPurchaseEdit()
        {
            InitializeComponent();
            this.Loaded += (s, e) => txtSearch.Focus();
        }
    }
}
