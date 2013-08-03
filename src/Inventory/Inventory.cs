using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Inventory
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

        /*public bool SetItem(InventoryItem item, int slot)
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
        }*/

        public bool RemoveItem(int slot, int amount)
        {
            lock (storedItems)
            {
                if (storedItems[slot] != null)
                {
                    if (storedItems[slot].GetAmount() > 1)
                        storedItems[slot].SetAmount(storedItems[slot].GetAmount() - 1);
                    else
                        storedItems.RemoveAt(slot);
                    return true;
                }
                return false;
            }
        }

        public bool AddItem(InventoryItem item, int amount)
        {
            lock (storedItems)
            {
                if (storedItems.Contains(item))
                {
                    storedItems[storedItems.IndexOf(item)].SetAmount(storedItems[storedItems.IndexOf(item)].GetAmount() + amount);
                    return true;
                }
                if (storedItems.Count != storedItems.Capacity)
                {
                    item.SetAmount(amount);
                    storedItems.Add(item);
                    return true;
                }
                return false;
            }
        }

        public string GetContents()
        {
            string contents = "Inventory: ";
            foreach (InventoryItem i in storedItems)
            {
                contents += i.GetAmount() + " " + i.GetName() + ",";
            }
            return contents;
        }
    }

}
