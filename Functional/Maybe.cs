using System;
using System.Threading.Tasks;

namespace Functional
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
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
        public static bool operator ==(Maybe<T> a, Maybe<T> b) => a.Equals(b);
        public static bool operator !=(Maybe<T> a, Maybe<T> b) => !a.Equals(b);
        public override int GetHashCode() => (!HasValue) ? 0 : Value.GetHashCode();
        public bool Equals(Maybe<T> other) => HasValue == other.HasValue && (!HasValue || value.Equals(other.value));

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(Maybe<T>)) return false;
            return Equals((Maybe<T>)other);
        }
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

        public static Maybe<T2> SelectMany<T, T1, T2>(this Maybe<T> self, Func<T, Maybe<T1>> selector, Func<T, T1, T2> mapper) =>
            self.Match(
                x => selector(x).Match(
                    y => Maybe.Just(mapper(x, y)),
                    () => Maybe.Nothing
                ),
                () => Maybe.Nothing
            );

        public static Either<Unit, T> ToEither<T>(this Maybe<T> self) => self.Match(x => Either.New<Unit, T>(x), () => Unit.Default);
    }


    public static class AsyncMaybeExt
    {
        public static async Task<TResult> Match<T, TResult>(this Task<Maybe<T>> self, Func<T, TResult> left, Func<TResult> right) =>
            (await self).Match(left, right);

        public static async void Match<T>(this Task<Maybe<T>> self, Action<T> left, Action right) =>
            (await self).Match(left, right);

        public static async Task<TResult> MatchAsync<T, TResult>(this Task<Maybe<T>> self, Func<T, Task<TResult>> left, Func<Task<TResult>> right) =>
            await (await self).Match(left, right);

        public static async void MatchAsync<T>(this Task<Maybe<T>> self, Func<T, Task> left, Func<Task> right) =>
            await (await self).Match(left, right);

        public static async Task<Maybe<T1>> Select<T, T1>(this Task<Maybe<T>> self, Func<T, T1> selector)
        {
            var ret = await self;
            return ret.HasValue? Maybe.Just(selector(ret.Value)) : Maybe.Nothing;
        }
            

        public static async Task<Maybe<T1>> SelectMany<T, T1>(this Task<Maybe<T>> self, Func<T, Task<Maybe<T1>>> selector)
        {
            var ret = await self;
            return ret.HasValue? await selector(ret.Value) : Maybe.Nothing;
        }

        public static async Task<Maybe<T2>> SelectMany<T, T1, T2>(this Task<Maybe<T>> self, Func<T, Task<Maybe<T1>>> selector, Func<T, T1, T2> mapper)
        {
            var ret = await self;
            return await ret.Match(
                async x => (await selector(x)).Match(
                    y => Maybe.Just(mapper(x, y)),
                    () => Maybe.Nothing
                ),
                () => Task.FromResult<Maybe<T2>>(Maybe.Nothing)
            );
        }
    }
}
