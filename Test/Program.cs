using HarmonyLib;
using Klei.AI;
using ProcGenGame;
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
             FieldInfo renderedByWorld = typeof(Substance).GetField("renderedByWorld", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            return;
        }
    }
}
