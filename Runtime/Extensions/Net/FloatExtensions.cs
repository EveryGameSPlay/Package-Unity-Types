
using System.Globalization;

namespace Egsp.Core
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Самый эффективный способ округления числа и перевода его в строку.
        /// </summary>
        /// <param name="f">Число.</param>
        /// <param name="digits">Количество знаков после запятой.</param>
        /// <returns></returns>
        public static string ToString(this float f, int digits)
        {
            return System.Math.Round(f, digits).ToString(CultureInfo.CurrentCulture);
        }

        public static float Round(this float f, int digits = 1)
            => (float) System.Math.Round(f, digits);

        /// <summary>
        /// Возвращает число относительно промежутка (min--max) (от нуля до единицы).
        /// </summary>
        public static float Normalize(this float f, float min, float max)
        {
            if (min >= max)
                return 1;

            var normalized = (f - min) / (max - min);

            return normalized;
        }
    }
}