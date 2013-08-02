using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlayerIOClient;
using Skylight;

namespace OstBot_2_
{
    public class Room : SubBot
    {
        List<Block>[][,] blockMap = new List<Block>[2][,];
        object blockMapLock = 0;

        public static Queue<Block> blockQueue = new Queue<Block>();
        object blockQueueLock = 0;
        public static Queue<Block> blockRepairQueue = new Queue<Block>();
        object blockRepairQueueLock = 0;
        public static HashSet<Block> blockSet = new HashSet<Block>();
        object blockSetLock = 0;

        public int width;
        public int height;
        int drawSleep = 8;

        bool blockDrawerEnabled = false;

        public Room()
        {

        }

        public void setDrawSleep(int drawSleep)
        {
            this.drawSleep = drawSleep;
        }

        public void DrawBlock(Block block)
        {
            if (Block.Compare(getMapBlock(block.layer, block.x, block.y, 0), block))
                return;

            lock (blockSetLock)
            {
                if (blockSet.Contains(block))
                    return;

                blockSet.Add(block);
            }

            lock (blockQueue)
                blockQueue.Enqueue(block);
        }

        public Block getMapBlock(int layer, int x, int y)
        {
            return getMapBlock(layer, x, y, 0);
        }

        public Block getMapBlock(int layer, int x, int y, int rollbacks)
        {
            lock (blockMapLock)
            {
                if (blockMap[layer][x, y].Count > 0)
                {
                    if (blockMap[layer][x, y].Count <= rollbacks)
                        return blockMap[layer][x, y][0];
                    else
                        return blockMap[layer][x, y][blockMap[layer][x, y].Count - 1 - rollbacks];
                }
            }
            return Block.CreateBlock(layer, x, y, 0, -1);
        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    bool isOwner;
                    if (OstBot.isBB)
                    {
                        //worldKey = rot13(m[3].ToString());
                        //botPlayerID = m.GetInt(6);
                        width = m.GetInt(10);
                        height = m.GetInt(11);
                        //hasCode = m.GetBoolean(8);
                        isOwner = m.GetBoolean(9);
                    }
                    else
                    {
                        //worldKey = rot13(m[5].ToString());
                        //botPlayerID = m.GetInt(6);
                        width = m.GetInt(12);
                        height = m.GetInt(13);
                        //hasCode = m.GetBoolean(10);
                        isOwner = m.GetBoolean(11);
                    }


                    if (isOwner)
                        BlockDrawer();

                    lock (blockMapLock)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            blockMap[l] = new List<Block>[width, height];

                            for (int x = 0; x < width; x++)
                                for (int y = 0; y < height; y++)
                                    blockMap[l][x, y] = new List<Block>();
                        }
                    }

                    LoadMap(m, 18);
                    break;

                case "reset":
                    LoadMap(m, 0);
                    break;

                case "access":
                    Thread.Sleep(5);
                    BlockDrawer();
                    break;

                case "b":
                    lock (blockMap)
                        blockMap[m.GetInt(0)][m.GetInt(1), m.GetInt(2)].Add(new Block(m));

                    DrawBlock(getMapBlock(m.GetInt(0), m.GetInt(1), m.GetInt(2), 1)); 
                    break;

                case "bc":
                    lock (blockMap)
                        blockMap[0][m.GetInt(0), m.GetInt(1)].Add(new Block(m));
                    break;

                case "bs":
                    goto case "bc";

                case "pt":
                    goto case "bc";

                case "lb":
                    goto case "bc";

                case "br":
                    goto case "bc";

                
            }
        }

        public void onDisconnect(object sender, string reason)
        {
        }

        private void LoadMap(Message m, uint position)
        {
            lock (blockMapLock)
            {
                byte[] xByteArray;
                byte[] yByteArray;
                for (uint i = position; i < m.Count; i++)
                {
                    if (m[i] is byte[])
                    {
                        int blockID = m.GetInt(i - 2);
                        int layer = m.GetInt(i - 1);
                        xByteArray = m.GetByteArray(i);
                        yByteArray = m.GetByteArray(i + 1);
                        int xIndex = 0;
                        int yIndex = 0;
                        i += 2;
                        for (int x = 0; x < xByteArray.Length; x += 2)
                        {
                            xIndex = (xByteArray[x] * 256) + xByteArray[x + 1];
                            yIndex = (yByteArray[x] * 256) + yByteArray[x + 1];

                            Block block;

                            switch (blockID)
                            {
                                case BlockIds.Action.Doors.COIN:
                                    {
                                        int coinsToOpen = m.GetInt(i);
                                        block = Block.CreateBlockCoin(xIndex, yIndex, blockID, coinsToOpen);
                                        break;
                                    }
                                case BlockIds.Action.Gates.COIN:
                                    goto case BlockIds.Action.Doors.COIN;

                                case BlockIds.Action.Music.PIANO:
                                    {
                                        int soundId = m.GetInt(i);
                                        block = Block.CreateNoteBlock(xIndex, yIndex, blockID, soundId);
                                        break;
                                    }
                                case BlockIds.Action.Music.PERCUSSION:
                                    goto case BlockIds.Action.Music.PIANO;

                                case BlockIds.Action.Portals.NORMAL:
                                    {
                                        int rotation = m.GetInt(i);
                                        int id = m.GetInt(i + 1);
                                        int target = m.GetInt(i + 1);
                                        block = Block.CreatePortal(xIndex, yIndex, rotation, id, target);
                                        break;
                                    }
                                case BlockIds.Action.Portals.INVISIBLE:
                                    goto case BlockIds.Action.Portals.NORMAL;

                                case 1000:
                                    {
                                        string text = m.GetString(i);
                                        block = Block.CreateText(xIndex, yIndex, text);
                                    }
                                    break;

                                case BlockIds.Action.Hazards.SPIKE:
                                    {
                                        int rotation = m.GetInt(i);
                                        block = Block.CreateSpike(xIndex, yIndex, rotation);
                                        break;
                                    }

                                default:
                                    block = Block.CreateBlock(layer, xIndex, yIndex, blockID, -1);
                                    break;

                            }
                            blockMap[layer][xIndex, yIndex].Add(block);//blockID;
                        }
                    }
                }
            }
        }


        private void BlockDrawer()
        {
            OstBot.connection.Send(OstBot.worldKey+"k", true);
            if (!blockDrawerEnabled)
            {
                blockDrawerEnabled = true;
                new Thread(() =>
                {

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (OstBot.connected)
                    {
                        while (OstBot.hasCode)
                        {
                            if (blockQueue.Count != 0)
                            {
                                lock (blockQueueLock)
                                {
                                    blockQueue.Peek().Send(OstBot.connection);
                                    lock (blockRepairQueue)
                                    blockRepairQueue.Enqueue(blockQueue.Dequeue());
                                }
                            }
                            else if (blockRepairQueue.Count != 0)
                            {
                                lock (blockRepairQueue)
                                {
                                    while (!blockSet.Contains(blockRepairQueue.Peek()))
                                    {
                                        blockRepairQueue.Dequeue();
                                        if (blockRepairQueue.Count == 0)
                                            break;
                                    }

                                    if (blockRepairQueue.Count == 0)
                                        continue;

                                    blockRepairQueue.Peek().Send(OstBot.connection);
                                    blockRepairQueue.Enqueue(blockRepairQueue.Dequeue());
                                }
                            }
                            else
                            {
                                Thread.Sleep(5);
                                continue;
                            }
                            double sleepTime = drawSleep - stopwatch.Elapsed.TotalMilliseconds;
                            if (sleepTime >= 0.5)
                            {
                                Thread.Sleep((int)sleepTime);
                            }
                            stopwatch.Reset();
                        }
                        Thread.Sleep(100);
                    }

                }).Start();
            }
        }


    }
}
