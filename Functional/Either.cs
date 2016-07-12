using System;
using System.Threading.Tasks;

namespace Functional
{
    public sealed class Either<TLeft, TRight>
    {
        private bool? leftOrRight;
        private TLeft left;
        public TLeft Left
        {
            get
            {
                if (!leftOrRight.HasValue || !leftOrRight.Value) throw new ArgumentNullException();
                return left;
            }
        }

        private TRight right;
        public TRight Right
        {
            get
            {
                if (!leftOrRight.HasValue || leftOrRight.Value) throw new ArgumentNullException();
                return right;
            }
        }

        public bool IsLeft
        {
            get
            {
                if (!leftOrRight.HasValue) throw new ArgumentNullException();
                return leftOrRight.Value;
            }
        }

        public bool IsRight
        {
            get
            {
                if (!leftOrRight.HasValue) throw new ArgumentNullException();
                return !leftOrRight.Value;
            }
        }

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
        {
            return IsLeft ? leftFunc(left) : rightFunc(right);
        }

        public void Match(Action<TLeft> leftAction, Action<TRight> rightAction)
        {
            if (IsLeft)
                leftAction(left);
            else
                rightAction(right);
        }

        internal Either(TLeft value)
        {
            this.leftOrRight = true;
            this.left = value;
        }
        internal Either(TRight value)
        {
            this.leftOrRight = false;
            this.right = value;
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left)
        {
            return new Either<TLeft, TRight>(left);
        }

        public static implicit operator Either<TLeft, TRight>(TRight right)
        {
            return new Either<TLeft, TRight>(right);
        }

        public static Either<TLeft, TRight> New(TLeft value)
        {
            return new Either<TLeft, TRight>(value);
        }
        public static Either<TLeft, TRight> New(TRight value)
        {
            return new Either<TLeft, TRight>(value);
        }
    }

    public static class Either
    {
        public static Either<TLeft, TRight> New<TLeft, TRight>(TLeft value)
        {
            return new Either<TLeft, TRight>(value);
        }
        public static Either<TLeft, TRight> New<TLeft, TRight>(TRight value)
        {
            return new Either<TLeft, TRight>(value);
        }
    }

    public static class EitherExt
    {
        public static Either<TLeftNew, TRight> Select<TLeft, TLeftNew, TRight>(this Either<TLeft, TRight> self, Func<TLeft, TLeftNew> mapper) =>
            self.Match<Either<TLeftNew, TRight>>(
                x => mapper(x),
                x => x
            );

        public static Either<T1, TRight> SelectMany<T, TRight, T1>(this Either<T, TRight> self, Func<T, Either<T1, TRight>> selector) =>
            self.Match(
                x => selector(x),
                x => x
            );

        public static Either<T2, TRight> SelectMany<T, TRight, T1, T2>(this Either<T, TRight> self, Func<T, Either<T1, TRight>> selector, Func<T, T1, T2> mapper) =>
            self.Match(
                x => selector(x).Match< Either<T2, TRight>>(
                    y => mapper(x, y),
                    z => z
                ),
                x => x
            );

        public static Either<TRight, TLeft> Swap<TLeft, TRight>(this Either<TLeft, TRight> self) =>
            self.Match(
                x => Either<TRight, TLeft>.New(x),
                x => Either<TRight, TLeft>.New(x)
            );

        public static Maybe<T> ToMaybe<T>(this Either<Unit, T> self) => self.Match(_ => Maybe.Nothing, x => Maybe.Just(x));

        public static Either<T2, TRight2> bmap<T, TRight, T2, TRight2>(this Either<T, TRight> self, Func<T, T2> mapLeft, Func<TRight, TRight2> mapRight) =>
            self.Match<Either<T2, TRight2>>(
                x => mapLeft(x),
                x => mapRight(x)
            );
    }


    public static class AsyncEitherExt
    {
        public async static Task<T> Match<TLeft, TRight, T>(this Task<Either<TLeft, TRight>> self, Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc) =>
            (await self).Match(leftFunc, rightFunc);

        public async static Task<T> MatchAsync<TLeft, TRight, T>(this Task<Either<TLeft, TRight>> self, Func<TLeft, Task<T>> leftFunc, Func<TRight, Task<T>> rightFunc) =>
            await (await self).Match(leftFunc, rightFunc);

        public async static void Match<TLeft, TRight>(this Task<Either<TLeft, TRight>> self, Action<TLeft> leftAction, Action<TRight> rightAction) =>
            (await self).Match(leftAction, rightAction);

        public async static void MatchAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> self, Func<TLeft, Task> leftAction, Func<TRight, Task> rightAction) =>
            await (await self).Match(leftAction, rightAction);

        public async static Task<Either<TLeftNew, TRight>> Select<TLeft, TLeftNew, TRight>(this Task<Either<TLeft, TRight>> self, Func<TLeft, TLeftNew> mapper) =>
            (await self).Match<Either<TLeftNew, TRight>>(
                x => mapper(x),
                x => x
            );

        public async static Task<Either<T1, TRight>> SelectMany<T, TRight, T1>(this Task<Either<T, TRight>> self, Func<T, Task<Either<T1, TRight>>> selector) =>
            await (await self).Match(
                x => selector(x),
                x => Task.FromResult<Either<T1, TRight>>(x)
            );

        public async static Task<Either<T2, TRight>> SelectMany<T, TRight, T1, T2>(this Task<Either<T, TRight>> self, Func<T, Task<Either<T1, TRight>>> selector, Func<T, T1, T2> mapper) =>
            await (await self).Match(
                async x => (await selector(x)).Match<Either<T2, TRight>>(
                    y => mapper(x, y),
                    z => z
                ),
                x => Task.FromResult<Either<T2, TRight>>(x)
            );
    }
    }
