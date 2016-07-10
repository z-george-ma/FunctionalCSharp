using System;

namespace Functional
{
    public sealed class Unit
    {
        private Unit() { }
        public readonly static Unit Default = new Unit();
    }

    public sealed class Void
    {
        private Void() { }
    }

    public class Box<T> : IEquatable<Box<T>>
    {
        public T Value { get; private set; }

        public bool Equals(Box<T> other) => Value.Equals(other.Value);

        public static implicit operator T(Box<T> b) => b.Value;

        public static implicit operator Box<T>(T b) => new Box<T> { Value = b };
    }

    public static class Box
    {
        public static Box<T2> Fmap<T1, T2>(Func<T1, T2> mapper, Box<T1> t1) => mapper(t1);
        public static Box<T3> Fmap<T1, T2, T3>(Func<T1, T2, T3> mapper, Box<T1> t1, Box<T2> t2) => mapper(t1, t2);
        public static Box<T4> Fmap<T1, T2, T3, T4>(Func<T1, T2, T3, T4> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3) => mapper(t1, t2, t3);
        public static Box<T5> Fmap<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4) => mapper(t1, t2, t3, t4);
        public static Box<T6> Fmap<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5) => mapper(t1, t2, t3, t4, t5);
        public static Box<T7> Fmap<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5, Box<T6> t6) => mapper(t1, t2, t3, t4, t5, t6);
        public static Box<T8> Fmap<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5, Box<T6> t6, Box<T7> t7) => mapper(t1, t2, t3, t4, t5, t6, t7);
    }
}
