using System;
using System.Numerics;

namespace Implementering
{
    public interface IHashFunction
    {
        BigInteger getvalue(ulong x);
    }
}
