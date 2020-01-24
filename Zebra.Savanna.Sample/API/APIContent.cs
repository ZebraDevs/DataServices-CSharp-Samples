using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zebra.Savanna.Sample.API
{
    /// <summary>
    /// Helper class for providing API content for user interfaces.
    /// </summary>
    [Serializable]
    public class APIContent : Dictionary<string, ApiItem>
    {
        public APIContent() { }

        protected APIContent(SerializationInfo serializationInfo, StreamingContext streamingContext) { }

        public void AddItem(string content, string details, int icon)
        {
            var item = new ApiItem((Count + 1).ToString(), content, details, icon);
            this[item.Id] = item;
        }
    }
}