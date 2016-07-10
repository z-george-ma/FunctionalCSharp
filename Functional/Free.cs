using System;

namespace Functional
{
    public interface Free<TCommand> : IUnion { }
    public interface Free<TCommand, out TResult> : Free<TCommand>
        where TResult : class // value type does not support covariance
    { }
    public interface IPoint<TCommand, out TResult> : Free<TCommand, TResult>
        where TResult : class // value type does not support covariance
    {
        TResult Value { get; }
    }

    public delegate Free<TCommand, TResult> Next<TCommand, in TInput, out TResult>(TInput input)
        where TInput : class // value type does not support contravariance
        where TResult : class; // value type does not support covariance

    public interface IJoin<TCommand, out TResult> : Free<TCommand, TResult>
        where TResult : class // value type does not support covariance
    {
        TCommand Command { get; }
        Next<TCommand, object, TResult> Next { get; }
    }

    public sealed class Point<TCommand, T> : IPoint<TCommand, T>
        where T : class
    {
        public T Value { get; private set; }
        internal Point(T value)
        {
            this.Value = value;
        }
    }

    public sealed class Join<TCommand, TResult> : IJoin<TCommand, TResult>
        where TResult : class // value type does not support covariance
    {
        public TCommand Command { get; private set; }
        public Next<TCommand, object, TResult> Next { get; private set; }
        internal Join(TCommand command, Next<TCommand, object, TResult> next)
        {
            this.Command = command;
            this.Next = next;
        }
    }
    public class Free
    {
        public static Free<TCommand, T> Point<TCommand, T>(T value)
            where T : class 
            => new Point<TCommand, T>(value);
        public static Free<TCommand, TResult> Join<TCommand, TResult>(TCommand command, Next<TCommand, object, TResult> next)
            where TResult : class
            => new Join<TCommand, TResult>(command, next);
        public static Free<TCommand, T> Join<TCommand, T>(TCommand command)
            where T : class 
            => new Join<TCommand, T>(command, x => Point<TCommand, T>((T)x));
        public static Free<TCommand, T> Join<TCommand, T>(TCommand command, T value)
            where T : class 
            => new Join<TCommand, T>(command, _ => Point<TCommand, T>(value));
    }
    public static class FreeExt
    {
        public static Free<TCommand, T1> Select<TCommand, T, T1>(this Free<TCommand, T> self, Func<T, T1> selector)
            where T : class
            where T1 : class
            => self.Match(
                (IJoin<TCommand, T> x) => Free.Join(x.Command, r => Select(x.Next(r), selector)),
                (IPoint<TCommand, T> x) => Free.Point<TCommand, T1>(selector(x.Value)),
                _ => null);

        public static Free<TCommand, T1> SelectMany<TCommand, T, T1>(this Free<TCommand, T> self, Func<T, Free<TCommand, T1>> selector)
            where T: class
            where T1 : class
            => self.Match(
                (IJoin<TCommand, T> x) => Free.Join(x.Command, r => SelectMany(x.Next(r), selector)),
                (IPoint<TCommand, T> x) => selector(x.Value),
                _ => null);

        public static Free<TCommand, T2> SelectMany<TCommand, T, T1, T2>(this Free<TCommand, T> self, Func<T, Free<TCommand, T1>> selector, Func<T, T1, T2> mapper)
            where T : class
            where T1 : class
            where T2 : class
            => self.SelectMany(x =>
                selector(x).SelectMany(y => Free.Point<TCommand, T2>(mapper(x, y))));
    }
}
