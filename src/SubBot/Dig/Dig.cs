using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Graphics.Tools.Noise;
using Skylight;

namespace OstBot_2_
{
    public partial class Dig : SubBot
    {
        protected Queue<Block> dugBlocksToPlaceQueue = new Queue<Block>();
        protected object dugBlocksToPlaceQueueLock = 0;
        protected float[,] digHardness;
        protected int minDigRange;

        public override void Update()
        {
            if (!OstBot.hasCode)
                return;

            lock (dugBlocksToPlaceQueueLock)
            {
                while (dugBlocksToPlaceQueue.Count > OstBot.room.width * OstBot.room.height / 20)
                {
                    OstBot.room.DrawBlock(dugBlocksToPlaceQueue.Dequeue());
                }
            }
        }

        private bool isDigable(int blockId)
        {
            if (blockId >= Skylight.BlockIds.Blocks.Sand.BROWN && blockId <= Skylight.BlockIds.Blocks.Sand.BROWN)
                return true;
            else if (blockId >= 16 && blockId <= 21)
                return true;
            else if (blockId == BlockIds.Blocks.JungleRuins.BLUE)
                return true;
            else
                return false;
        }

        private void DigBlock(int x, int y, BotPlayer player, float digStrength, bool mining)
        {
            if (digHardness == null)
                resetDigHardness();

            if (!(x > 0 && y > 0 && x < OstBot.room.width && y < OstBot.room.height))
                return;

            if (digHardness[x,y] <= 0)
                return;

            Block block = OstBot.room.getMapBlock(0, x, y, 0);

            int blockId = -1;

            if (mining)
            {
                if (DigBlockMap.blockTranslator.ContainsKey(block.blockId))
                {
                    blockId = 4;

                    InventoryItem temp = DigBlockMap.blockTranslator[block.blockId];

                    if (player.digLevel >= Convert.ToInt32(temp.GetDataAt(5)))
                    {
                        //Shop.shopInventory[DigBlockMap.blockTranslator[block.blockId]].GetDataAt(3)//för hårdhet
                        if (digHardness[x, y] <= digStrength)
                        {

                            InventoryItem newsak = new InventoryItem(temp.GetData());
                            player.inventory.AddItem(newsak, 1);
                            player.digXp += Convert.ToInt32(temp.GetDataAt(1));
                        }
                    }
                    else
                    {
                        return;
                    }

                }
            }
            else
            {
                //if (!isReachAble(new BlockPos(player.blockX, player.blockY, 0), new BlockPos(x, y, 0)))
                 //   return;
            }

            switch (block.blockId)
            {
                case BlockIds.Blocks.Sand.BROWN:
                    blockId = 4;
                    break;

                case BlockIds.Blocks.JungleRuins.BLUE:
                    blockId = BlockIds.Action.Liquids.WATER;
                    break;

                case 21:
                    blockId = 369;//BlockIds.Action.Liquids.MUD;
                    break;

                default:
                    if (blockId == -1)
                        return;
                    else
                        break;
            }

            digHardness[x, y] -= digStrength;

            if (digHardness[x, y] <= 0)
            {
                OstBot.room.DrawBlock(Block.CreateBlock(0, x, y, blockId, -1));
                lock (dugBlocksToPlaceQueueLock)
                    dugBlocksToPlaceQueue.Enqueue(block);
            }
        }

        private bool isReachAble(BlockPos start, BlockPos end)
        {
            int deltaX = start.x - end.x;
            int deltaY = start.y - end.y;
            Block block;

            while (deltaX != 0 || deltaY != 0)
            {
                block = OstBot.room.getBotMapBlock(0, start.x, start.y);

                if (!isDigable(block.blockId) && block.blockId >= 9)
                    return false;

                if (deltaX != 0)
                {
                    start.x += deltaX / Math.Abs(deltaX);
                }
                if (deltaY != 0)
                {
                    start.y += deltaY / Math.Abs(deltaY);
                }

                deltaX = start.x - end.x;
                deltaY = start.y - end.y;
            }

            block = OstBot.room.getBotMapBlock(0, start.x, start.y);

            return (isDigable(block.blockId) || block.blockId < 9);
        }

        private void resetDigHardness()
        {
            digHardness = new float[OstBot.room.width, OstBot.room.height];

            for (int y = 0; y < OstBot.room.height; y++)
            {
                for (int x = 0; x < OstBot.room.width; x++)
                {
                    resetBlockHardness(x, y, OstBot.room.getMapBlock(0, x, y ,0).blockId);
                }
            }
        }

        private void resetBlockHardness(int x, int y, int blockId)
        {
            //if (!OstBot.hasCode) < ta bort detta
            //    return;

            if (x < 0 || y < 0 || x >= OstBot.room.width || y >= OstBot.room.height)

            if (digHardness == null)
            {
                //lock (digHardness)
                {
                    digHardness = new float[OstBot.room.width, OstBot.room.height];
                }
            }

            if (digHardness == null)
                return;
            //lock (digHardness)
            {
                if (isDigable(blockId))
                {
                    digHardness[x, y] = 1F;
                }
                else if (DigBlockMap.blockTranslator.ContainsKey(blockId))
                {
                    if (Shop.shopInventory.ContainsKey(DigBlockMap.blockTranslator[blockId].GetName()))
                        digHardness[x, y] = Convert.ToInt32(Shop.shopInventory[DigBlockMap.blockTranslator[blockId].GetName()].GetDataAt(4));
                }
            }
        }

    }
}
