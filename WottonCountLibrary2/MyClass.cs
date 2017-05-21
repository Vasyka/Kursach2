﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using WottonFederhenCountLibrary;
namespace WottonCountLibrary2
{
    public class BinomCount : WottonCount
    {
        
        public static Dictionary<int,double> LnFactTable(int k)
        {
            Dictionary<int, double> hash = new Dictionary<int, double>();
            hash.Add(0,Math.Log(1,4));
            Console.WriteLine(0 + " " + hash[0]);
            long n = 1;
            for (int i = 1; i <= k; i++)
            {
                hash.Add(i,hash[i-1] + Math.Log(i,4));
                n = n * i;
            }
            return hash;
        }
        public BinomCount() { }
        public BinomCount(char[] nucl):base(nucl){}
        public static void BinomCountWF(string s, int k){
            Console.WriteLine("Yea");
            Dictionary<int, double> hash = LnFactTable(k);
            Console.WriteLine("В хэше:");
            foreach(var e in hash){
                Console.WriteLine(e.Key + " " + e.Value);
            }
			char[] nucl = { 'A', 'T', 'G', 'C' };
           /* int[][] nuclk = new int[k][];
            nuclk[0] = new int[nucl.Length];
            for (int i = 0,j = 0; i < s.Length; i++){
                nuclk[j][CountLetter(nucl,s[i])]++;
                nuclk[k + j][CountLetter(nucl, s[i])]--;
            }*/

        }

    }
}
