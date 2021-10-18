using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;

namespace Egsp.Core
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Возвращает первый объект заданного типа. Может вернуть null.
        /// </summary>
        public static Option<T> FindType<T>(this IEnumerable source)
        {
            foreach (var obj in source)
            {
                if (obj is T casted)
                    return casted;
            }

            return Option<T>.None;
        }

        /// <summary>
        /// Возвращает все экземпляры заданного типа. Может вернуть пустой список.
        /// </summary>
        public static List<T> FindTypes<T>(this IEnumerable source)
        {    
            var coincidences = new List<T>();

            foreach (var obj in source)
            {
                if (obj is T casted)
                    coincidences.Add(casted);
            }

            return coincidences;
        }

        /// <summary>
        /// Возвращает индекс наибольшего элемента.
        /// </summary>
        public static Option<int> IndexOfMax<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            var maxIndex = -1;
            var maxValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (selector(value).CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            if(maxIndex == -1)
                return Option<int>.None;
            
            return maxIndex;
        }
        
        /// <summary>
        /// Возвращает индекс наименьшего элемента.
        /// </summary>
        public static Option<int> IndexOfMin<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            // Изменить Func - сделать сравнение в Func
            
            var minIndex = -1;
            var minValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (minValue == null)
                {
                    minValue = value;
                    minIndex = index;
                }
                else if (selector(value).CompareTo(selector(minValue)) < 0 || minIndex == -1)
                {
                    minIndex = index;
                    minValue = value;
                }
                index++;
            }
            if(minIndex == -1)
                return Option<int>.None;
            
            return minIndex;
        }

        private static int _seed;
        /// <summary>
        /// Возвращает случайное значение коллекции.
        /// </summary>
        public static T Random<T>(this IEnumerable<T> collection)
        {
            if (_seed == int.MaxValue)
                _seed = int.MinValue;

            _seed++;
            var list = collection.ToList();
            if (list.Count == 0)
                throw new ArgumentOutOfRangeException();
            
            var randomIndex = new System.Random(_seed).Next(0, list.Count);

            return list.ElementAt(randomIndex);
        }

        /// <summary>
        /// Проходит по всем элементам и выполняет действие.
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        /// <summary>
        /// Возвращает первый найденный объект или ничего. Удобно для использования со структурами, т.к. структуры не
        /// могут быть null.
        /// </summary>
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            ThrowIfNull(source);
            ThrowIfNull(predicate);
            
            foreach (T source1 in source)
            {
                if (predicate(source1))
                    return source1;
            }

            return Option<T>.None;
        }

        public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Func<T, bool> predicate,
            Action<T> applyAction)
        {
            ThrowIfNull(source);
            ThrowIfNull(predicate);
            ThrowIfNull(applyAction);
            
            foreach (var element in source)
            {
                if (predicate(element))
                {
                    applyAction(element);
                }
            }

            return source;
        }

        private static void ThrowIfNull(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
        }
    }
}