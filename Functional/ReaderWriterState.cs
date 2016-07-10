using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional
{
    public delegate Tuple<TState, TResult, IEnumerable<TWriter>> ReaderWriterState<TReader, TWriter, TState, TResult>(Action<IEnumerable<TWriter>> writerProjector, TReader reader, TState state);

    public static class ReaderWriterState
    {
        public static ReaderWriterState<TReader, TWriter, TState, TResult> Return<TReader, TWriter, TState, TResult>(TResult t) => (p, r, s) => Tuple.Create(s, t, Enumerable.Empty<TWriter>());
        public static ReaderWriterState<TReader, TWriter, TState, TResult> New<TReader, TWriter, TState, TResult>(Func<Action<IEnumerable<TWriter>>, TReader, TState, Tuple<TState, TResult, IEnumerable<TWriter>>>) => (p, r, s) => f(p, r, s);

    }

    public static class ReaderWriterStateExt
    {
        public static ReaderWriterState<TReader, TWriter, TState, TResult1> Select<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, TResult1> selector) =>
            (p, r, s) =>
            {
                var t = self(p, r, s);
                return Tuple.Create(t.Item1, selector(t.Item2));
            };

        public static ReaderWriterState<TReader, TWriter, TState, TResult1> SelectMany<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, ReaderWriterState<TReader, TWriter, TState, TResult1>> selector) =>
            state =>
            {
                var t = self(state);
                return selector(t.Item2)(t.Item1);
            };  

        public static ReaderWriterState<TReader, TWriter, TState, TResult2> SelectMany<TReader, TWriter, TState, TResult, TResult1, TResult2>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TResult, ReaderWriterState<TReader, TWriter, TState, TResult1>> selector, Func<TResult, TResult1, TResult2> mapper) =>
            state => {
                var t = self(state);
                var t1 = selector(t.Item2)(t.Item1);
                return Tuple.Create(t1.Item1, mapper(t.Item2, t1.Item2));
            };

        public static ReaderWriterState<TReader, TWriter, TState, TResult> Get<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self) =>
            state => {
                var t = self(state);
                return Tuple.Create(t.Item1, t.Item1);
            };
        public static ReaderWriterState<TReader, TWriter, TState, TResult1> Gets<TReader, TWriter, TState, TResult, TResult1>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TState, TResult1> mapper) =>
            state =>
            {
                var t = self(state);
                return Tuple.Create(t.Item1, mapper(t.Item1));
            };
        public static ReaderWriterState<TReader, TWriter, TState, Unit> Modify<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, Func<TState, TState> mapper) =>
            state =>
            {
                var t = self(state);
                return Tuple.Create(mapper(t.Item1), Unit.Default);
            };
        public static ReaderWriterState<TReader, TWriter, TState, Unit> Put<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, TState newState) =>
            state => {
                var t = self(state);
                return Tuple.Create(newState, Unit.Default);
            };
        
        public static Tuple<TState, TResult> Run<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, TState initState) =>
            self(initState);
        public static TResult Eval<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, TState initState) =>
            self(initState).Item2;
        public static TState Exec<TReader, TWriter, TState, TResult>(this ReaderWriterState<TReader, TWriter, TState, TResult> self, TState initState) =>
            self(initState).Item1;
    }
}
