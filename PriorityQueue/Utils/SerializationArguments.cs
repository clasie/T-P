using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue.Utils
{
    public class SerializationArguments
    {
        public int SerializationMode {get;set;}
        public object ListOfElements { get; set; }
        public string ElementToDeserialize { get; set; }
    }
}
