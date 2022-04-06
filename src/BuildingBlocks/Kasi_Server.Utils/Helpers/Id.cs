using Kasi_Server.Utils.IdGenerators.Abstractions;
using Kasi_Server.Utils.IdGenerators.Core;

namespace Kasi_Server.Utils.Helpers
{
    public static class Id
    {
        private static string _id;

        private static IGuidGenerator GuidGenerator { get; set; } = SequentialGuidGenerator.Current;

        private static ILongGenerator LongGenerator { get; set; } = SnowflakeIdGenerator.Current;

        private static IStringGenerator StringGenerator { get; set; } = TimestampIdGenerator.Current;

        private static IStringGenerator ObjectGenerator { get; set; } = ObjectIdGenerator.Current;

        public static void SetId(string id) => _id = id;

        public static void Reset() => _id = null;

        public static string Guid => string.IsNullOrWhiteSpace(_id) ? System.Guid.NewGuid().ToString("N") : _id;

        public static long SnowflakeId => LongGenerator.Create();

        public static string TimestampId => StringGenerator.Create();

        public static string ObjectId => ObjectGenerator.Create();

        public static class Sequential
        {
            public static Guid Binary => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsBinary);

            public static Guid String => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsString);

            public static Guid End => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAtEnd);
        }

        public static class SequentialString
        {
            public static string Binary => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsBinary).ToString("N");

            public static string String => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsString).ToString("N");

            public static string End => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAtEnd).ToString("N");
        }
    }
}