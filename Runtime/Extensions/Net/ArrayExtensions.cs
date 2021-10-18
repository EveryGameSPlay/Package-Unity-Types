namespace Egsp.Core
{
    public static class ArrayExtensions
    {
        public static TValue Random<TValue>(this TValue[] array)
        {
            if (array.Length == 0)
            {
                return default;
            }
            
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
    }
}