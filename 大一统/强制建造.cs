using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 强制建造
{
	class 强制建造
	{

		static MethodInfo[] IsAreaClear = typeof(BuildingDef).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.Name == "IsAreaClear").ToArray();
		public static void Postfix(ref BuildingDef __instance, ref bool __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.强制建造)
				if (((int)GetAsyncKeyState(VK_SHIFT) & KEY_PRESSED) != 0) __result = true;
			Console.WriteLine("IsAreaClear hook!");
		}

		public 强制建造(){

			Harmony harmony = new Harmony("强制建造");
			foreach (MethodInfo methodInfo in IsAreaClear){
				harmony.Patch(methodInfo, postfix: new HarmonyMethod(typeof(强制建造), nameof(Postfix)));
				Console.WriteLine("Patch " + methodInfo.Name);
			}


		}

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int vKey);
		const int VK_SHIFT = 0x10;
		const int KEY_PRESSED = 0x8000;
	}
}