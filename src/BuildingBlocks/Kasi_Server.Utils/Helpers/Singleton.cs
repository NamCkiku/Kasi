namespace Kasi_Server.Utils.Helpers
{
    public class Singleton
    {
        public static IDictionary<Type, object> AllSingletons { get; }

        static Singleton()
        {
            if (AllSingletons == null)
            {
                AllSingletons = new Dictionary<Type, object>();
            }
        }
    }

    public class Singleton<T> : Singleton
    {
        private static T _instance;

        public static T Instance
        {
            get => _instance;
            set
            {
                _instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
}