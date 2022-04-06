namespace Kasi_Server.Utils.Date.Chinese
{
    public static class ChineseAnimalHelper
    {
        private static readonly string[] Animals = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        private static readonly string[] Animalz = { "鼠", "牛", "虎", "兔", "龍", "蛇", "馬", "羊", "猴", "雞", "狗", "豬" };

        private const int AnimalStartYear = 1900;

        private static int Index(DateTime dt)
        {
            var offset = dt.Year - AnimalStartYear;
            return offset % 12;
        }

        public static string Get(DateTime dt, bool traditionalChineseCharacter = false)
        {
            var animals = traditionalChineseCharacter ? Animalz : Animals;
            return animals[Index(dt)];
        }
    }
}