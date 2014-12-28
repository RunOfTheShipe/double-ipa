using System;
using System.Collections.Generic;
using DoubleIPA.Utils;
using NUnit.Framework;

namespace DoubleIPA.Testing.TestUtils
{
    [TestFixture]
    public class ArgTests : TestBase
    {
        [Test]
        public void Arg_NotNull()
        {
            object obj = null;

            Assert.Catch<ArgumentNullException>(() => {
                Arg.NotNull(obj, "test1");
            });

            obj = new object();
            Arg.NotNull(obj, "test2");
        }

        [Test]
        public void Arg_Cond()
        {
            Assert.Catch<ArgumentException>(() => {
                Arg.Cond(false);
            });

            Assert.Catch<ArgumentException>(() => {
                Arg.Cond(false, "message!");
            });

            Arg.Cond(true);
            Arg.Cond(true, "message!");
        }

        [Test]
        public void Arg_NotEmpty()
        {
            Assert.Catch<ArgumentException>(() => {
                Arg.NotEmpty<int>(new List<int>(), "test1");
            });

            Assert.Catch<ArgumentException>(() => {
                Arg.NotEmpty<object>(null, "test2");
            });

            List<double> list = new List<double>() { 0.0 };
            Arg.NotEmpty<double>(list, "test3");
        }
    }
}
