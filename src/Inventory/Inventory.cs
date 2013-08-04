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

        public bool RemoveItem(InventoryItem item, int amount)
        {
            InventoryItem itemToRemove = null;
            bool removeAll = false;
            lock (storedItems)
            {
                foreach (InventoryItem i in storedItems)
                {
                    if (i.GetName() == item.GetName() && i.GetData() == item.GetData())
                    {
                        if (i.GetAmount() > amount)
                        {
                            i.SetAmount(i.GetAmount() - amount);
                            return true;
                        }
                        else
                        {
                            itemToRemove = new InventoryItem(i.GetData(), i.GetName(), item.GetAmount());
                            removeAll = true;
                        }
                    }
                }
                if (removeAll)
                {
                    storedItems.Remove(item);
                    return true;
                }
                return false;
            }
        }

        public bool AddItem(InventoryItem item)
        {
            lock (storedItems)
            {
                foreach (InventoryItem i in storedItems)
                {
                    if (i.GetData() == item.GetData() && i.GetName() == item.GetName())
                    {
                        storedItems[storedItems.IndexOf(i)].SetAmount(storedItems[storedItems.IndexOf(i)].GetAmount() + item.GetAmount());
                        return true;
                    }
                }
                if (storedItems.Count != storedItems.Capacity)
                {
                    storedItems.Add(item);
                    return true;
                }
                return false;
            }
        }

        public string GetContents()
        {
            lock (storedItems)
            {
                string contents = "Inventory: ";
                foreach (InventoryItem i in storedItems)
                {
                    contents += i.GetAmount() + " " + i.GetName() + ",";
                }
                return contents;
            }
        }

        public bool Contains(InventoryItem item)
        {
            lock (storedItems)
            {
                foreach (InventoryItem i in storedItems)
                {
                    if (i.GetName() == item.GetName() && i.GetData() == item.GetData())
                        return true;
                }
                return false;
            }
        }

        public int GetAmount(InventoryItem item)
        {
            lock (storedItems)
            {
                foreach (InventoryItem i in storedItems)
                {
                    if (i.GetName() == item.GetName() && i.GetData() == item.GetData())
                    {
                        return i.GetAmount();
                    }
                }
                return 0;
            }
        }
    }

}
