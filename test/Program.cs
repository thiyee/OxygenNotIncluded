using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static string[] aaaa=new string[]{"asdasd"};
        public static string[] asd()
        {
            return aaaa;
        }
        public static void Main(string[] args)
        {
            var DeriveRecipiesFromSource = typeof(OvercrowdingMonitor.RegionAnalysis).GetMembers(AccessTools.all);

            return;
            
        }
    }
}
