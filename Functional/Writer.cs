using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Functional
{
    public class Writer<TResult, TOutput>
    {
        public TResult Result { get; set; }
        public IEnumerable<TOutput> Output { get; set; }
    }

    public static class Writer
    {
        public static Writer<TResult, TOutput> Return<TResult, TOutput>(TResult r) => Writer.New(r, Enumerable.Empty<TOutput>());
        public static Writer<TResult, TOutput> New<TResult, TOutput>(TResult r, IEnumerable<TOutput> o) => new Writer<TResult, TOutput> { Result = r, Output = o };
    }

    public static class WriterExt
    {
        public static Writer<TResult1, TOutput> Select<TResult, TOutput, TResult1>(this Writer<TResult, TOutput> self, Func<TResult, TResult1> selector) =>
            Writer.New(selector(self.Result), self.Output);

        public static Writer<TResult1, TOutput> SelectMany<TResult, TOutput, TResult1>(this Writer<TResult, TOutput> self, Func<TResult, Writer<TResult1, TOutput>> selector)
        {
            var w = selector(self.Result);
            return Writer.New(w.Result, self.Output.Concat(w.Output));
        }
            

        public static Writer<TResult2, TOutput> SelectMany<TResult, TOutput, TResult1, TResult2>(this Writer<TResult, TOutput> self, Func<TResult, Writer<TResult1, TOutput>> selector, Func<TResult, TResult1, TResult2> mapper)
        {
            var w = selector(self.Result);
            return Writer.New(mapper(self.Result, w.Result), self.Output.Concat(w.Output));
        }

        public static Writer<TResult, TOutput> WithLogs<TResult, TOutput>(this TResult self, IEnumerable<TOutput> output)
        {
            return Writer.New(self, output);
        }
    }
    
    public static class AsyncWriterExt
    {
        public async static Task<Writer<TResult, TOutput>> WithLogs<TResult, TOutput>(this Task<TResult> self, IEnumerable<TOutput> output)
        {
            return Writer.New(await self, output);
        }
    }
}
