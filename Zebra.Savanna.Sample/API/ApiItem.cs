namespace Zebra.Savanna.Sample.API
{
    /// <summary>
    /// An item representing a piece of API content.
    /// </summary>
    public class ApiItem : Java.Lang.Object
    {
        public readonly string Id;
        public readonly string Content;
        public readonly string Details;
        public readonly int Icon;

        public ApiItem(string id, string content, string details, int icon)
        {
            Id = id;
            Content = content;
            Details = details;
            Icon = icon;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}