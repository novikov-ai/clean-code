using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CleanCode.Comments.Engineering
{
    // 3.1 (2)
    /// <summary>
    /// Blocked parameters are read only for user when NewTypeWindow is invoked
    /// </summary>
    public class AutoColumnBlockedParams
    {
        public const string ColumnParamWidthName = "Ширина колонны";
        public const string ColumnParamLengthName = "Длинна колонны";
        public const string ColumnParamConcreteClassName = "Класс бетона";

        public readonly double Width;
        public readonly double Length;
        public readonly double ConcreteClass;

        public AutoColumnBlockedParams(double width, double length, int concreteClass)
        {
            Width = width;
            Length = length;
            ConcreteClass = concreteClass;
        }

        public static List<string> GetBlockedParamsNames()
        {
            var fieldInfo = typeof(AutoColumnBlockedParams).GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfo
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Select(fi => fi.GetValue(null).ToString())
                .ToList();
        }
    }
}