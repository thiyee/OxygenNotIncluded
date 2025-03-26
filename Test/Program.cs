using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using 效果修改;

namespace Test
{
    class Program
    {

		public static void Postfix(ref MinionStartingStats __instance)
		{

		}

		static void Main(string[] args)
        {
            MemberInfo[] memberInfo = typeof(Klei.AI.AttributeModifier).GetMembers().Where(m => m.Name == ".ctor").ToArray();

            var method = typeof(效果修改.效果修改).GetMethod("AttributeModifier", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var h=new HarmonyMethod(method);

            Type targetType = typeof(Klei.AI.AttributeModifier);
            string methodName = ".ctor";
            MethodBase[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            methods.Concat(targetType.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
            methods = methods.Where(m => m.Name == methodName).ToArray();

            return;
        }
    }
}
