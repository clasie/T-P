using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue.Model
{
    /// <summary>
    /// Manage the way we run the Queue.
    /// </summary>
    public class QueueLoadingPolicy 
    {
        //TODO: Implement the QueuePolicy. -CSI
        //TODO: Add policy config into the web config
        public int MaxAmountOfQueueToProcess = 1000;
        public DateTime StartPeriod;
        //...
    }
}
