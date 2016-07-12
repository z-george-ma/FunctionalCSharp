using System;
using System.Threading.Tasks;

namespace Functional
{
    public static class Dependency<T, TMarker> where T : class
    {
        private static Func<T> _create = () => default(T);
        private static Action<T> _dispose = x => {
            var disposable = x as IDisposable;
            if (disposable != null) disposable.Dispose();
        };

        public static Func<T> Create => _create;
        public static Action<T> Dispose => _dispose;

        public static void Register(Func<T> f)
        {
            _create = f;
        }

        public static void Register(Func<T> create, Action<T> dispose)
        {
            _create = create;
            _dispose = dispose;
        }

        public static Func<TInput, TResult> Inject<TInput, TResult>(Func<T, TInput, TResult> f)
        {
            return t1 =>
            {
                var provider = _create();

                try
                {
                    return f(provider, t1);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static Func<TResult> Inject<TResult>(Func<T, TResult> f)
        {
            return () =>
            {
                var provider = _create();

                try
                {
                    return f(provider);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static Action Inject(Action<T> f)
        {
            return () =>
            {
                var provider = _create();

                try
                {
                    f(provider);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static TResult Run<TInput, TResult>(Func<T, TInput, TResult> f, TInput input)
        {
            return Inject(f)(input);
        }

        public static TResult Run<TResult>(Func<T, TResult> f)
        {
            return Inject(f)();
        }

        public static void Run(Action<T> f)
        {
            Inject(f)();
        }

        public static Func<T1, Task<T2>> InjectAsync<T1, T2>(Func<T, T1, Task<T2>> f)
        {
            return async t1 =>
            {
                var provider = _create();

                try
                {
                    return await f(provider, t1);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static Func<Task<TResult>> InjectAsync<TResult>(Func<T, Task<TResult>> f)
        {
            return async () =>
            {
                var provider = _create();

                try
                {
                    return await f(provider);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static Func<Task> InjectAsync(Func<T, Task> f)
        {
            return async () =>
            {
                var provider = _create();

                try
                {
                    await f(provider);
                }
                finally
                {
                    _dispose(provider);
                }
            };
        }

        public static Task<TResult> RunAsync<TInput, TResult>(Func<T, TInput, Task<TResult>> f, TInput input)
        {
            return InjectAsync(f)(input);
        }

        public static Task<TResult> RunAsync<TResult>(Func<T, Task<TResult>> f)
        {
            return InjectAsync(f)();
        }

        public static Task RunAsync(Func<T, Task> f)
        {
            return InjectAsync(f)();
        }
    }

    public class Default { }

    public static class Dependency
    {
        public static void Register<T, TMarker>(Func<T> f) where T : class
        {
            Dependency<T, TMarker>.Register(f);
        }
        public static void Register<T, TMarker>(Func<T> create, Action<T> dispose) where T : class
        {
            Dependency<T, TMarker>.Register(create, dispose);
        }
        public static void Register<T>(Func<T> f) where T : class
        {
            Dependency<T, Default>.Register(f);
        }

        public static void Register<T>(Func<T> create, Action<T> dispose) where T : class
        {
            Dependency<T, Default>.Register(create, dispose);
        }
    }

    public static class Dependency<T> where T: class
    {
        public static Func<T> Create => Dependency<T, Default>.Create;
        public static Action<T> Dispose => Dependency<T, Default>.Dispose;

        public static void Register(Func<T> f)
        {
            Dependency<T, Default>.Register(f);
        }

        public static void Register(Func<T> create, Action<T> dispose)
        {
            Dependency<T, Default>.Register(create, dispose);
        }

        public static Func<TInput, TResult> Inject<TInput, TResult>(Func<T, TInput, TResult> f)
        {
            return Dependency<T, Default>.Inject(f);
        }

        public static Func<TResult> Inject<TResult>(Func<T, TResult> f)
        {
            return Dependency<T, Default>.Inject(f);
        }

        public static Action Inject(Action<T> f)
        {
            return Dependency<T, Default>.Inject(f);
        }

        public static TResult Run<TInput, TResult>(Func<T, TInput, TResult> f, TInput input)
        {
            return Dependency<T, Default>.Run(f, input);
        }

        public static TResult Run<TResult>(Func<T, TResult> f)
        {
            return Dependency<T, Default>.Run(f);
        }

        public static void Run(Action<T> f)
        {
            Dependency<T, Default>.Run(f);
        }

        public static Func<T1, Task<T2>> InjectAsync<T1, T2>(Func<T, T1, Task<T2>> f)
        {
            return Dependency<T, Default>.InjectAsync(f);
        }

        public static Func<Task<TResult>> InjectAsync<TResult>(Func<T, Task<TResult>> f)
        {
            return Dependency<T, Default>.InjectAsync(f);
        }

        public static Func<Task> InjectAsync(Func<T, Task> f)
        {
            return Dependency<T, Default>.InjectAsync(f);
        }

        public static Task<TResult> RunAsync<TInput, TResult>(Func<T, TInput, Task<TResult>> f, TInput input)
        {
            return Dependency<T, Default>.RunAsync(f, input);
        }

        public static Task<TResult> RunAsync<TResult>(Func<T, Task<TResult>> f)
        {
            return Dependency<T, Default>.RunAsync(f);
        }

        public static Task RunAsync(Func<T, Task> f)
        {
            return Dependency<T, Default>.RunAsync(f);
        }
    }
}
