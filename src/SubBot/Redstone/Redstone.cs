﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    public class Redstone : SubBot
    {
        //float[][,] frequency = new float[2][,];   // < kanske senare
        Dictionary<BlockPos, Dictionary<BlockPos, float>> wires = new Dictionary<BlockPos, Dictionary<BlockPos, float>>();
        Dictionary<BlockPos, float> activations = new Dictionary<BlockPos, float>();
        Dictionary<BlockPos, PowerSource> powerSources = new Dictionary<BlockPos, PowerSource>();
        Dictionary<BlockPos, Destination> destinations = new Dictionary<BlockPos, Destination>();
        Stopwatch currentRedTime = new Stopwatch();

        Dictionary<int, float> wireTypes = new Dictionary<int, float>();
        Dictionary<int, PowerSource> powerSourceTypes = new Dictionary<int, PowerSource>();
        Dictionary<int, Destination> destinationTypes = new Dictionary<int, Destination>();

        public Redstone()
            : base()
            {
                currentRedTime.Start();

                wireTypes.Add(189, 0.001F);
                powerSourceTypes.Add(Skylight.BlockIds.Blocks.Metal.BRONZE, new Torch());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Special.GLOSSYBLACK, new Lamp());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Cloud.WHITE, new Lamp());
            }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        resetRed();
                    }
                    break;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            switch (args[0])
            {
                case "resetredstone":
                    if (isBotMod)
                        resetRed();
                    break;
            }
        }

        public override void Update()
        {
            lock (this)
            {
                float power;
                float wirePower;

                foreach (var powerSourcePair in powerSources)
                {
                    power = powerSourcePair.Value.getOutput(currentRedTime);

                    if (power > 0)
                    {
                        if (wires.ContainsKey(powerSourcePair.Key))  //foreach (var wirePair in wires)
                        {
                            foreach (var wire in wires[powerSourcePair.Key])
                            {
                                wirePower = power - wire.Value;

                                if (wirePower > 0)
                                {
                                    if (activations.ContainsKey(wire.Key))
                                    {
                                        if (wirePower > activations[wire.Key])
                                        {
                                            activations[wire.Key] = wirePower;
                                        }
                                    }
                                    else
                                    {
                                        activations.Add(wire.Key, wirePower);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var activationPair in activations)
                {
                    if (powerSources.ContainsKey(activationPair.Key))
                    {
                        powerSources[activationPair.Key].onSignal(currentRedTime, activationPair.Value);
                    }

                    if (destinations.ContainsKey(activationPair.Key))
                    {
                        destinations[activationPair.Key].onSignal(currentRedTime, activationPair.Value);
                    }
                }
                activations.Clear();

                foreach (var powerSourcePair in powerSources)
                {
                    powerSourcePair.Value.Update(currentRedTime, powerSourcePair.Key);
                }

                foreach (var destinationPair in destinations)
                {
                    destinationPair.Value.Update(currentRedTime, destinationPair.Key);
                }
            }
        }

        private void resetRed()
        {
            lock (this)
            {
                Program.console.WriteLine("Reseting redstone...");
                wires.Clear();
                activations.Clear();
                powerSources.Clear();
                destinations.Clear();

                for (int l = 0; l < 2; l++)
                {
                    for (int y = 0; y < OstBot.room.height; y++)
                    {
                        for (int x = 0; x < OstBot.room.width; x++)
                        {
                            Block block = OstBot.room.getBotMapBlock(l, x, y);

                            if (powerSourceTypes.ContainsKey(block.blockId))
                            {
                                powerSources.Add(new BlockPos(x, y, l),
                                    powerSourceTypes[block.blockId].Clone() as PowerSource
                                    );
                            }

                            if (destinationTypes.ContainsKey(block.blockId))
                            {
                                destinations.Add(new BlockPos(x, y, l),
                                    destinationTypes[block.blockId].Clone() as Destination
                                    );
                            }
                        }
                    }
                }

                foreach (var powerSourcePair in powerSources)
                {
                    BlockPos pos = powerSourcePair.Key;
                    BlockPos newPos;

                    float[, ,] redMap = new float[OstBot.room.width, OstBot.room.height, 2];
                    Queue<BlockPos> blockQueue = new Queue<BlockPos>();

                    redMap[pos.x, pos.y, pos.l] = 1.0F;
                    blockQueue.Enqueue(pos);

                    while (blockQueue.Count > 0)
                    {
                        pos = blockQueue.Dequeue();

                        for (int i = 0; i < 4; i++)
                        {
                            float power = redMap[pos.x, pos.y, pos.l];
                            newPos = new BlockPos(pos.x, pos.y, pos.l);

                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 1)
                                {
                                    if (pos.x == powerSourcePair.Key.x && pos.y == powerSourcePair.Key.y)
                                        break;
                                }

                                if (i % 2 == 0)
                                    newPos.x += (i > 1) ? 1 : -1;
                                else
                                    newPos.y += (i > 1) ? 1 : -1;

                                power -= 0.01F;
                                if (power > redMap[newPos.x, newPos.y, newPos.l])
                                    redMap[newPos.x, newPos.y, newPos.l] = power;
                                else
                                    break;

                                Block block = OstBot.room.getBotMapBlock(newPos.l, newPos.x, newPos.y);
                                if (wireTypes.ContainsKey(block.blockId))
                                {
                                    blockQueue.Enqueue(newPos);
                                    break;
                                }
                                else if (destinationTypes.ContainsKey(block.blockId))
                                {
                                    if (!wires.ContainsKey(powerSourcePair.Key))
                                        wires.Add(powerSourcePair.Key, new Dictionary<BlockPos, float>());

                                    if (wires[powerSourcePair.Key].ContainsKey(newPos))
                                        wires[powerSourcePair.Key].Remove(newPos);
                                    wires[powerSourcePair.Key].Add(newPos, 1.0F - power);
                                    break;
                                }
                                else if (powerSourceTypes.ContainsKey(block.blockId))
                                {
                                    if (j == 0)
                                        continue;

                                    if (!wires.ContainsKey(powerSourcePair.Key))
                                        wires.Add(powerSourcePair.Key, new Dictionary<BlockPos, float>());

                                    if (wires[powerSourcePair.Key].ContainsKey(newPos))
                                        wires[powerSourcePair.Key].Remove(newPos);
                                    wires[powerSourcePair.Key].Add(newPos, 1.0F - power);
                                    break;
                                }
                                else if (block.blockId < 9 || block.blockId > 15)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }

    }
}
