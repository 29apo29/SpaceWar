using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3
{
    internal class ArrayUtils
    {
        public static bool contains(int[,] arr, int[] val)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                bool result = true;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] != val[j])
                    {
                        result = false;
                        break;
                    }
                }
                if (result) return true;
            }
            return false;
        }
        public static T[] removeLastItem<T>(T[] arr)
        {
            T[] result = new T[arr.Length - 1];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                result[i] = arr[i];
            }
            return result;
        }

        public static T[] add<T>(T[] arr, T val)
        {
            int len = arr.Length;
            T[] newArr = new T[len + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }
            newArr[len] = val;
            return newArr;
        }

        public static T[] addWithLocation<T>(T[] arr, T val, int location)
        {
            if (arr.Length < 10) arr = toCopy(arr, new T[arr.Length + 1]);
            for (int i = (arr.Length - 1); i >= 0; i--)
            {
                if (i > location)
                {
                    arr[i] = arr[i - 1];
                }
            }
            arr[location] = val;
            return arr;
        }
        public static T[] toCopy<T>(T[] old, T[] newer)
        {
            for (int i = 0; i < old.Length; i++)
            {
                newer[i] = old[i];
            }
            return newer;
        }
    }
}
