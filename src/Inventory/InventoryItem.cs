using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_.Inventory
{
    class InventoryItem
    {
        private object[] data;
        private string itemName;
        public InventoryItem(object[] data, string itemName)
        {
            this.data = data;
            this.itemName = itemName;
        }

        public string GetName()
        {
            return itemName;
        }

        public object[] GetData()
        {
            return data;
        }

        public int GetXPGain()
        {
            return (int)data[0];
        }

        public int GetShopBuyPrice()
        {
            return (int)data[1];
        }

        public int GetShopSellPrice()
        {
            return (int)data[2];
        }

        public float GetHardness()
        {
            return (float)data[3];
        }

        public int GetLevelRequired()
        {
            return (int)data[4];
        }
    }
}
