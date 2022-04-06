using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils.Randoms
{
    public class GuidRandomGenerator : IRandomGenerator
    {
        public string Generate()
        {
            return Id.Guid;
        }

        public static readonly IRandomGenerator Instance = new GuidRandomGenerator();
    }
}