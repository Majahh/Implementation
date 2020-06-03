using System;
using System.Numerics;

namespace Implementering
{
    public class multipleShift:IHashFunction
    {
        ulong a;
        int l;

        public multipleShift(ulong aValue, int lValue)
        {
             a = aValue;
             l = lValue;
        }

        public BigInteger getvalue(ulong x)
        {
            ulong hx = (a * x) >> (64 - l);
            //Console.WriteLine("shifthash " + hx +  "       x: " + x);
            BigInteger ret = hx;
            return ret;
        }
    }
}
