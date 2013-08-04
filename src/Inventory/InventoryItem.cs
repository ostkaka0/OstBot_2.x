using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class InventoryItem
    {
        private object[] data;
        private string itemName;
        private int amount = 1;
        public InventoryItem(object[] data, string itemName, int amount)
        {
            this.data = data;
            this.itemName = itemName;
            this.amount = amount;
        }

        public string GetName()
        {
            return itemName;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public int GetAmount()
        {
            return amount;
        }

        public object[] GetData()
        {
            return data;
        }

        public object GetDataAt(int index)
        {
            return (data[index]);
        }

        public void SetData(object[] data)
        {
            this.data = data;
        }

        public void SetDataAt(object data, int index)
        {
            this.data[index] = data;
        }
    }
}
