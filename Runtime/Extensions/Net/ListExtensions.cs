using System.Collections;
using System.Collections.Generic;

namespace Egsp.Core
{
    public static class ListExtensions
    {
        /// <summary>
        /// Убирает все элементы списка являющиеся типом T.
        /// </summary>
        public static void RemoveType<T>(this IList source)
        {
            for (var i = source.Count - 1; i > -1; i--)
            {
                if(source[i] is T)
                    source.RemoveAt(i);
            }
        }
        
        public static LinkedList<T> ToLinked<T>(this List<T> list)
        {
            var linkedList = new LinkedList<T>();

            for (var i = 0; i < list.Count; i++)
            {
                linkedList.AddLast(list[i]);
            }

            return linkedList;
        }
    }
}