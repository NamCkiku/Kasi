using Kasi_Server.Utils.IdGenerators.Abstractions;
using Kasi_Server.Utils.IdGenerators.Ids;

namespace Kasi_Server.Utils.IdGenerators.Core
{
    public class ObjectIdGenerator : IStringGenerator
    {
        public static ObjectIdGenerator Current { get; } = new ObjectIdGenerator();

        public string Create()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}