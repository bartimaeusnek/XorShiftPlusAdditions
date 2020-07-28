using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace XorShiftPlusAdditions
{
    public partial class XorShiftRandom : Random
    {
        public override int Next() => this.NextInt32();

        public override int Next(int maxValue) => this.NextInt32() % maxValue;

        public override int Next(int minValue, int maxValue)
            => clamp(this.NextInt32(), minValue, maxValue);

        public override void NextBytes(Span<byte> buffer)
            => this.GetRandomBytes(buffer);

        [ExcludeFromCodeCoverage]
        protected override double Sample()
            => this.NextDouble();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T clamp<T>(T v, T lo, T hi) where T: IComparable<T>
        {
            if (hi.CompareTo(lo) == -1)
                throw new ArgumentOutOfRangeException($"{nameof(hi)} must be a higher Value than {nameof(lo)}");
            
            return v.CompareTo(lo) == -1 ? lo : hi.CompareTo(v) == -1 ? hi : v;
        }
    }
}