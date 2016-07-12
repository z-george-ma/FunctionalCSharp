using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Functional.Tests
{
    [TestClass]
    public class MaybeTest
    {
        [TestMethod]
        public void MaybeTest_SelectMany()
        {
            var m = Maybe.Just(100);
            Func<int, Maybe<string>> f1 = i => i > 50 ? Maybe.Just(i.ToString()) : Maybe.Nothing;
            Func<string, string> f2 = i => "Hello world " + i.ToString();
            Func<string, Maybe<string>> f3 = i => i.EndsWith("0") ? Maybe.Just("End with 0") : Maybe.Nothing;

            var result =
                from r0 in m
                from r1 in f1(r0)
                let r2 = f2(r1)
                from r3 in f3(r2)
                select r3;

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("End with 0", result.Value);
        }

        [TestMethod]
        public void MaybeTest_MonadLaw1()
        {
            var m = Maybe.Just(1000);
            Func<int, Maybe<string>> f = (int r) => Maybe.Just(r.ToString());
            var left = m.SelectMany(f);
            var right = f(1000);
            
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void MaybeTest_MonadLaw2()
        {
            var m = Maybe.Just(1000);
            var left = m.SelectMany(x => Maybe.Just(x));
            var right = m;
            
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void MaybeTest_MonadLaw3()
        {
            var m = Maybe.Just(1000);
            Func<int, Maybe<string>> f = (int r) => Maybe.Just(r.ToString());
            Func<string, Maybe<int>> g = (string r) =>
            {
                int ret;
                return int.TryParse(r, out ret) ? Maybe.Just(ret) : Maybe.Nothing;
            };

            var left = m.SelectMany(f).SelectMany(g);
            var right = m.SelectMany(x => f(x).SelectMany(g));
            
            Assert.AreEqual(left, right);
        }
    }
}
