using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 叠叠乐{
	[HarmonyPatch(typeof(BuildingDef), "IsAreaClear")]
	class 叠叠乐{
		static string[] PrefabID = new string[] { SteamTurbineConfig2.ID };
		public static void Postfix(ref BuildingDef __instance, ref bool __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.叠叠乐){
				if (PrefabID.Contains(__instance.PrefabID)) __result = true;
			}
		}
	}
}
