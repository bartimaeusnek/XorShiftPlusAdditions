using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace XorShiftPlusAdditions
{
    public partial class XorShiftRandom
    {
        private const int Vector256Size = 32;
        private const int Vector128Size = 16;

        public unsafe Vector128<byte> NextVector128()
        {
            Span<byte> pBuffer = stackalloc byte[Vector128Size];
            this.GetRandomBytes(pBuffer);
            fixed(byte* buff = &pBuffer.GetPinnableReference())
                return Sse2.LoadVector128(buff);
        }
        
        public unsafe Vector256<byte> NextVector256()
        {
            Span<byte> pBuffer = stackalloc byte[Vector256Size];
            this.GetRandomBytes(pBuffer);
            fixed(byte* buff = &pBuffer.GetPinnableReference())
                return Avx.LoadVector256(buff);
        }
        
        public Vector<byte> NextVector(int size)
        {
            Span<byte> pBuffer = stackalloc byte[size];
            this.GetRandomBytes(pBuffer);
            return new Vector<byte>(pBuffer);
        }

        private unsafe void GetRandomBytes(Span<byte> bytes)
        {
            ulong x = this.x_, y = this.y_, temp_x, temp_y, z;

            fixed (byte* pRef = &bytes.GetPinnableReference())
            {
                ulong* pIndex = (ulong*) pRef;
                ulong* pEnd = (ulong*) (pRef + bytes.Length);

                while (pIndex <= pEnd - 1)
                {
                    temp_x =  y;
                    x      ^= x << 23;
                    temp_y =  x ^ y ^ (x >> 17) ^ (y >> 26);

                    *pIndex++ = temp_y + y;

                    x = temp_x;
                    y = temp_y;
                }

                if (pIndex < pEnd)
                {
                    x      ^= x << 23;
                    temp_y =  x ^ y ^ (x >> 17) ^ (y >> 26);
                    z      =  temp_y + y;

                    byte* pByte                   = (byte*) pIndex;
                    while (pByte < pEnd) *pByte++ = (byte) (z >>= 8);
                }
            }
            
            this.x_ = x;
            this.y_ = y;
        }
    }
}