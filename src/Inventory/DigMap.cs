using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_.Inventory
{
    class DigBlockMap
    {
        static Dictionary<int, InventoryItem> IdToItem = new Dictionary<int, InventoryItem>();
        public DigBlockMap()
        {
            IdToItem.Add((int)Blocks.Stone, new InventoryItem(new object[]{
            1, //XPGAIN
            1, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "Stone"));

            IdToItem.Add((int)Blocks.Iron, new InventoryItem(new object[]{
            1, //XPGAIN
            1, //SHOPBUY
            0, //SHOPSELL
            1, //HARDNESS
            0  //LEVELREQ
            }, "Iron"));
        }
    }
    static enum Blocks
    {
        Stone = Skylight.BlockIds.Blocks.Basic.GRAY,
        Iron = Skylight.BlockIds.Blocks.Metal.SILVER,
        Copper = Skylight.BlockIds.Blocks.Metal.BRONZE,
        Gold = Skylight.BlockIds.Blocks.Metal.GOLD,
        Diamond = Skylight.BlockIds.Blocks.Minerals.CYAN,
        Ruby = Skylight.BlockIds.Blocks.Minerals.RED,
        Sapphire = Skylight.BlockIds.Blocks.Minerals.BLUE,
        Emerald = Skylight.BlockIds.Blocks.Minerals.GREEN
    };
}
