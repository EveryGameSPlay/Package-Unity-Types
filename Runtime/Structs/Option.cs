using System;

namespace Egsp.Core
{
    /// <summary>
    /// <para>Данная структура является аналогом option из языка F#.
    /// Позволяет вернуть результат или ничего.</para>
    /// <para>Данный тип сигнализирует о том, что результат может вернуться в двух вариантах. 
    /// Варианты: Some и None.</para>
    /// </summary>
    public readonly struct Option<TValue>
    {
        private static Option<TValue> _none = new Option<TValue>();
        public static ref readonly Option<TValue> None => ref _none;

        private readonly TValue _option;
        
        /// <summary>
        /// Имеется ли значение.
        /// </summary>
        public readonly bool IsSome;

        /// <summary>
        /// Отсутствует ли значение.
        /// </summary>
        public bool IsNone => !IsSome;

        /// <summary>
        /// Вложенное значение.
        /// </summary>
        public TValue option => _option;

        public Option(TValue option)
        {
            _option = option;
            IsSome = option != null;
        }

        public static implicit operator TValue(Option<TValue> option)
        {
            return option.option;
        }
        
        public static implicit operator Option<TValue>(TValue value)
        {
            return new Option<TValue>(value);
        }

        public static implicit operator bool(Option<TValue> option)
        {
            return option.IsSome;
        }

        public override bool Equals(object obj)
        {
            if (obj is Option<TValue>)
                return this.Equals((Option<TValue>)obj);
            else
                return false;
        }

        public bool Equals(Option<TValue> other)
        {
            if (this.IsSome && other.IsSome)
                return object.Equals(_option, other._option);
            else
                return IsNone == other.IsNone;
        }

        public override int GetHashCode()
        {
            return !this ? -1 : _option.GetHashCode();
        }

        public override string ToString()
        {
            return this ? $"Some: {option.ToString()}" : "None";
        }
    }
}