using System;

namespace Functional
{
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        private T value;
        public T Value
        {
            get
            {
                if (HasValue) return value;
                throw new ArgumentNullException();
            }
        }
        
        public bool HasValue { get; private set; }

        internal Maybe(T value)
        {
            this.value = value;
            this.HasValue = true;
        }
        internal Maybe()
        {
            this.HasValue = false;
        }

        public bool Equals(Maybe<T> other) =>
            HasValue == other.HasValue && (!HasValue || value.Equals(other.value));

        public TResult Match<TResult>(Func<T, TResult> left, Func<TResult> right) =>
            HasValue ? left(Value) : right();

        public void Match(Action<T> left, Action right)
        {
            if (HasValue)
                left(Value);
            else
                right();
        }

        public static implicit operator Maybe<T>(Maybe<Unit> value) => new Maybe<T>();
    }

    public static class Maybe
    {
        public static Maybe<T> Just<T>(T value) => new Maybe<T>(value);
        public static readonly Maybe<Unit> Nothing = new Maybe<Unit>();
    }

    public static class MaybeExt
    {
        public static Maybe<T1> Select<T, T1>(this Maybe<T> self, Func<T, T1> selector) =>
            self.HasValue ? Maybe.Just(selector(self.Value)) : Maybe.Nothing;

        public static Maybe<T1> SelectMany<T, T1>(this Maybe<T> self, Func<T, Maybe<T1>> selector) =>
            self.HasValue ? selector(self.Value) : Maybe.Nothing;

        public static Maybe<T2> SelectMany<T, T1, T2>(this Maybe<T> self, Func<T, Maybe<T1>> selector, Func<T, T1, T2> mapper)
        {
            var t1 = self.SelectMany(selector);
            return t1.HasValue ? Maybe.Just(mapper(self.Value, t1.Value)) : Maybe.Nothing;
        }
    }
}
