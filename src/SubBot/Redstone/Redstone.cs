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

        List<int> layerSwitches = new List<int>();
        Dictionary<int, float> wireTypes = new Dictionary<int, float>();
        Dictionary<int, PowerSource> powerSourceTypes = new Dictionary<int, PowerSource>();
        Dictionary<int, Destination> destinationTypes = new Dictionary<int, Destination>();

        // *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***
        /// Bra lösning för att testa om material leder ström
        delegate bool IsConductive(BlockPos pos, int jumps, float power, Block sourceBlock, Block block);
        List<IsConductive> conductivityDelegateList = new List<IsConductive>();

        public Redstone()
            : base()
            {
                UpdateSleep = 10;
                currentRedTime.Start();

                wireTypes.Add(189, 0.001F);
                wireTypes.Add(Skylight.BlockIds.Background.Checker.RED, 0.001F);

                conductivityDelegateList.Add(new IsConductive((BlockPos pos, int jumps, float power, Block sourceBlock, Block block)=>  // colored wires
                {
                    return (block.blockId >= Skylight.BlockIds.Blocks.Checker.GRAY
                        && block.blockId <= Skylight.BlockIds.Blocks.Checker.CYAN
                        && (block.blockId == sourceBlock.blockId
                            || sourceBlock.blockId == 20 // redstone
                            || powerSourceTypes.ContainsKey(sourceBlock.blockId)
                            || layerSwitches.Contains(sourceBlock.blockId)));
                }));
                conductivityDelegateList.Add(new IsConductive((BlockPos pos, int jumps, float power, Block sourceBlock, Block block) =>
                {
                    return block.blockId == 20;
                }));

                layerSwitches.Add(Skylight.BlockIds.Blocks.Industrial.CROSSSUPPORT);
                layerSwitches.Add(Skylight.BlockIds.Background.Carnival.CHECKER);
                powerSourceTypes.Add(Skylight.BlockIds.Blocks.Metal.BRONZE, new Torch());
                powerSourceTypes.Add(Skylight.BlockIds.Blocks.Metal.SILVER, new Repeater());
                powerSourceTypes.Add(Skylight.BlockIds.Decorative.Cloud.BOTTOM, new PressurePlate());
                powerSourceTypes.Add(Skylight.BlockIds.Decorative.Sand.WHITE, new PressurePlate());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Special.GLOSSYBLACK, new Lamp());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Cloud.WHITE, new Lamp());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Scifi.GRAY, new Door());
                destinationTypes.Add(Skylight.BlockIds.Blocks.Secrets.NONSOLID, new Door());
            }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        ResetRed();
                    }
                    break;

                case "b":
                    {
                        Block block = new Block(m);
                        Block oldBlock = OstBot.room.getMapBlock(block.layer, block.x, block.y, 1);

                        BlockPos position = new BlockPos(block.x, block.y, block.layer);

                        if (wireTypes.ContainsKey(oldBlock.blockId) || (block.blockId >= 9 && block.blockId <= 15))
                        {
                            if (!wireTypes.ContainsKey(block.blockId))
                            {
                                lock (this)
                                    ResetPowerSources();
                                break;
                            }
                        }
                        else if (powerSourceTypes.ContainsKey(oldBlock.blockId))
                        {
                            if (!powerSourceTypes.ContainsKey(block.blockId))
                            {
                                lock (this)
                                {
                                    RemoveWiresFromPowerSource(new KeyValuePair<BlockPos, PowerSource>(
                                        position,
                                        powerSourceTypes[oldBlock.blockId]));

                                    powerSources.Remove(position);
                                }
                            }
                        }
                        else if (destinationTypes.ContainsKey(oldBlock.blockId))
                        {
                            if (!destinationTypes.ContainsKey(block.blockId))
                            {
                                lock (this)
                                {
                                    if (destinations.ContainsKey(position))
                                    {
                                        destinations.Remove(position);
                                        ResetPowerSources();
                                    }
                                }
                                break;
                                /*lock (this)
                                    RemoveWiresFromDestination(new KeyValuePair<BlockPos, Destination>(
                                        new BlockPos(block.x, block.y, block.layer),
                                        destinationTypes[oldBlock.blockId]));*/
                            }
                        }

                        if (wireTypes.ContainsKey(block.blockId))
                        {
                            lock (this)
                                ResetPowerSources();
                        }
                        else if (powerSourceTypes.ContainsKey(block.blockId))
                        {
                            lock (this)
                            {
                                if (!powerSources.ContainsKey(new BlockPos(block.x, block.y, block.layer)))
                                {
                                    KeyValuePair<BlockPos, PowerSource> powerSourceKeyValuePair = new KeyValuePair<BlockPos, PowerSource>(new BlockPos(block.x, block.y, block.layer),
                                        powerSourceTypes[block.blockId].Create() as PowerSource);
                                    powerSources.Add(powerSourceKeyValuePair.Key, powerSourceKeyValuePair.Value);
                                    ResetPowerSources();//ResetPowerSourceWires(powerSourceKeyValuePair);
                                }
                            }
                        }
                        else if (destinationTypes.ContainsKey(block.blockId))
                        {
                            lock (this)
                            {
                                if (!destinations.ContainsKey(new BlockPos(block.x, block.y, block.layer)))
                                {
                                    destinations.Add(new BlockPos(block.x, block.y, block.layer),
                                        destinationTypes[block.blockId].Create() as Destination);
                                    ResetPowerSources();
                                }
                            }
                        }
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
                        ResetRed();
                    break;
            }
        }

        /// <summary>
        /// Updates the redstone.
        /// </summary>
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

        /// <summary>
        /// Resets all redstone.
        /// </summary>
        private void ResetRed()
        {
            lock (this)
            {
                Program.mainForm.WriteToConsole("Reseting redstone...");
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
                                    powerSourceTypes[block.blockId].Create() as PowerSource
                                    );
                            }

                            if (destinationTypes.ContainsKey(block.blockId))
                            {
                                destinations.Add(new BlockPos(x, y, l),
                                    destinationTypes[block.blockId].Create() as Destination
                                    );
                            }
                        }
                    }
                }

                foreach (var powerSourceKeyValuePair in powerSources)
                {
                    ResetPowerSourceWires(powerSourceKeyValuePair);
                }
                
            }

        }

        /// <summary>
        /// Resets the power sources
        /// </summary>
        private void ResetPowerSources()
        {
            wires.Clear();
            foreach (KeyValuePair<BlockPos, PowerSource> powerSourceKeyValuePair in powerSources)
            {
                ResetPowerSourceWires(powerSourceKeyValuePair);
            }
        }

        /// <summary>
        /// Resets the wires from a power source.
        /// </summary>
        /// <param name="powerSourceKeyValuePair">Resets the wires from this power source.</param>
        private void ResetPowerSourceWires(KeyValuePair<BlockPos, PowerSource> powerSourceKeyValuePair)
        {
            //RemoveWiresFromPowerSource(powerSourceKeyValuePair);

            BlockPos pos = powerSourceKeyValuePair.Key;
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
                            if (pos.x == powerSourceKeyValuePair.Key.x && pos.y == powerSourceKeyValuePair.Key.y)
                                break;
                        }

                        if (i % 2 == 0)
                            newPos.x += (i > 1) ? 1 : -1;
                        else
                            newPos.y += (i > 1) ? 1 : -1;

                        if (newPos.x <= 1 || newPos.x >= OstBot.room.width - 2 || newPos.y <= 1 || newPos.y >= OstBot.room.height - 2)
                            break;

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
                        else if (layerSwitches.Contains(block.blockId))
                        {
                            blockQueue.Enqueue(new BlockPos(newPos.x, newPos.y, newPos.l^1));
                            redMap[newPos.x, newPos.y, newPos.l ^ 1] = power;
                            break;
                        }
                        else if (destinationTypes.ContainsKey(block.blockId))
                        {
                            if (!wires.ContainsKey(powerSourceKeyValuePair.Key))
                                wires.Add(powerSourceKeyValuePair.Key, new Dictionary<BlockPos, float>());

                            if (wires[powerSourceKeyValuePair.Key].ContainsKey(newPos))
                                wires[powerSourceKeyValuePair.Key][newPos] = 1.0F - power;//wires[powerSourceKeyValuePair.Key].Remove(newPos);
                            else
                                wires[powerSourceKeyValuePair.Key].Add(newPos, 1.0F - power);
                            break;
                        }
                        else if (powerSourceTypes.ContainsKey(block.blockId))
                        {
                            if (j == 0)
                                continue;

                            if (!wires.ContainsKey(powerSourceKeyValuePair.Key))
                                wires.Add(powerSourceKeyValuePair.Key, new Dictionary<BlockPos, float>());

                            if (wires[powerSourceKeyValuePair.Key].ContainsKey(newPos))
                                wires[powerSourceKeyValuePair.Key][newPos] = 1.0F - power;
                            else    //wires[powerSourceKeyValuePair.Key].Remove(newPos);
                                wires[powerSourceKeyValuePair.Key].Add(newPos, 1.0F - power);
                            break;
                        }
                        else if ((block.blockId < 9 || block.blockId > 15) && (block.blockId < 500 || block.blockId > 507))
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ?
        /// </summary>
        private void ResetPowerSourcesFromWire()//(BlockPos pos)
        {
            foreach (KeyValuePair<BlockPos, PowerSource> powerSourceKeyValuePair in powerSources)
            {
                ResetPowerSourceWires(powerSourceKeyValuePair);
            }
        }

        private void RemoveWiresFromDestination(KeyValuePair<BlockPos, Destination> destinationKeyValuePair)
        {
            foreach (var wiresKeyValuePair in wires)
            {
                if (wiresKeyValuePair.Value.ContainsKey(destinationKeyValuePair.Key))
                {
                    wiresKeyValuePair.Value.Remove(destinationKeyValuePair.Key);
                }
            }
        }

        private void RemoveWiresFromPowerSource(KeyValuePair<BlockPos, PowerSource> powerSourceKeyValuePair)
        {
            if (wires.ContainsKey(powerSourceKeyValuePair.Key))
            {
                wires.Remove(powerSourceKeyValuePair.Key);
            }

            foreach (var wiresKeyValuePair in wires)
            {
                if (wiresKeyValuePair.Value.ContainsKey(powerSourceKeyValuePair.Key))
                {
                    wiresKeyValuePair.Value.Remove(powerSourceKeyValuePair.Key);
                }
            }
        }
    }
}
