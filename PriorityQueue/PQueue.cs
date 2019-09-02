
//---What-------------------------------------------------------------
//
// This modified implementation is inpired from the article of James McCaffrey found here (https://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx)
//
//---Why-------------------------------------------------------------
//
// The interresting thing is about the way the liste is half sorted in order to optimize
// reloads and resorts when new elements are added/removed.
// The maximum of operations is O(lg n) vs O(n lg n) for normal list ordering.
//
//---Modifs-------------------------------------------------------------
//
// The modifications added made this a singleton thread safe and methods locked
// to protect inconsistencies in a Web multiple thread context.
//
//-------------------------------------------------------------


//using Models.ComptaPlusModel;
using PriorityQueue;
using PriorityQueue.Enum;
using PriorityQueue.Model;
using PriorityQueue.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Constants;
using TokenHandler.Models;

namespace PriorityQueue
{
    public sealed class PQueue
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( TokenKey.NormalLogsNameSpace);
        private List<QueueItem> queueItemList;
        private static readonly Lazy<PQueue> lazy = new Lazy<PQueue>(() => new PQueue());
        private static readonly object KeyLock = new object();//for method calls
        public static PQueue Instance { get { return lazy.Value; } }

        private PQueue()
        {
            this.queueItemList = new List<QueueItem>();
            //XmlLoad();
            DBLoad();
        }
        public void DBLoad()
        {
            lock (KeyLock)
            {
                log.Info("-------------------> in DBLoad");
                //do not reload a not empty queue
                if (!queueItemList.Count.Equals(0))
                {
                    return;
                }
                var queueList = GTPQueue.GetQueue();
                int flag = 0;
                foreach (var queueDb in queueList) {
                    log.Info($"-------------------> 1.0 From db, Flag: {flag++}");
                    log.Info(queueDb.ObjectsIds);
                    log.Info(queueDb.Id);
                    log.Info(queueDb.ObjectIn);
                    //---------------------
                    var qItem = new QueueItem()
                    {
                        IdQueue = queueDb.Id,
                        FichierObjetIn = (string.IsNullOrEmpty(queueDb.ObjectIn)) ? string.Empty:queueDb.ObjectIn,
                        Etat = QueueStatesEnum.PasEncoreTraite,
                        InterfaceEnum = ContractEnum.Unknown,
                        Priority = 2.0f
                    };
                    Enqueue(qItem);
                }
                //string guid1 = "d63bf405-ec96-44c7-92b3-793f3e586147";
                //string guid2 = "751ee1bc-2b5b-47dc-829a-19d4a973c534";
                //string serializedRequest1 = Xml.Select(new Guid(guid1));
                ////Create and add items to the queue
                //TypeDetector.GetInterfaceTypeNameFromXml(serializedRequest1);
                //var qItem1 = new QueueItem()
                //{
                //    IdQueue = new Guid(guid1),
                //    FichierObjetIn = serializedRequest1,
                //    Etat = QueueStatesEnum.PasEncoreTraite,
                //    InterfaceEnum = ContractEnum.Unknown,
                //    Priority = 2.0f
                //};
                //string serializedRequest2 = Xml.Select(new Guid(guid2));
                //var qItem2 = new QueueItem()
                //{
                //    IdQueue = new Guid(guid2),
                //    FichierObjetIn = serializedRequest2,
                //    Etat = QueueStatesEnum.PasEncoreTraite,
                //    InterfaceEnum = ContractEnum.Unknown,
                //    Priority = 2.0f
                //};
                ////Enqueue items
                //Enqueue(qItem1);
                //Enqueue(qItem2);
            }
        }
        public void Enqueue(QueueItem item)
        {
            lock (KeyLock)
            {
                try
                {
                    if (GetElementByIdQueue(item.IdQueue) != null) return;//No duplicates.

                    queueItemList.Add(item);
                    int ci = queueItemList.Count - 1; // child index; start at end
                    while (ci > 0)
                    {
                        int pi = (ci - 1) / 2; // parent index
                        if (queueItemList[ci].CompareTo(queueItemList[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                        QueueItem tmp = queueItemList[ci]; queueItemList[ci] = queueItemList[pi]; queueItemList[pi] = tmp;
                        ci = pi;
                    }
                    log.Info($"PQueue, Element {item}, added");
                }
                catch (Exception exception) {
                    log.Error($"Error In PQueue -> 1.0: {exception.ToString()}");
                }
            }
        }
        public QueueItem Dequeue()
        {
            lock (KeyLock)
            {
                // assumes pq is not empty; up to calling code
                if (queueItemList.Count < 1) {
                    DBLoad();
                    if (queueItemList.Count < 1) {
                        return new QueueItem() { IsEmpty = true };
                    }
                }
                int li = queueItemList.Count - 1; // last index (before removal)
                QueueItem frontItem = queueItemList[0];   // fetch the front
                queueItemList[0] = queueItemList[li];
                queueItemList.RemoveAt(li);

                --li; // last index (after removal)
                int pi = 0; // parent index. start at front of pq
                while (true)
                {
                    int ci = pi * 2 + 1; // left child index of parent
                    if (ci > li) break;  // no children so done
                    int rc = ci + 1;     // right child
                    if (rc <= li && queueItemList[rc].CompareTo(queueItemList[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                        ci = rc;
                    if (queueItemList[pi].CompareTo(queueItemList[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                    QueueItem tmp = queueItemList[pi]; queueItemList[pi] = queueItemList[ci]; queueItemList[ci] = tmp; // swap parent and child
                    pi = ci;
                }
                return frontItem;   
            }            
        }   
        //public QueueItem Peek()
        //{
        //    lock (KeyLock)
        //    {
        //        QueueItem frontItem = queueItemList[0];
        //        return frontItem;
        //    }
        //}

        public int Count()
        {
            lock (KeyLock)
            {
                return queueItemList.Count;
            }
        }

        public override string ToString()
        {
            lock (KeyLock)
            {
                string s = "";
                for (int i = 0; i < queueItemList.Count; ++i)
                    s += queueItemList[i].ToString() + " ";
                s += "count = " + queueItemList.Count;
                return s;
            }
        }

        public bool IsConsistent()
        {
            lock (KeyLock)
            {
                // is the heap property true for all queueItemList?
                if (queueItemList.Count == 0) return true;
                int li = queueItemList.Count - 1; // last index
                for (int pi = 0; pi < queueItemList.Count; ++pi) // each parent index
                {
                    int lci = 2 * pi + 1; // left child index
                    int rci = 2 * pi + 2; // right child index

                    if (lci <= li && queueItemList[pi].CompareTo(queueItemList[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
                    if (rci <= li && queueItemList[pi].CompareTo(queueItemList[rci]) > 0) return false; // check the right child too.
                }
                return true; // passed all checks
            }
        } // IsConsistent
        //public bool ElementExists(Guid erpNumOprID) {
        //    return (GetElementByIdQueue(erpNumOprID) != null)? true : false;
        //}
        private QueueItem GetElementByIdQueue(Guid idQueue)
        {
            return queueItemList.Find(x=>x.IdQueue == idQueue);
        }
        public void ClearQueue() {
            lock (KeyLock)
            {
                queueItemList.Clear();
            }
        }
        public void TestLog(string message) {
            log.Info(message);
        }

    }
}
