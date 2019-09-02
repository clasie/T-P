using PriorityQueue.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue.Model
{
    /// <summary>
    /// This class represents a Queue item.
    /// </summary>
    public class QueueItem : IComparable<QueueItem>
    {
        #region fields
        private Guid idQueue;
        private Guid d365NumOprID;
        private Guid eRPNumOprID;
        private int interfaceID;
        private ContractEnum interfaceEnum;
        private int methodeID;
        private string fichierObjetIn;
        private string fichierObjetOut;
        private QueueStatesEnum etat;
        private string listeDesIdsAzureSeparesParUnPipe;
        //public string StatutDemande;
        private string tpInterfaceId;
        private double priority; // smaller values are higher Priority 1.0 -> 10.0
        private string message;
        private string erreur;
        private bool isEmpty;
        #endregion

        #region pub accessors fields
        public Guid IdQueue
        {
            get
            {
                return idQueue;
            }

            set
            {
                idQueue = value;
            }
        }

        public Guid D365NumOprID
        {
            get
            {
                return d365NumOprID;
            }

            set
            {
                d365NumOprID = value;
            }
        }

        public Guid ERPNumOprID
        {
            get
            {
                return eRPNumOprID;
            }

            set
            {
                eRPNumOprID = value;
            }
        }

        public string TPInterfaceID
        {
            get
            {
                return tpInterfaceId;
            }

            set
            {
                tpInterfaceId = value;
            }
        }

        public int MethodeID
        {
            get
            {
                return methodeID;
            }

            set
            {
                methodeID = value;
            }
        }

        public string FichierObjetIn
        {
            get
            {
                return fichierObjetIn;
            }

            set
            {
                fichierObjetIn = value;
            }
        }

        public string FichierObjetOut
        {
            get
            {
                return fichierObjetOut;
            }

            set
            {
                fichierObjetOut = value;
            }
        }

        public QueueStatesEnum Etat
        {
            get
            {
                return etat;
            }

            set
            {
                etat = value;
            }
        }

        public string ListeDesIdsAzureSeparesParUnPipe
        {
            get
            {
                return listeDesIdsAzureSeparesParUnPipe;
            }

            set
            {
                listeDesIdsAzureSeparesParUnPipe = value;
            }
        }

        public string TPInterfaceId
        {
            get
            {
                return tpInterfaceId;
            }

            set
            {
                tpInterfaceId = value;
            }
        }

        public double Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
            }
        }

        public ContractEnum InterfaceEnum
        {
            get
            {
                return interfaceEnum;
            }

            set
            {
                interfaceEnum = value;
            }
        }

        public string Erreur
        {
            get
            {
                return erreur;
            }

            set
            {
                erreur = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }

            set
            {
                isEmpty = value;
            }
        }
        #endregion 

        #region Methods
        public QueueItem() { }
        /// <summary>
        /// A way to debug a QueuItem when necessary.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var prop in this.GetType().GetProperties())
            {
                sb.AppendLine($"{prop.Name} {prop.GetValue(this, null)}");
            }

            return sb.ToString();// $" (TPInterfaceId: {TPInterfaceId} ERPNumOprID: {eRPNumOprID} Priority: {Priority} Etat: {etat}"; // FichierObjetIn: {fichierObjetIn} ";
        }
        /// <summary>
        /// Used to store QueuItems inside the Queue by priorities when we Dequeue().
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(QueueItem other)
        {
            if (this.Priority < other.Priority) return -1;
            else if (this.Priority > other.Priority) return 1;
            else return 0;
        }
        #endregion
    }
}
