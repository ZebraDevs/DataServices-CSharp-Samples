using System.Collections.Generic;

namespace Zebra.Savanna.Sample.API
{
    /// <summary>
    /// Helper class for providing API content for user interfaces.
    /// </summary>
    public static class APIContent
    {
        /// <summary>
        /// A map of API items, by ID.
        /// </summary>
        public static readonly Dictionary<string, ApiItem> Items = new Dictionary<string, ApiItem>();

        public static void AddItem(ApiItem item)
        {
            Items[item.Id] = item;
        }
    }

    /// <summary>
    /// An item representing a piece of API content.
    /// </summary>
    public class ApiItem : Java.Lang.Object
    {
        public readonly string Id;
        public readonly string Content;
        public readonly string Details;

        public ApiItem(string id, string content, string details)
        {
            Id = id;
            Content = content;
            Details = details;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}