using Newtonsoft.Json;

namespace Kasi_Server.Utils
{
    public class Item : IComparable<Item>
    {
        public Item()
        { }

        public Item(string text, object value, int? sortId = null, string group = null, bool? disabled = null)
        {
            Text = text;
            Value = value;
            SortId = sortId;
            Group = group;
            Disabled = disabled;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SortId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Disabled { get; set; }

        public int CompareTo(Item other) => string.Compare(Text, other.Text, StringComparison.CurrentCulture);
    }
}