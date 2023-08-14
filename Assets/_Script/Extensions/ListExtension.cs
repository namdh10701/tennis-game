using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace ListExtensions
{
    public static class ListExtensions
    {
        public static void AddFirst<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
        }

        public static bool GetFirst<T>(this List<T> list, out T item)
        {
            if (list.Count > 0)
            {
                item = list[0];
                list.RemoveAt(0);
                return true;
            }
            else
            {
                item = default(T);
                return false;
            }
        }

        public static void AddLast<T>(this List<T> list, T item)
        {
            list.Add(item);
        }

        public static bool GetLast<T>(this List<T> list, out T item)
        {
            if (list.Count > 0)
            {
                int lastIndex = list.Count - 1;
                item = list[lastIndex];
                list.RemoveAt(lastIndex);
                return true;
            }
            else
            {
                item = default(T);
                return false;
            }
        }
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
            return default(T);
        }
    }
}