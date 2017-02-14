using CompuStore.Store.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store
{
    class CategoryAdded : PubSubEvent<Category> { }
    class CategoryDeleted : PubSubEvent<Category> { }
    class ItemAdded : PubSubEvent<Item> { }
    class ItemDeleted : PubSubEvent<Item> { }
    class ItemRefresh : PubSubEvent<Item> { }
}
