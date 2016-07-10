using System;

namespace Functional
{
    public interface IUnion { }
        
    public static class UnionExt
    {
        public static TResult Match<TUnion, TResult>(this TUnion self, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
        {
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, TResult>(this TUnion self, Func<T1, TResult> case1, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1: class, TUnion
        {
            if (self is T1) return case1(self as T1);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, T4, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<T4, TResult> case4, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            if (self is T4) return case4(self as T4);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, T4, T5, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<T4, TResult> case4, Func<T5, TResult> case5, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            if (self is T4) return case4(self as T4);
            if (self is T5) return case5(self as T5);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, T4, T5, T6, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<T4, TResult> case4, Func<T5, TResult> case5, Func<T6, TResult> case6, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            if (self is T4) return case4(self as T4);
            if (self is T5) return case5(self as T5);
            if (self is T6) return case6(self as T6);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, T4, T5, T6, T7, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<T4, TResult> case4, Func<T5, TResult> case5, Func<T6, TResult> case6, Func<T7, TResult> case7, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
            where T7 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            if (self is T4) return case4(self as T4);
            if (self is T5) return case5(self as T5);
            if (self is T6) return case6(self as T6);
            if (self is T7) return case7(self as T7);
            return defaultCase(self);
        }
        public static TResult Match<TUnion, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this TUnion self, Func<T1, TResult> case1, Func<T2, TResult> case2, Func<T3, TResult> case3, Func<T4, TResult> case4, Func<T5, TResult> case5, Func<T6, TResult> case6, Func<T7, TResult> case7, Func<T8, TResult> case8, Func<TUnion, TResult> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
            where T7 : class, TUnion
            where T8 : class, TUnion
        {
            if (self is T1) return case1(self as T1);
            if (self is T2) return case2(self as T2);
            if (self is T3) return case3(self as T3);
            if (self is T4) return case4(self as T4);
            if (self is T5) return case5(self as T5);
            if (self is T6) return case6(self as T6);
            if (self is T7) return case7(self as T7);
            if (self is T8) return case8(self as T8);
            return defaultCase(self);
        }

        public static void Match<TUnion>(this TUnion self, Action<TUnion> defaultCase)
            where TUnion : IUnion
        {
            defaultCase(self);
        }
        public static void Match<TUnion, T1>(this TUnion self, Action<T1> case1, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3, T4>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<T4> case4, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else if (self is T4) case4(self as T4);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3, T4, T5>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<T4> case4, Action<T5> case5, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else if (self is T4) case4(self as T4);
            else if (self is T5) case5(self as T5);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3, T4, T5, T6>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<T4> case4, Action<T5> case5, Action<T6> case6, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else if (self is T4) case4(self as T4);
            else if (self is T5) case5(self as T5);
            else if (self is T6) case6(self as T6);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3, T4, T5, T6, T7>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<T4> case4, Action<T5> case5, Action<T6> case6, Action<T7> case7, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
            where T7 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else if (self is T4) case4(self as T4);
            else if (self is T5) case5(self as T5);
            else if (self is T6) case6(self as T6);
            else if (self is T7) case7(self as T7);
            else defaultCase(self);
        }
        public static void Match<TUnion, T1, T2, T3, T4, T5, T6, T7, T8>(this TUnion self, Action<T1> case1, Action<T2> case2, Action<T3> case3, Action<T4> case4, Action<T5> case5, Action<T6> case6, Action<T7> case7, Action<T8> case8, Action<TUnion> defaultCase)
            where TUnion : IUnion
            where T1 : class, TUnion
            where T2 : class, TUnion
            where T3 : class, TUnion
            where T4 : class, TUnion
            where T5 : class, TUnion
            where T6 : class, TUnion
            where T7 : class, TUnion
            where T8 : class, TUnion
        {
            if (self is T1) case1(self as T1);
            else if (self is T2) case2(self as T2);
            else if (self is T3) case3(self as T3);
            else if (self is T4) case4(self as T4);
            else if (self is T5) case5(self as T5);
            else if (self is T6) case6(self as T6);
            else if (self is T7) case7(self as T7);
            else if (self is T8) case8(self as T8);
            else defaultCase(self);
        }
    }
}

