using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_.Inventory
{
    class Inventory
    {
        public List<InventoryItem> storedItems;

        public Inventory(int size)
        {
            storedItems = new List<InventoryItem>(size);
        }

        public InventoryItem GetItem(int slot)
        {
            lock (storedItems)
            {
                return storedItems[slot];
            }
        }

        public int GetItems(int slot)
        {
            lock (storedItems)
            {
                if (storedItems[slot] != null)
                {
                    return storedItems[slot].GetAmount();
                }
                return 0;
            }
        }

        public InventoryItem GetItemByName(string name)
        {
            lock (storedItems)
            {
                foreach (InventoryItem i in storedItems)
                {
                    if (i.GetName() == name)
                        return (i);
                }
                return null;
            }
        }

        public List<InventoryItem> GetAllItems()
        {
            lock (storedItems)
            {
                return storedItems;
            }
        }

        public bool SetItem(InventoryItem item, int slot)
        {
            lock (storedItems)
            {
                if (storedItems[slot] != null)
                {
                    storedItems[slot] = item;
                    return true;
                }
                return false;
            }
        }

        public bool RemoveItem(int slot)
        {
            lock (storedItems)
            {
                if (storedItems[slot] != null)
                {
                    storedItems.RemoveAt(slot);
                    return true;
                }
                return false;
            }
        }

        public bool AddItem(InventoryItem item)
        {
            lock (storedItems)
            {
                if (storedItems.Contains(item))
                {
                    storedItems[storedItems.IndexOf(item)]]
                }
                if (storedItems.Count != storedItems.Capacity)
                {
                    storedItems.Add(item);
                    return true;
                }
                return false;
            }
        }
    }

}
