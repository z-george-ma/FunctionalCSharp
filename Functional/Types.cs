using System;
using System.Collections.Generic;

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
        public T Value { get; set; }
        
        public static bool operator ==(Box<T> a, Box<T> b) => a.Equals(b);
        public static bool operator !=(Box<T> a, Box<T> b) => !a.Equals(b);
        public override int GetHashCode() => Value.GetHashCode();
        public bool Equals(Box<T> other) => Value.Equals(other.Value);

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(Box<T>)) return false;
            return Equals((Box<T>)other);
        }
        
        public static implicit operator T(Box<T> b) => b.Value;
        public static implicit operator Box<T>(T b) => new Box<T> { Value = b };
    }

    public static class Box
    {
        public static Box<T> New<T>(T value) => value;
        public static Box<T2> fmap<T1, T2>(Func<T1, T2> mapper, Box<T1> t1) => mapper(t1);
        public static Box<T3> fmap<T1, T2, T3>(Func<T1, T2, T3> mapper, Box<T1> t1, Box<T2> t2) => mapper(t1, t2);
        public static Box<T4> fmap<T1, T2, T3, T4>(Func<T1, T2, T3, T4> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3) => mapper(t1, t2, t3);
        public static Box<T5> fmap<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4) => mapper(t1, t2, t3, t4);
        public static Box<T6> fmap<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5) => mapper(t1, t2, t3, t4, t5);
        public static Box<T7> fmap<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5, Box<T6> t6) => mapper(t1, t2, t3, t4, t5, t6);
        public static Box<T8> fmap<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8> mapper, Box<T1> t1, Box<T2> t2, Box<T3> t3, Box<T4> t4, Box<T5> t5, Box<T6> t6, Box<T7> t7) => mapper(t1, t2, t3, t4, t5, t6, t7);
    }

    public static class Ext
    {
        public static void Iter<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach(var t in self)
            {
                action(t);
            }
        }

        public static Tuple<T, T1> With<T, T1>(this T self, T1 t1) => Tuple.Create(self, t1);
        public static Tuple<T, T1, T2> With<T, T1, T2>(this T self, T1 t1, T2 t2) => Tuple.Create(self, t1, t2);
        public static Tuple<T, T1, T2, T3> With<T, T1, T2, T3>(this T self, T1 t1, T2 t2, T3 t3) => Tuple.Create(self, t1, t2, t3);
        public static Tuple<T, T1, T2, T3, T4> With<T, T1, T2, T3, T4>(this T self, T1 t1, T2 t2, T3 t3, T4 t4) => Tuple.Create(self, t1, t2, t3, t4);
        public static Tuple<T, T1, T2, T3, T4, T5> With<T, T1, T2, T3, T4, T5>(this T self, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => Tuple.Create(self, t1, t2, t3, t4, t5);
        public static Tuple<T, T1, T2, T3, T4, T5, T6> With<T, T1, T2, T3, T4, T5, T6>(this T self, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => Tuple.Create(self, t1, t2, t3, t4, t5, t6);
    }
}
