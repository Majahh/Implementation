using System;
using System.Numerics;

namespace Implementering
{
    public class multipleModPrime:IHashFunction
    {
        BigInteger a;
        BigInteger b;
        BigInteger p;
        int q;
        int l;
        ulong s;

        public multipleModPrime(BigInteger aValue, BigInteger bValue, int lValue)
        {
            a = aValue;
            b = bValue;
            l = lValue;
            s = 1UL << l;
            q = 89;
            p = BigInteger.Pow(2, q) - 1;
        }

        public BigInteger getvalue(ulong x)
        {
            BigInteger c = a * x + b;
            BigInteger hx = (c & p) + (c >> q);
            if (hx >= p)
            {
                hx -= p;
            }
            hx %= s;
            return hx;
        }
    }
}
