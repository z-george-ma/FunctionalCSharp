using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional
{
    public delegate Tuple<TState, TResult, IEnumerable<TWriter>> ReaderWriterState<TReader, TWriter, TState, TResult>(Action<TWriter> writerProjector, TReader reader, TState state);

    public static class ReaderWriterState
    {
        public static ReaderWriterState<TReader, TWriter, TState, TResult> Return<TReader, TWriter, TState, TResult>(TResult t) => (p, r, s) => s.With(t, Enumerable.Empty<TWriter>());
        public static ReaderWriterState<TReader, TWriter, TState, TResult> New<TReader, TWriter, TState, TResult>(Func<TReader, TState, Tuple<TState, TResult, IEnumerable<TWriter>>> f) => (p, r, s) => f(r, s);
    }

    public static class ReaderWriterStateExt
    {
        public static ReaderWriterState<TReader, TWriter, TState, TResult1> Select<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, TResult1> selector) =>
            (p, r, s) =>
            {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return t.Item1.With(selector(t.Item2), Enumerable.Empty<TWriter>());
            };

        public static ReaderWriterState<TReader, TWriter, TState, TResult1> SelectMany<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, ReaderWriterState<TReader, TWriter, TState, TResult1>> selector) =>
            (p, r, s) =>
            {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return selector(t.Item2)(p, r, t.Item1);
            };  

        public static ReaderWriterState<TReader, TWriter, TState, TResult2> SelectMany<TReader, TWriter, TState, TResult, TResult1, TResult2>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, ReaderWriterState<TReader, TWriter, TState, TResult1>> selector, Func<TResult, TResult1, TResult2> mapper) =>
            (p, r, s) => {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                var t1 = selector(t.Item2)(p, r, t.Item1);
                t1.Item3.Iter(p);
                return t1.Item1.With(mapper(t.Item2, t1.Item2), Enumerable.Empty<TWriter>());
            };

        public static ReaderWriterState<TReader, TWriter, TState, TState> Get<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self) =>
            (p, r, s) => {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return t.Item1.With(t.Item1, Enumerable.Empty<TWriter>());
            };
        public static ReaderWriterState<TReader, TWriter, TState, TResult1> Gets<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TState, TResult1> mapper) =>
            (p, r, s) =>
            {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return t.Item1.With(mapper(t.Item1), Enumerable.Empty<TWriter>());
            };
        public static ReaderWriterState<TReader, TWriter, TState, Unit> Modify<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TState, TState> mapper) =>
            (p, r, s) =>
            {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return mapper(t.Item1).With(Unit.Default, Enumerable.Empty<TWriter>());
            };
        public static ReaderWriterState<TReader, TWriter, TState, Unit> Put<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, TState newState) =>
            (p, r, s) => {
                var t = self(p, r, s);
                t.Item3.Iter(p);
                return newState.With(Unit.Default, Enumerable.Empty<TWriter>());
            };
        
        public static Tuple<TState, TResult> Run<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Action<TWriter> p, TReader r, TState s)
        {
            var t = self(p, r, s);
            t.Item3.Iter(p);
            return t.Item1.With(t.Item2);
        }

        public static TResult Eval<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Action<TWriter> p, TReader r, TState s)
        {
            var t = self(p, r, s);
            t.Item3.Iter(p);
            return t.Item2;
        }

        public static TState Exec<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Action<TWriter> p, TReader r, TState s)
        {
            var t = self(p, r, s);
            t.Item3.Iter(p);
            return t.Item1;
        }
    }
}
