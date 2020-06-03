using System;
using System.Numerics;

namespace Implementering
{
    public class FouruHash:IHashFunction
    {
        BigInteger p;
        BigInteger a0;
        BigInteger a1;
        BigInteger a2;
        BigInteger a3;
        int q;

        public FouruHash(BigInteger a0, BigInteger a1, BigInteger a2, BigInteger a3)
        {
            q = 89;
            p = BigInteger.Pow(2, q) - 1;
        }

        public BigInteger getvalue(ulong x)
        {
            BigInteger hx = a3;
            hx = hx * x + a2;
            hx = (hx & p) + (hx >> q);
            hx = hx * x + a1;
            hx = (hx & p) + (hx >> q);
            hx = hx * x + a0;
            hx = (hx & p) + (hx >> q);
            if (hx >= p) { hx = hx - p; }
            return hx;
        }
    }
}
