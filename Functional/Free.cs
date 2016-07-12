using System;

namespace Functional
{
    public class _T { public object Value { get; internal set; } }
    public class _T<T> : _T {
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }
    }

    public interface Free<TCommand> : IUnion { }
    public interface Free<TCommand, out TResult> : Free<TCommand>
        where TResult : class // value type does not support covariance
    { }

    public delegate Free<TCommand, TResult> Next<TCommand, in TInput, out TResult>(TInput input)
        where TInput : class // value type does not support contravariance
        where TResult : class; // value type does not support covariance

    public interface IPoint<TCommand, out TResult> : Free<TCommand, TResult>
        where TResult : class
    {
        TResult Value { get; }
    }

    public interface IJoin<TCommand, out TResult> : Free<TCommand, TResult>
        where TResult : class
    {
        TCommand Command { get; }
        Next<TCommand, _T, TResult> Next { get; }
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

    public sealed class Join<TCommand, TResult> : IJoin<TCommand, _T<TResult>>
    {
        public TCommand Command { get; private set; }
        public Next<TCommand, _T, _T<TResult>> Next { get; private set; }

        internal Join(TCommand command, Next<TCommand, _T, _T<TResult>> next)
        {
            this.Command = command;
            this.Next = next;
        }
    }
    public class Free
    {
        public static Free<TCommand, _T<T>> Point<TCommand, T>(T value)
            => new Point<TCommand, _T<T>>(new _T<T> { Value = value });
        public static Free<TCommand, _T<TResult>> Join<TCommand, TResult>(TCommand command, Next<TCommand, _T, _T<TResult>> next)
            => new Join<TCommand, TResult>(command, next);
        public static Free<TCommand, _T<T>> Join<TCommand, T>(TCommand command)
            => new Join<TCommand, T>(command, x => Point<TCommand, T>((T)x.Value));
        public static Free<TCommand, _T<T>> Join<TCommand, T>(TCommand command, T value)
            => new Join<TCommand, T>(command, _ => Point<TCommand, T>(value));
    }
    public static class FreeExt
    {
        public static Free<TCommand, _T<T1>> Select<TCommand, T, T1>(this Free<TCommand, _T<T>> self, Func<T, T1> selector)
            => self.Match(
                (IJoin<TCommand, _T<T>> x) => Free.Join(x.Command, r => Select(x.Next(r), selector)),
                (IPoint<TCommand, _T<T>> x) => Free.Point<TCommand, T1>(selector(x.Value.Value)),
                _ => null);

        public static Free<TCommand, _T<T1>> SelectMany<TCommand, T, T1>(this Free<TCommand, _T<T>> self, Func<T, Free<TCommand, _T<T1>>> selector)
            => self.Match(
                (IJoin<TCommand, _T<T>> x) => Free.Join(x.Command, r => SelectMany(x.Next(r), selector)),
                (IPoint<TCommand, _T<T>> x) => selector(x.Value.Value),
                _ => null);

        public static Free<TCommand, _T<T2>> SelectMany<TCommand, T, T1, T2>(this Free<TCommand, _T<T>> self, Func<T, Free<TCommand, _T<T1>>> selector, Func<T, T1, T2> mapper)
            => self.SelectMany(x =>
                selector(x).SelectMany(y => Free.Point<TCommand, T2>(mapper(x, y))));

        
        private static _T RunInterpreter<TCommand>(Free<TCommand> program, Func<TCommand, object> interpreter)
        {
            var join = program as IJoin<TCommand, _T>;

            while (join != null)
            {
                program = join.Next(new _T { Value = interpreter(join.Command) });
                join = program as IJoin<TCommand, _T>;
            }

            var point = program as IPoint<TCommand, _T>;
            
            return point?.Value;
        }

        public static TResult Interpret<TCommand, TResult>(this Free<TCommand> self, Func<TCommand, object> interpreter) =>
            (TResult)RunInterpreter(self, interpreter).Value;

        public static void Interpret<TCommand>(this Free<TCommand> self, Func<TCommand, object> interpreter)
        {
            RunInterpreter(self, interpreter);
        }
    }
}
