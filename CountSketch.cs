using System;
using System.Collections.Generic;

namespace Implementering
{
    public class CountSketch
    {
        DoubleHash _hs;
        long[] C;
        ulong k;
        public CountSketch(DoubleHash hs)
        {
            _hs = hs;
            k = hs.m;
            C = new long[k];
        }

        public void Process(ulong x, int delta)
        {
            (ulong hx, int sx) = _hs.GetValue(x);
            C[hx] = C[hx] + sx * delta;
        }

        public ulong EstimateS() {
            ulong X = 0;
            for (ulong i = 0; i < k; i++) {
           
                X += (ulong) (C[i] * C[i]);
            }
            return X;
            }
        }
    }

