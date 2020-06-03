using System;
using System.Numerics;

namespace Implementering
{
    public class DoubleHash
    {
        private IHashFunction g;
        public ulong m;
        
        

        public DoubleHash(IHashFunction hash, int t)
        {
            m = 1UL << t;
            
            g = hash;
        }

        public (ulong,int) GetValue(ulong x) {

            BigInteger gx = g.getvalue(x);
            ulong hx = (ulong)(gx&(m-1));
            int bx = (int) gx >> 88;
            int sx = 1 - 2 * bx;
            return (hx, sx);
        }
        

    }
}
