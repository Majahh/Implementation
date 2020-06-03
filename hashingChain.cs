using System;
using System.Collections.Generic;

namespace Implementering
{
    public class hashingChain
    {
        private List<Tuple<ulong, int>>[] Hashtable;
        private IHashFunction hash;
        private ulong mysize;
        private int lValue;


        public hashingChain(int l, IHashFunction myhash)
        {
            hash = myhash;
            mysize = 1UL << l;
            lValue = l;
            Hashtable = new List<Tuple<ulong, int>>[mysize];

            for (int i = 0; i < Hashtable.Length; i++)
            {
                Hashtable[i] = new List<Tuple<ulong, int>>();
            }
        }
        public int get(ulong x)
        {
            ulong hx = (ulong) hash.getvalue(x);
            int len = Hashtable[hx].Count;
            if (len == 0)
            {
                return 0;
            }
            for (int i = 0; i < len; i++)
            {
                if (Hashtable[hx][i].Item1 == x)
                {
                    return Hashtable[hx][i].Item2;
                }
            }
            return 0;
        }

        public void set(ulong x, int v)
        {
            ulong hx = (ulong) hash.getvalue(x);
            int len = Hashtable[hx].Count;
            if (get(x) == 0)
            {
                Tuple<ulong, int> entry = Tuple.Create(x, v);
                Hashtable[hx].Add(entry);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if (Hashtable[hx][i].Item1 == x)
                    {
                        Tuple<ulong, int> overwrite = Tuple.Create(x, v);
                        Hashtable[hx][i] = overwrite;
                    }

                }

            }
        }
        public void increment(ulong x, int d)
        {
            ulong hx = (ulong) hash.getvalue(x);
            int len = Hashtable[hx].Count;
            if (get(x) == 0)
            {
                Tuple<ulong, int> entry = Tuple.Create(x, d);
                Hashtable[hx].Add(entry);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if (Hashtable[hx][i].Item1 == x)
                    {
                        int newValue = Hashtable[hx][i].Item2;
                        newValue += d;
                        Tuple<ulong, int> overwrite = Tuple.Create(x, newValue);
                        Hashtable[hx][i] = overwrite;
                    }

                }

            }
        }

        public int KvadratSum() {
            int sum = 0;
            int len = (int)mysize;
            for (int i = 0; i < len; i++) {
                int innerLen = Hashtable[i].Count;
                for (int j = 0; j < innerLen; j++) {

                    int sx = Hashtable[i][j].Item2;
                    int sx2 = (int)Math.Pow(2, sx);
                    sum += sx2;
                }
            }

            return sum;
        }

        public ulong count() {
            return mysize;
        }

        public int subcount(int index) {
            int tal = Hashtable[index].Count;
            return tal;
        }

        public List<Tuple<ulong, int>> getbyindex(int index) {
            List<Tuple<ulong, int>> list = new List<Tuple<ulong, int>>();
            foreach (var elm in Hashtable[index]) {

                list.Add(elm);
            }
            return list;
        }

        public double expectedEmptyShift(int balls) {
            double m = (double)balls;
            double n = (double)mysize;
            double probZero = Math.Pow(1 - 1 / n, m);
            return probZero * n;
        }

    }
}
