using System;

namespace Functional
{
    public delegate T Reader<in TEnv, out T>(TEnv env);

    public static class Reader
    {
        public static Reader<TEnv, TEnv> Ask<TEnv>() => e => e;
        public static Reader<TEnv, T> Return<TEnv, T>(T  value) => _ => value;
        public static Reader<TEnv, T> New<TEnv, T>(Func<TEnv, T> f) => e => f(e);
    }

    public static class ReaderExt
    {
        public static Reader<TEnv, T1> Select<TEnv, T, T1>(this Reader<TEnv, T> self, Func<T, T1> selector) =>
            env => selector(self(env));

        public static Reader<TEnv, T1> SelectMany<TEnv, T, T1>(this Reader<TEnv, T> self, Func<T, Reader<TEnv, T1>> selector) =>
            env => selector(self(env))(env);

        public static Reader<TEnv, T2> SelectMany<TEnv, T, T1, T2>(this Reader<TEnv, T> self, Func<T, Reader<TEnv, T1>> selector, Func<T, T1, T2> mapper) =>
            env => {
                var t = self(env);
                return mapper(t, selector(t)(env));
            };

        public static Reader<TEnvNew, T> WithReaderT<TEnv, T, TEnvNew>(this Reader<TEnv, T> self, Func<TEnvNew, TEnv> mapper) =>
            env => self(mapper(env));

        public static T Run<TEnv, T>(this Reader<TEnv, T> self, TEnv env) =>
            self(env);
    }
}
