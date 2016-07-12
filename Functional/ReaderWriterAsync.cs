using System;
using System.Linq;
using System.Threading.Tasks;

namespace Functional
{
    public delegate Task<Writer<TResult, TWriter>> ReaderWriterAsync<TReader, TWriter, TResult>(Action<TWriter> writerProjector, TReader reader);

    public static class ReaderWriterAsync
    {
        public static ReaderWriterAsync<TReader, TWriter, TResult> Return<TReader, TWriter, TResult>(TResult t) => (p, r) => Task.FromResult(t.WithLogs(Enumerable.Empty<TWriter>()));
        public static ReaderWriterAsync<TReader, TWriter, TResult> New<TReader, TWriter, TResult>(Func<TReader, Task<Writer<TResult, TWriter>>> f) => (p, r) => f(r);
    }

    public static class ReaderWriterAsyncExt
    {
        public static ReaderWriterAsync<TReader, TWriter, TResult1> Select<TReader, TWriter, TResult, TResult1>(this ReaderWriterAsync<TReader, TWriter, TResult> self, Func<TResult, TResult1> selector) =>
            async (p, r) =>
            {
                var t = await self(p, r);
                t.Output.Iter(p);
                return selector(t.Result).WithLogs(Enumerable.Empty<TWriter>());
            };

        public static ReaderWriterAsync<TReader, TWriter, TResult1> SelectMany<TReader, TWriter, TResult, TResult1>(this ReaderWriterAsync<TReader, TWriter, TResult> self, Func<TResult, ReaderWriterAsync<TReader, TWriter, TResult1>> selector) =>
            async (p, r) =>
            {
                var t = await self(p, r);
                t.Output.Iter(p);
                return await selector(t.Result)(p, r);
            };  

        public static ReaderWriterAsync<TReader, TWriter, TResult2> SelectMany<TReader, TWriter, TResult, TResult1, TResult2>(this ReaderWriterAsync<TReader, TWriter, TResult> self, Func<TResult, ReaderWriterAsync<TReader, TWriter, TResult1>> selector, Func<TResult, TResult1, TResult2> mapper) =>
            async (p, r) => {
                var t = await self(p, r);
                t.Output.Iter(p);
                var t1 = await selector(t.Result)(p, r);
                t1.Output.Iter(p);
                return mapper(t.Result, t1.Result).WithLogs(Enumerable.Empty<TWriter>());
            };
        
        public static async Task<TResult> Run<TReader, TWriter, TResult>(this ReaderWriterAsync<TReader, TWriter, TResult> self, Action<TWriter> p, TReader r)
        {
            var t = await self(p, r);
            t.Output.Iter(p);
            return t.Result;
        }
    }
}
