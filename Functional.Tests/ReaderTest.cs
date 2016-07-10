using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functional.Tests
{
    [TestClass]
    public class ReaderTest
    {
        delegate int Generator();

        [TestMethod]
        public void ReaderTest_Select()
        {
            int generatorState = 0;
            Generator generator = () => ++generatorState; // returns 1, 2, 3, 4
            var reader =
                from g in Reader.Ask<Generator>()
                select "Result=" + g();
            Assert.AreEqual("Result=1", reader.Run(generator));
        }

        [TestMethod]
        public void ReaderTest_SelectMany()
        {
            int generatorState = 0;
            Generator generator = () => ++generatorState; // returns 1, 2, 3, 4
            var reader =
                from r1 in Reader.New<Generator, int>(gen => gen() + 1)
                from r2 in Reader.New<Generator, int>(gen => gen() + r1)
                select "Result=" + r2;
            Assert.AreEqual("Result=4", reader.Run(generator));
        }
        
        delegate int RandomGenerator();

        [TestMethod]
        public void ReaderTest_MonadLaw1()
        {
            var m = Reader.Return<RandomGenerator, int>(9999);
            Func<int, Reader<RandomGenerator, int>> f = (int r) => Reader.New<RandomGenerator, int>(gen => gen() + r);
            var left = m.SelectMany(f);
            var right = f(9999);

            var random = new Random().Next();
            RandomGenerator generator = () => random;
            Assert.AreEqual(left(generator), right(generator));
        }

        [TestMethod]
        public void ReaderTest_MonadLaw2()
        {
            var m = Reader.New<RandomGenerator, int>(gen => gen() + 100);
            var left = m.SelectMany(x => Reader.Return<RandomGenerator, int>(x));
            var right = m;

            var random = new Random().Next();
            RandomGenerator generator = () => random;
            Assert.AreEqual(left(generator), right(generator));
        }

        [TestMethod]
        public void ReaderTest_MonadLaw3()
        {
            var m = Reader.New<RandomGenerator, int>(gen => gen() + 100);
            Func<int, Reader<RandomGenerator, int>> f = (int r) => Reader.New<RandomGenerator, int>(gen => gen() - r);
            Func<int, Reader<RandomGenerator, int>> g = (int r) => Reader.New<RandomGenerator, int>(gen => gen() * r);

            var left = m.SelectMany(f).SelectMany(g);
            var right = m.SelectMany(x => f(x).SelectMany(g));

            var random = new Random().Next();
            RandomGenerator generator = () => random;
            Assert.AreEqual(left(generator), right(generator));
        }
    }
}
