using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XorShiftPlusAdditions.Test
{
    [TestClass, ExcludeFromCodeCoverage]
    public class RandomOverrideTest
    {
        private Random rng = new XorShiftRandom();

        [TestMethod]
        public void ClampThrows()
            => Assert.ThrowsException<ArgumentOutOfRangeException>(() => this.rng.Next(12, 5));
        
        [TestMethod]
        public void TestRandomNext()
            => this.rng.Next();

        [TestMethod]
        public void TestRandomNextDouble()
            => this.rng.NextDouble();

        [TestMethod]
        public void TestRandomBytes()
        {
            var bytes = new byte[12];
            this.rng.NextBytes(bytes);
            var bl = bytes.Aggregate(false, (current, baite) => current | baite != 0);
            Assert.IsTrue(bl);
        }
        
        [TestMethod]
        public void TestRandomBytesSpan()
        {
            var bytes = new byte[12].AsSpan();
            this.rng.NextBytes(bytes);
            var bl = false;
            foreach (var b in bytes) bl |= b != 0;
            Assert.IsTrue(bl);
        }


        [TestMethod]
        public void TestRandomClamp()
        {
            for (var i = 0; i < 10000; i++)
            {
                var rngN = this.rng.Next(12);
                if (rngN > 12)
                    Assert.Fail($"Random Number {rngN} is bigger than 12");
            
                rngN = this.rng.Next(5, 12);
                if (rngN > 12)
                    Assert.Fail($"Random Number {rngN} is bigger than 12");
                if (rngN < 5)
                    Assert.Fail($"Random Number {rngN} is smaller than 5");
            }
        }
    }
}