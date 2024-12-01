using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Test
{
    class Program
    {

		public static void Postfix(ref MinionStartingStats __instance)
		{

		}

		static void Main(string[] args)
        {
			Harmony harmony = new Harmony("小人获得更多特质");
            Exception ex;
            var des = Memory.GetMethodStart(typeof(Program).GetMethod(nameof(Postfix)), out ex);
			return;
        }
    }
}
