using UnityEngine;

namespace _1_Game.Scripts.Util
{
    public static class ArrayExtention
    {
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]); // Swap elements
            }
        }
    }
}