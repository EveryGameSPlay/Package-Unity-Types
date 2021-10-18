using System;
using System.Collections.Generic;
using Egsp.Core;

namespace Egsp.Core
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Присоединяет другой список в конец текущего.
        /// </summary>
        public static LinkedList<T> Join<T>(this LinkedList<T> source, LinkedList<T> another)
        {
            foreach (var value in another)
            {
                source.AddLast(value);
            }

            return source;
        }

        /// <summary>
        /// Убирает все подходящие элементы из списка. Возвращает ссылку на оригинальный список.
        /// </summary>
        public static LinkedList<T> RemoveAll<T>(this LinkedList<T> source, Func<T, bool> predicate)
        {
            var node = source.First;
            while (node != null)
            {
                var next = node.Next;
                if (predicate(node.Value))
                {
                    source.Remove(node);
                }

                node = next;
            }

            return source;
        }

        /// <summary>
        /// Возвращает первый найденный объект или ничего. Удобно для использования со структурами, т.к. структуры не
        /// могут быть null.
        /// </summary>
        public static Option<LinkedListNode<T>> FirstNodeOrNone<T>(this LinkedList<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof (source));
            if (predicate == null)
                throw new ArgumentNullException(nameof (predicate));

            var node = source.First;
            if(node == null)
                return Option<LinkedListNode<T>>.None;

            while (node != null)
            {
                if (predicate(node.Value))
                    return node;

                node = node.Next;
            }
            
            return Option<LinkedListNode<T>>.None;
        }
    }
}