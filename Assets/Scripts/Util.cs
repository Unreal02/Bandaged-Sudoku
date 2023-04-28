using System.Linq;
using UnityEngine;

public static class Util
{
    public static void Shuffle<T>(this T[] array)
    {
        for (var i = 0; i < array.Length; i++)
        {
            var p1 = Random.Range(0, array.Count());
            var p2 = Random.Range(0, array.Count());
            (array[p1], array[p2]) = (array[p2], array[p1]);
        }
    }
}