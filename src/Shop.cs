using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Shop
    {
        static int shopX = 0;
        static int shopY = 0;
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
            2, //XPGAIN
            2, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "Iron"));

            shopInventory.Add("Gold", new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "Gold"));
        }

        public InventoryItem Buy(string itemName, int amount)
        {
            InventoryItem temp = shopInventory[itemName];
            temp.SetAmount(amount);
            return temp;
        }

        public int Sell(string itemName, int amount)
        {
            return ((int)shopInventory[itemName].GetDataAt(2)) * amount;
        }
    }
}
