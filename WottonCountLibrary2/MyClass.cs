using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using WottonFederhenCountLibrary;
namespace WottonCountLibrary2
{
    public class BinomCount : WottonCount
    {
        char[] nucl;

        public BinomCount() { }
        public BinomCount(char[] nucl):base(nucl){}
        public static void BinomCountWF(string s, int k){
            Console.WriteLine("Yea");
        }

    }
}
