using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Functional.Tests
{
    using DSL;
    using static DSL.Static;

    [TestClass]
    public class FreeTest
    {
        [TestMethod]
        public void FreeTest_TestDSL()
        {
            var program =
                from r1 in add(1, 2)
                from r2 in subtract(r1, 1)
                select r1 + r2;
            int result = program.Interpret<Command, int>(run);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void FreeTest_MonadLaw1()
        {
            var m = Free.Point<Command, int>(100);
            Func<int, Free<Command, _T<int>>> f = x => add(x, 9999);
            var left = m.SelectMany(f);
            var right = f(100);

            int leftResult = left.Interpret<Command, int>(run);
            int rightResult = right.Interpret<Command, int>(run);

            Assert.AreEqual(leftResult, rightResult);
        }

        [TestMethod]
        public void FreeTest_MonadLaw2()
        {
            var m = add(1999, 2999);
            var left = m.SelectMany(x => Free.Point<Command, int>(x));
            var right = m;

            int leftResult = left.Interpret<Command, int>(run);
            int rightResult = right.Interpret<Command, int>(run);

            Assert.AreEqual(leftResult, rightResult);
        }

        [TestMethod]
        public void FreeTest_MonadLaw3()
        {
            var m = add(1999, 2999);
            Func<int, Free<Command, _T<int>>> f = x => subtract(x, 9999);
            Func<int, Free<Command, _T<int>>> g = x => subtract(x, 8888);

            var left = m.SelectMany(f).SelectMany(g);
            var right = m.SelectMany(x => f(x).SelectMany(g));

            int leftResult = left.Interpret<Command, int>(run);
            int rightResult = right.Interpret<Command, int>(run);

            Assert.AreEqual(leftResult, rightResult);
        }
    }

    namespace DSL
    {
        public abstract class Command : IUnion { }

        public sealed class Add : Command
        {
            public int Argument1 { get; set; }
            public int Argument2 { get; set; }
        }

        public sealed class Subtract : Command
        {
            public int Argument1 { get; set; }
            public int Argument2 { get; set; }
        }
        
        public static class Static
        {
            public static Free<Command, _T<int>> add(int a, int b)
            {
                return Free.Join<Command, int>(new Add
                {
                    Argument1 = a,
                    Argument2 = b
                });
            }
            public static Free<Command, _T<int>> subtract(int a, int b)
            {
                return Free.Join<Command, int>(new Subtract
                {
                    Argument1 = a,
                    Argument2 = b
                });
            }

            public static object run(Command cmd) =>
                cmd.Match(
                    (Add x) => x.Argument1 + x.Argument2,
                    (Subtract x) => x.Argument1 - x.Argument2,
                    _ => 0 // will never happen
                );
        }
    }
}
