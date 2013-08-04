using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class DigBlockMap
    {
        public static Dictionary<int, InventoryItem> blockTranslator = new Dictionary<int, InventoryItem>();
        public static Dictionary<string, InventoryItem> itemTranslator = new Dictionary<string, InventoryItem>();
        static DigBlockMap()
        {
            blockTranslator.Add((int)Blocks.Stone, new InventoryItem(new object[]{
            1, //XPGAIN
            1, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "stone"));

            blockTranslator.Add((int)Blocks.Iron, new InventoryItem(new object[]{
            2, //XPGAIN
            2, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "iron"));

            blockTranslator.Add((int)Blocks.Copper, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "copper"));

            blockTranslator.Add((int)Blocks.Gold, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "gold"));

            blockTranslator.Add((int)Blocks.Diamond, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "diamond"));

            blockTranslator.Add((int)Blocks.Ruby, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "ruby"));

            blockTranslator.Add((int)Blocks.Sapphire, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "sapphire"));

            blockTranslator.Add((int)Blocks.Emerald, new InventoryItem(new object[]{
            5, //XPGAIN
            5, //SHOPBUY
            0, //SHOPSELL
            2, //HARDNESS
            0  //LEVELREQ
            }, "emerald"));


            foreach (InventoryItem i in blockTranslator.Values)
            {
                itemTranslator.Add(i.GetName(), i);
            }
        }
    }
    public enum Blocks
    {
        Stone = Skylight.BlockIds.Blocks.Castle.BRICK,
        Iron = Skylight.BlockIds.Blocks.Metal.SILVER,
        Copper = Skylight.BlockIds.Blocks.Metal.BRONZE,
        Gold = Skylight.BlockIds.Blocks.Metal.GOLD,
        Diamond = Skylight.BlockIds.Blocks.Minerals.CYAN,
        Ruby = Skylight.BlockIds.Blocks.Minerals.RED,
        Sapphire = Skylight.BlockIds.Blocks.Minerals.BLUE,
        Emerald = Skylight.BlockIds.Blocks.Minerals.GREEN
    };
}
