using System;
using System.Text;
using PlayerIOClient;

namespace OstBot_2_
{
    public class Block
    {
        string blockType;
        object[] dataArray;

        public int x
        {
            get { return (int)dataArray[1]; }
        }

        public int y
        {
            get { return (int)dataArray[2]; }
        }

        public int layer
        {
            get { return (int)dataArray[0]; }
        }

        public int blockId
        {
            get { return (int)dataArray[3]; }
        }

        public int b_userId
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "b")
                    return (int)dataArray[4];
                return 0;
            }
        }

        public int bc_coinsToOpen
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "bc")
                    return (int)dataArray[4];
                return 0;
            }
        }

        public int bs_soundId
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "bs")
                    return (int)dataArray[4];
                return 0;
            }
        }

        public int pt_rotation
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "pt")
                    return (int)dataArray[4];
                return 0;
            }
        }
        public int pt_id
        {
            get
            {
                if (dataArray.Length >= 5 && blockType == "pt")
                    return (int)dataArray[5];
                return 0;
            }
        }

        public int pt_target
        {
            get
            {
                if (dataArray.Length >= 6 && blockType == "pt")
                    return (int)dataArray[6];
                return 0;
            }
        }

        public string lb_text
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "lb")
                    return (string)dataArray[4];
                return "";
            }
        }

        public int br_rotation
        {
            get
            {
                if (dataArray.Length >= 4 && blockType == "br")
                    return (int)dataArray[4];
                return 0;
            }
        }

        public Block(Message m) : this()
        {
            dataArray = new object[m.Count];

            blockType = m.Type;

            int i;

            if (blockType == "b")
            {
                i = 0;
            }
            else
            {
                i = 1;
                dataArray[0] = 0;
            }

            for (int j = 0; j < m.Count; j++)
            {
                dataArray[i + j] = OstBot.toObject(m, (uint)(i + j));
            }
        }

        protected Block()
        {

        }

        public static bool Compare(Block a, Block b)
        {
            if (a.blockType != b.blockType)
                return false;

            if (a.dataArray[3] != b.dataArray[3])
                return false;

            /*object[] dataArrayA = new object[a.dataArray.Length];
            object[] dataArrayB = new object[b.dataArray.Length];

            Array.Copy(dataArrayA, a.dataArray, a.dataArray.Length);
            Array.Copy(dataArrayB, b.dataArray, b.dataArray.Length);*/

            switch (a.blockType)
            {
                case "b":
                    return true;

                case "bc":
                    return a.dataArray[3] == b.dataArray[3];

                case "bs":
                    goto case "bc";

                case "pt":
                    return a.dataArray[3] == b.dataArray[3] && a.dataArray[4] == b.dataArray[4] && a.dataArray[5] == b.dataArray[5];

                case "lb":
                    goto case "bc";

                case "br":
                    goto case "bc";

                default:
                    return true;
            }
        }

        public static Block CreateBlock(int layer, int x, int y, int blockId, int userId)
        {
            Block block = new Block();
            block.blockType = "b";
            block.dataArray = new object[] { layer, x, y, blockId, userId };
            return block;
        }

        public static Block CreateBlockCoin(int x, int y, int blockId, int coinsToOpen)
        {
            Block block = new Block();
            block.blockType = "bc";
            block.dataArray = new object[] { 0, x, y, blockId, coinsToOpen };
            return block;
        }

        public static Block CreateNoteBlock(int x, int y, int blockId, int soundId)
        {
            Block block = new Block();
            block.blockType = "bs";
            block.dataArray = new object[] { 0, x, y, blockId, soundId };
            return block;
        }

        public static Block CreatePortal(int x, int y, int rotation, int id, int target)
        {
            Block block = new Block();
            block.blockType = "pt";
            block.dataArray = new object[] { 0, x, y, 242, rotation, id, target };
            return block;
        }

        public static Block CreateText(int x, int y, string text)
        {
            Block block = new Block();
            block.blockType = "lb";
            block.dataArray = new object[] { 0, x, y, 1000, text };
            return block;
        }

        public static Block CreateSpike(int x, int y, int rotation)
        {
            Block block = new Block();
            block.blockType = "br";
            block.dataArray = new object[] { 0, x, y, 361, rotation};
            return block;
        }

        public object getObject(int index)
        {
            if (index < dataArray.Length)
                return dataArray[index];
            else
                return 0;
        }

        public int getDataSize()
        {
            return dataArray.Length;
        }

        public void Send(Connection connection)
        {
            if (blockType == "")
            {
                throw new System.Exception("Attempt to draw a block of void.");
            }
            else if (blockType == "b")
            {
                Object[] sendData = new object[dataArray.Length - 1];
                Array.Copy(dataArray, sendData, dataArray.Length - 1);
                connection.Send(OstBot.worldKey, sendData);
            }
            else
            {
                connection.Send(OstBot.worldKey, dataArray);
            }
        }
    }
}
