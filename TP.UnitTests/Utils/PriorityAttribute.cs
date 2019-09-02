using System;

namespace TP.UnitTests.Utils
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}