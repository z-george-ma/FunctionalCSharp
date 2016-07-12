using System;

namespace Functional
{
    public static partial class Static
    {
        public static Func<TResult> fun<TResult>(Func<TResult> func)
        {
            return func;
        }
        public static Func<T1, TResult> fun<T1, TResult>(Func<T1, TResult> func)
        {
            return func;
        }
        public static Func<T1, Func<T2, TResult>> fun<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            return t1 => t2 => func(t1, t2);
        }
        public static Func<T1, Func<T2, Func<T3, TResult>>> fun<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            return t1 => t2 => t3 => func(t1, t2, t3);
        }
        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> fun<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            return t1 => t2 => t3 => t4 => func(t1, t2, t3, t4);
        }
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> fun<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return t1 => t2 => t3 => t4 => t5 => func(t1, t2, t3, t4, t5);
        }
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>> fun<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return t1 => t2 => t3 => t4 => t5 => t6 => func(t1, t2, t3, t4, t5, t6);
        }
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>> fun<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return t1 => t2 => t3 => t4 => t5 => t6 => t7 => func(t1, t2, t3, t4, t5, t6, t7);
        }
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>>> fun<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return t1 => t2 => t3 => t4 => t5 => t6 => t7 => t8 =>func(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static void ignore<T>(T _) { }        
    }
}
