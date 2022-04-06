namespace Kasi_Server.Utils
{
    public class KeyValue : KeyValue<int, string>
    {
        public KeyValue()
        { }

        public KeyValue(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class KeyValue<TValue> : KeyValue<int, TValue>
    {
        public KeyValue()
        { }

        public KeyValue(int key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public class KeyValue<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public KeyValue()
        { }

        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}