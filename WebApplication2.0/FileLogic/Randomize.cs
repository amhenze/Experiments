using WebApplication2._0.Interfaces;

namespace NewTextreader
{
    public class Randomize : IRandomize
    {
        public int GetRandomInt(int total, int min, int max) // рандомизатор с сидом от времени
        {
            int time = Convert.ToInt32(DateTime.Now.Ticks % 100000);
            Random random = new Random(time);
            return random.Next(min, max);
        }

        public string GetRandomString()
        {
            Char[] pwdChars = new Char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            string text = "";
            for (int i = 0; i < GetRandomInt(1, 2, 20); i++)
                text += pwdChars[GetRandomInt(1, 0, 25)];
            return text;
        }

    }
}
