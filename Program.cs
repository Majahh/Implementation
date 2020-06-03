using System;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Implementering
{
    public class Program

    {
        public static void Main(string[] args)
        {
            //function for calculating the sum of the hashvalue for x's in a stream
            ulong checkHashSum(IEnumerable<Tuple<ulong, int>> streeem, IHashFunction hash)
            {
                ulong sum = 0UL;
                foreach (var element in streeem)
                {
                    ulong x = element.Item1;
                    sum += (ulong)hash.getvalue(x);
                }
                return sum;
            }

            int l = 12;
            int n = 4 * (int)Math.Pow(2, l);

            ulong a = 13459550533363289979UL;
            BigInteger ab = BigInteger.Parse("455661459950917187559983225"); 
            BigInteger bb = BigInteger.Parse("99118367276030210930124383"); 

            multipleShift shiftHash = new multipleShift(a, l);
            multipleModPrime modPrimeHash = new multipleModPrime(ab, bb, l);
            Console.WriteLine(" ----Opgave 1------\n ");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            timer.Stop();
            List<int> nstørrelse = new List<int>();
            List<double> realtivtid = new List<double>();
            for (int i = 2; i < 3; i = i+50) {
                int ntest = i * (int)Math.Pow(2, l);
                nstørrelse.Add(ntest);
                var stream = CreateStream.DoCreateStream(ntest, l);

                timer.Restart();
                Console.WriteLine(checkHashSum(stream, shiftHash));
                timer.Stop();
                double shiftest1 = timer.Elapsed.TotalMilliseconds;
                //Debug.WriteLine("shifttest Time Taken: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
                timer.Restart();
                Console.WriteLine(checkHashSum(stream, modPrimeHash));
                timer.Stop();
                double modprimetest1 = timer.Elapsed.TotalMilliseconds;
                //Debug.WriteLine("modPrimeTest Time Taken: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

                Console.WriteLine("n: " + ntest + " i: " + i);
                Console.WriteLine("Shift hashvalue hastighed: " + (shiftest1/ntest) + " milliseconds pr x" +
                    "\nModPrime hashvalue hastighed: " + (modprimetest1/ntest) + " milliseconds pr x");
                Console.WriteLine("Den reeele tidsforskel: " + (shiftest1 - modprimetest1));
                Console.WriteLine("Den relative hastighedsforskel: "+ (modprimetest1/ shiftest1) +"\n");
                realtivtid.Add((modprimetest1 / shiftest1));
                }

            Console.WriteLine("n'erne: \n");
            nstørrelse.ForEach(i => Console.Write("{0}\n", i));
            Console.WriteLine("relativ hastighedsforskel: \n");
            realtivtid.ForEach(i => Console.Write("{0}\n", i));

            /*
             Opgave 3 udregning af kvadratsummer
            1) Forskellige nøgler blandt x.
             */

            //list for readable output
            List<Tuple<int, double, double, double, double, double, double>>  Runningtime = new List<Tuple<int, double, double, double, double, double, double>>();
            int nn = 3000000; //4 * il; 
            for (int i = 1; i < 22; i++)
            {
                int il = 1 << i;
                multipleShift shiftHash2 = new multipleShift(a, i);
                multipleModPrime modPrimeHash2 = new multipleModPrime(ab, bb, i);
                var stream2 = CreateStream.DoCreateStream(nn, i);
                hashingChain ShiftHashtable = new hashingChain(i, shiftHash2);
                //timing only the increment part of the code
                timer.Restart();
                foreach (var element in stream2)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ShiftHashtable.increment(x, d);
                }
                timer.Stop();
                double shiftTabletime = timer.Elapsed.TotalMilliseconds;
                hashingChain ModPrimeHashtable = new hashingChain(i, modPrimeHash2);
                //timing only the increment part of the code
                timer.Restart();
                foreach (var element in stream2)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ModPrimeHashtable.increment(x, d);
                }
                timer.Stop();
                double modprimeTabletime = timer.Elapsed.TotalMilliseconds;

                //for analysis of the result, we calculate the actual number of empty entries in the hashtables
                int emptylistShift = 0;
                int emptylistModPrime = 0;
                int maxShift = 0;
                int maxMod = 0;
                for (int m = 0; m < (int)ShiftHashtable.count(); m++){
                    int tjek = ShiftHashtable.subcount(m);
                    int tjek2 = ModPrimeHashtable.subcount(m);
                    if (tjek == 0) { emptylistShift += 1; }
                    if (tjek2 == 0) { emptylistModPrime += 1; }
                    if (tjek > maxShift) { maxShift = tjek; }
                    if (tjek2 > maxMod) { maxMod = tjek2; }
                }
                double actualemptyshift = Math.Round(emptylistShift/(double)ShiftHashtable.count(), 2, MidpointRounding.AwayFromZero);
                double actualemptyMP = Math.Round(emptylistModPrime/(double)ModPrimeHashtable.count(), 2, MidpointRounding.AwayFromZero);
                double expectedemptyShift = Math.Round((ShiftHashtable.expectedEmptyShift(il) / (double)ShiftHashtable.count()), 2, MidpointRounding.AwayFromZero);
                //ongoing console output - it is nice to follow the stream
                Console.WriteLine("Time to create Hashtable with l = " + i + " and n = " + nn + " unique x's: " + il +
                    "\nTime With Multiple Shift: " + shiftTabletime +
                    "\nEmptyness in hashtable: " + actualemptyshift * 100 + "%" +
                    "\nExpected Shift Emptyness:  " + (expectedemptyShift * 100) + "%" +
                    "\nLargest numbers in same entry: " + maxShift +
                    "\nTime With Multiple ModPrime: " + modprimeTabletime +
                    "\nEmptyness in hashtable: " + actualemptyMP * 100 + "%" +
                    "\nLargest numbers in same entry: " + maxMod +
                    "\n") ;

                //Calculating the "kvadratsum" -  not using the number  
                int S = ShiftHashtable.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Shift: " + S);
                int S2 = ModPrimeHashtable.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Mod Prime: " + S2);

                //caculating difference in time    
                double difference = Math.Round((shiftTabletime - modprimeTabletime), 2, MidpointRounding.AwayFromZero);
                double relativedif = Math.Round((modprimeTabletime / shiftTabletime), 2, MidpointRounding.AwayFromZero);
                
                Tuple<int, double, double, double, double, double, double> timestamp = Tuple.Create(i, shiftTabletime, modprimeTabletime, difference, relativedif, actualemptyshift, actualemptyMP);
                Runningtime.Add(timestamp);
                
                Console.WriteLine("\n-----------------------------------------\n");
            }

   
            Console.WriteLine("l value  " + " time Shift " + " time ModPrime " + " difference " + " relativ diff " + " empty Shift " + " empty ModPrime \n");
            //making output to copy-paste into Latex
            for (int i = 0; i < Runningtime.Count; i++)
            {
                Console.WriteLine(Runningtime[i].Item1 +  " & " + Runningtime[i].Item2 + " & " + Runningtime[i].Item3 +
                    " & " + Runningtime[i].Item4 + " & " + Runningtime[i].Item5 + " & " + (Runningtime[i].Item6 * 100) +
                    @"\%" + " & " + (Runningtime[i].Item7 * 100) + @"\%" + @" \\ \hline");
            }
            /* The following code is just to test if the table is consistent, if runned 100 times.
             * it shouldn't be runned ever again (takes forever)
            double sum12 = 0;
            for (int i = 0; i < 100; i++) {
                multipleShift shiftHash3 = new multipleShift(a, 12);
                multipleModPrime modPrimeHash3 = new multipleModPrime(ab, bb, 12);
                var stream3 = CreateStream.DoCreateStream(nn, 12);
                timer.Restart();
                hashingChain ShiftHashtable2 = new hashingChain(12, shiftHash3);
                foreach (var element in stream3)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ShiftHashtable2.increment(x, d);
                }
                timer.Stop();
                double shiftTabletime2 = timer.Elapsed.TotalMilliseconds;
                //timing only the increment part of the code
                timer.Restart();
                hashingChain ModPrimeHashtable2 = new hashingChain(14, modPrimeHash3);
                foreach (var element in stream3)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ModPrimeHashtable2.increment(x, d);
                }
                timer.Stop();
                double modprimeTabletime2 = timer.Elapsed.TotalMilliseconds;
                int S = ShiftHashtable2.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Shift: " + S);
                int S2 = ModPrimeHashtable2.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Mod Prime: " + S2);
                double relativedif2 = Math.Round((modprimeTabletime2 / shiftTabletime2), 2, MidpointRounding.AwayFromZero);
                sum12 += relativedif2; 
            }
            Console.WriteLine("gennemsnit 12 l: " + sum12/100);
            double sum20 = 0;
            for (int i = 0; i < 100; i++)
            {
                multipleShift shiftHash3 = new multipleShift(a, 20);
                multipleModPrime modPrimeHash3 = new multipleModPrime(ab, bb, 20);
                var stream3 = CreateStream.DoCreateStream(nn, 20);
                timer.Restart();
                hashingChain ShiftHashtable2 = new hashingChain(20, shiftHash3);
                foreach (var element in stream3)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ShiftHashtable2.increment(x, d);
                }
                timer.Stop();
                double shiftTabletime2 = timer.Elapsed.TotalMilliseconds;
                //timing only the increment part of the code
                timer.Restart();
                hashingChain ModPrimeHashtable2 = new hashingChain(20, modPrimeHash3);
                foreach (var element in stream3)
                {
                    ulong x = element.Item1;
                    int d = element.Item2;
                    ModPrimeHashtable2.increment(x, d);
                }
                timer.Stop();
                double modprimeTabletime2 = timer.Elapsed.TotalMilliseconds;
                int S = ShiftHashtable2.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Shift: " + S);
                int S2 = ModPrimeHashtable2.KvadratSum();
                Console.WriteLine("Kvadratsummen, med Mod Prime: " + S2);
                double relativedif2 = Math.Round((modprimeTabletime2 / shiftTabletime2), 2, MidpointRounding.AwayFromZero);
                sum20 += relativedif2;
            }
            Console.WriteLine("gennemsnit 20 l: " + sum20 / 100);
            */

            Console.WriteLine("\n\n-----COUNT SKETCH------\n\n");

            BigInteger getrandom89bits() {
                var rand = new Random();
                var bytes = new byte[12];
                rand.NextBytes(bytes);
                byte val = 128;
                int byten = bytes[11];
                bytes[11] = (byte)(byten & val);
                BigInteger bigint = new BigInteger(bytes);
                return bigint;
            }

            int lc = 12;
            int nc = 4 * (int)Math.Pow(2, lc);
            int t = 4;

            var streamForCountSketch = CreateStream.DoCreateStream(100000, 15);


            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("\nexperiment no: " + i);
                BigInteger a0 = getrandom89bits();
                BigInteger a1 = getrandom89bits();
                BigInteger a2 = getrandom89bits();
                BigInteger a3 = getrandom89bits();

                multipleShift shiftHashForCS = new multipleShift(a, 15);
                multipleModPrime modPrimeHashForCS = new multipleModPrime(ab, bb, 15);

                Console.WriteLine(checkHashSum(streamForCountSketch, shiftHashForCS));
                Console.WriteLine(checkHashSum(streamForCountSketch, modPrimeHashForCS));

                FouruHash g = new FouruHash(a0, a1, a2, a3);
                DoubleHash sh = new DoubleHash(g, t);

                CountSketch Cs = new CountSketch(sh);

                foreach (Tuple<ulong, int> elm in streamForCountSketch) {
                    Cs.Process(elm.Item1, elm.Item2);
                }

                ulong ex = Cs.EstimateS();
                Console.WriteLine("Estimator X: " + ex);

            }
        }
    }
}