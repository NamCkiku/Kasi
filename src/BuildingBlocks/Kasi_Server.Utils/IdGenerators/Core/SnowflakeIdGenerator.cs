using Kasi_Server.Utils.IdGenerators.Abstractions;
using Kasi_Server.Utils.IdGenerators.Ids;

namespace Kasi_Server.Utils.IdGenerators.Core
{
    public class SnowflakeIdGenerator : ILongGenerator
    {
        private readonly SnowflakeId _id = new SnowflakeId(1, 1);

        public static SnowflakeIdGenerator Current { get; } = new SnowflakeIdGenerator();

        public long Create()
        {
            return _id.NextId();
        }
    }
}