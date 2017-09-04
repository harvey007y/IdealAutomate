using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// test
namespace IdealAutomate.Core
{
    public static class ArrayExtensions
    {
        public static Array ResizeArray(this Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
            {
                throw new ArgumentException("arr must have the same number of dimensions as there are elements in newSizes", "newSizes");
            }

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            var sizesToCopy = new int[newSizes.Length];
            for (var i = 0; i < sizesToCopy.Length; i++)
            {
                sizesToCopy[i] = Math.Min(newSizes[i], arr.GetLength(i));
            }

            var currentPositions = new int[sizesToCopy.Length];
            CopyArray(arr, temp, sizesToCopy, currentPositions, 0);

            return temp;
        }

        private static void CopyArray(Array arr, Array temp, int[] sizesToCopy, int[] currentPositions, int dimmension)
        {
            if (arr.Rank - 1 == dimmension)
            {
                //Copy this Array
                for (var i = 0; i < sizesToCopy[dimmension]; i++)
                {
                    currentPositions[dimmension] = i;
                    temp.SetValue(arr.GetValue(currentPositions), currentPositions);
                }
            }
            else
            {
                //Recursion one dimmension higher
                for (var i = 0; i < sizesToCopy[dimmension]; i++)
                {
                    currentPositions[dimmension] = i;
                    CopyArray(arr, temp, sizesToCopy, currentPositions, dimmension + 1);
                }
            }
        }
    }
}
