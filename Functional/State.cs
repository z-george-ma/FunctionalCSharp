using System;

namespace Functional
{
    public delegate Tuple<TState, TResult> State<TState, TResult>(TState state);

    public static class State
    {
        public static State<TState, TResult> Return<TState, TResult>(TResult t) => s => Tuple.Create(s, t);

        public static State<TState, TResult> New<TState, TResult>(Func<TState, Tuple<TState, TResult>> f) => s => f(s);
    }

    public static class StateExt
    {
        public static State<TState, T1> Select<TState, T, T1>(this State<TState, T> self, Func<T, T1> selector) =>
            state =>
            {
                var t = self(state);
                return Tuple.Create(t.Item1, selector(t.Item2));
            };

        public static State<TState, T1> SelectMany<TState, T, T1>(this State<TState, T> self, Func<T, State<TState, T1>> selector) =>
            state =>
            {
                var t = self(state);
                return selector(t.Item2)(t.Item1);
            };  

        public static State<TState, T2> SelectMany<TState, T, T1, T2>(this State<TState, T> self, Func<T, State<TState, T1>> selector, Func<T, T1, T2> mapper) =>
            state => {
                var t = self(state);
                var t1 = selector(t.Item2)(t.Item1);
                return Tuple.Create(t1.Item1, mapper(t.Item2, t1.Item2));
            };

        public static State<TState, TState> Get<TState, T>(this State<TState, T> self) =>
            state => {
                var t = self(state);
                return Tuple.Create(t.Item1, t.Item1);
            };
        public static State<TState, T1> Gets<TState, T, T1>(this State<TState, T> self, Func<TState, T1> mapper) =>
            state =>
            {
                var t = self(state);
                return Tuple.Create(t.Item1, mapper(t.Item1));
            };
        public static State<TState, Unit> Modify<TState, T>(this State<TState, T> self, Func<TState, TState> mapper) =>
            state =>
            {
                var t = self(state);
                return Tuple.Create(mapper(t.Item1), Unit.Default);
            };
        public static State<TState, Unit> Put<TState, T>(this State<TState, T> self, TState newState) =>
            state => {
                var t = self(state);
                return Tuple.Create(newState, Unit.Default);
            };
        
        public static Tuple<TState, TResult> Run<TState, TResult>(this State<TState, TResult> self, TState state) =>
            self(state);
        public static TResult Eval<TState, TResult>(this State<TState, TResult> self, TState state) =>
            self(state).Item2;
        public static TState Exec<TState, TResult>(this State<TState, TResult> self, TState state) =>
            self(state).Item1;
    }
}
