using OstBot_2_.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Shop
    {
        static Dictionary<string, InventoryItem> shopInventory = new Dictionary<string, InventoryItem>();
        static Shop()
        {
            shopInventory.Add("Stone", new InventoryItem(new object[]{
            1, //XPGAIN
            1, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "Stone"));

            shopInventory.Add("Iron", new InventoryItem(new object[]{
            1, //XPGAIN
            1, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "Iron"));
        }

        public InventoryItem Buy(string itemName)
        {
            return null;
        }

        public int Sell(string itemName)
        {
            return 0;
        }
    }
}
