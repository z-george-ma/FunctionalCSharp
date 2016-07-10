using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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
                select Box.Fmap((x, y) => x + y, r1, r2);
            int result = Interpret.Run(program) as Box<int>;
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void FreeTest_MonadLaw1()
        {
            var m = Free.Point<Command, Box<int>>(100);
            Func<Box<int>, Free<Command, Box<int>>> f = x => add(x, 9999);
            var left = m.SelectMany(f);
            var right = f(100);

            int leftResult = Interpret.Run(left) as Box<int>;
            int rightResult = Interpret.Run(right) as Box<int>;
            
            Assert.AreEqual(leftResult, rightResult);
        }

        [TestMethod]
        public void FreeTest_MonadLaw2()
        {
            var m = add(1999, 2999);
            var left = m.SelectMany(x => Free.Point<Command, Box<int>>(x));
            var right = m;
            
            int leftResult = Interpret.Run(left) as Box<int>;
            int rightResult = Interpret.Run(right) as Box<int>;

            Assert.AreEqual(leftResult, rightResult);
        }

        [TestMethod]
        public void FreeTest_MonadLaw3()
        {
            var m = add(1999, 2999);
            Func<Box<int>, Free<Command, Box<int>>> f = x => subtract(x, 9999);
            Func<Box<int>, Free<Command, Box<int>>> g = x => subtract(x, 8888);

            var left = m.SelectMany(f).SelectMany(g);
            var right = m.SelectMany(x => f(x).SelectMany(g));

            int leftResult = Interpret.Run(left) as Box<int>;
            int rightResult = Interpret.Run(right) as Box<int>;

            Assert.AreEqual(leftResult, rightResult);
        }
    }

    namespace DSL
    {
        public abstract class Command : IUnion { }

        public sealed class Add : Command
        {
            public Box<int> Argument1 { get; set; }
            public Box<int> Argument2 { get; set; }
        }

        public sealed class Subtract : Command
        {
            public Box<int> Argument1 { get; set; }
            public Box<int> Argument2 { get; set; }
        }
        
        public static class Static
        {
            public static Free<Command, Box<int>> add(Box<int> a, Box<int> b)
            {
                return Free.Join<Command, Box<int>>(new Add
                {
                    Argument1 = a,
                    Argument2 = b
                });
            }
            public static Free<Command, Box<int>> subtract(Box<int> a, Box<int> b)
            {
                return Free.Join<Command, Box<int>>(new Subtract
                {
                    Argument1 = a,
                    Argument2 = b
                });
            }
        }

        public static class Interpret
        {
            public static object RunCommand(Command cmd) =>
                cmd.Match(
                    (Add x) => Box.Fmap((a, b) => a + b, x.Argument1, x.Argument2),
                    (Subtract x) => Box.Fmap((a, b) => a - b, x.Argument1, x.Argument2),
                    _ => null
                );
            
            public static object Run(Free<Command> program)
            {
                return program.Match(
                    (IJoin<Command, object> x) =>
                        Run(x.Next(RunCommand(x.Command))),
                    (IPoint<Command, object> x) => x.Value,
                    _ => null
                    );
            }
        }
    }
}
