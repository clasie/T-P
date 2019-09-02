using System;

namespace TokenHandler.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Attribute    : Web Service base URL.
    /// </summary>
    public class ServiceRequestAttribute:Attribute 
    {
        /// <summary>
        /// Service URL
        /// </summary>
        public string Url { get; set; }
    }
}
