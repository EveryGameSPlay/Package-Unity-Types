namespace Egsp.Core
{
    public static class IntExtensions
    {
        public static float Normalize(this int i, float min, float max)
        {
            if (min >= max)
                return 1;

            var normalized = (i - min) / (max - min);

            return normalized;
        }
    }
}