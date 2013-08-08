using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class ZombiePathFinding : PathFinding
    {
        public ZombiePathFinding() : base()
        {
            walkableBlocks.Add(4);
        }
    }
}
