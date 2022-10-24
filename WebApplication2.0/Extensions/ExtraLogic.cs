using NewTextreader;
using WebApplication2._0.Models;

namespace WebApplication2._0.Extensions
{
    internal static class ExtraLogic
    {
        public static bool Remain(this RecordModel line)
        {
            return line.Number % 4 == 3;
        }
    }
}