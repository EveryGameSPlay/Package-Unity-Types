using UnityEngine;

namespace Egsp.Core
{
    public static class ColorExtensions
    {
        public static Color Random()
        {
            return new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
        }
    }
}