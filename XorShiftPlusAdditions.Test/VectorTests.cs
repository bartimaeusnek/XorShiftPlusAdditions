using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XorShiftPlusAdditions.Test
{
    [TestClass, ExcludeFromCodeCoverage]
    public class VectorTests
    {
        XorShiftRandom _random = new XorShiftRandom();
        
        [TestMethod]
        public void TestVectorNotCrashing()
        {
            this._random.NextVector(32);
            this._random.NextVector128();
            this._random.NextVector256();
        }
    }
}