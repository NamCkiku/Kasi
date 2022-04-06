namespace Kasi_Server.Utils.Extensions
{
    public static class RandomExtensions
    {
        public static long NextLong(this Random random) => random.NextLong(0, long.MaxValue);

        public static long NextLong(this Random random, long max) => random.NextLong(0, max);

        public static long NextLong(this Random random, long min, long max)
        {
            var buf = new byte[8];
            random.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);
            return Math.Abs(longRand % (max - min)) + min;
        }

        public static double NextDouble(this Random random, double max) => random.NextDouble() * max;

        public static double NextDouble(this Random random, double min, double max) =>
            random.NextDouble() * (max - min) + min;

        public static double NormalDouble(this Random random)
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();
            return Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
        }

        public static double NormalDouble(this Random random, double mean, double deviation) =>
            mean + deviation * random.NormalDouble();

        public static float NextFloat(this Random random) => (float)random.NextDouble();

        public static float NextFloat(this Random random, float max) => (float)(random.NextDouble() * max);

        public static float NextFloat(this Random random, float min, float max) =>
            (float)(random.NextDouble() * (max - min) + min);

        public static float NormalFloat(this Random random) => (float)random.NormalDouble();

        public static float NormalFloat(this Random random, float mean, float deviation) =>
            mean + (float)(deviation * random.NormalDouble());

        public static int NextSign(this Random random) => 2 * random.Next(2) - 1;

        public static bool NextBool(this Random random) => random.NextDouble() < 0.5;

        public static bool NextBool(this Random random, double probability) => random.NextDouble() < probability;
    }
}