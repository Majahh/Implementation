using System;
using System.Collections.Generic;
using System.Numerics;

namespace Implementering
{
    public class SHash:IHashFunction
    {
        public SHash()
        {
        }

        public BigInteger getvalue(ulong x)
        {
      
            Random rnd = new System.Random();
            ulong b = (ulong)rnd.Next(0, 2);
            BigInteger s = 1UL - (2UL * b);
            return s;

        }
    }
}
