using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional
{
    public delegate Writer<TResult, TWriter> ReaderWriter<TReader, TWriter, TResult>(Action<TWriter> writerProjector, TReader reader);

    public static class ReaderWriter
    {
        public static ReaderWriter<TReader, TWriter, TResult> Return<TReader, TWriter, TResult>(TResult t) => (p, r) => t.WithLogs(Enumerable.Empty<TWriter>());
        public static ReaderWriter<TReader, TWriter, TResult> New<TReader, TWriter, TResult>(Func<TReader, Writer<TResult, TWriter>> f) => (p, r) => f(r);
    }

    public static class ReaderWriterExt
    {
        public static ReaderWriter<TReader, TWriter, TResult1> Select<TReader, TWriter, TResult, TResult1>(this ReaderWriter<TReader, TWriter, TResult> self, Func<TResult, TResult1> selector) =>
            (p, r) =>
            {
                var t = self(p, r);
                t.Output.Iter(p);
                return selector(t.Result).WithLogs(Enumerable.Empty<TWriter>());
            };

        public static ReaderWriter<TReader, TWriter, TResult1> SelectMany<TReader, TWriter, TResult, TResult1>(this ReaderWriter<TReader, TWriter, TResult> self, Func<TResult, ReaderWriter<TReader, TWriter, TResult1>> selector) =>
            (p, r) =>
            {
                var t = self(p, r);
                t.Output.Iter(p);
                return selector(t.Result)(p, r);
            };  

        public static ReaderWriter<TReader, TWriter, TResult2> SelectMany<TReader, TWriter, TResult, TResult1, TResult2>(this ReaderWriter<TReader, TWriter, TResult> self, Func<TResult, ReaderWriter<TReader, TWriter, TResult1>> selector, Func<TResult, TResult1, TResult2> mapper) =>
            (p, r) => {
                var t = self(p, r);
                t.Output.Iter(p);
                var t1 = selector(t.Result)(p, r);
                t1.Output.Iter(p);
                return mapper(t.Result, t1.Result).WithLogs(Enumerable.Empty<TWriter>());
            };
        
        public static TResult Run<TReader, TWriter, TResult>(this ReaderWriter<TReader, TWriter, TResult> self, Action<TWriter> p, TReader r)
        {
            var t = self(p, r);
            t.Output.Iter(p);
            return t.Result;
        }
    }
}
