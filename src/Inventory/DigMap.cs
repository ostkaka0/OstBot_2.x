using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class DigBlockMap
    {
        public static Dictionary<int, string> blockTranslator = new Dictionary<int, string>();
        public DigBlockMap()
        {
            blockTranslator.Add((int)Blocks.Stone, "Stone");
            blockTranslator.Add((int)Blocks.Iron, "Iron");
        }
    }
    public enum Blocks
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
