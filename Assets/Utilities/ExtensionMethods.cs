using System.Collections;
using System.Collections.Generic;
using System;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        private static Random rando = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                i--;
                int j = rando.Next(i + 1);
                T value = list[j];
                list[j] = list[i];
                list[i] = value;
            }
        }
    }
}