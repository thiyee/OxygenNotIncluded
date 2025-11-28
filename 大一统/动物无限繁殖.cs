using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor.RegionAnalysis), "get_IsConfined", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止封闭
	{
		private static void Postfix(ref bool __result)
		{
			__result = false;
		}
	}
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor.RegionAnalysis), "get_IsOvercrowded", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止拥挤
	{
		static FieldInfo smiField = AccessTools.Field(typeof(OvercrowdingMonitor.RegionAnalysis), "smi");
		private static void Postfix(OvercrowdingMonitor.RegionAnalysis __instance,ref bool __result)
		{
			var smi = smiField.GetValue(__instance) as OvercrowdingMonitor.Instance;
			if (smi.cavity != null)
				{
					__result = smi.cavity.creatures.Count + smi.cavity.eggs.Count> 大一统.大一统控制台UI.Instance.无限繁殖上限;

				}
			
		}
	}
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor.RegionAnalysis), "get_IsFutureOvercrowded", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止蛋拥挤
	{
		static FieldInfo smiField = AccessTools.Field(typeof(OvercrowdingMonitor.RegionAnalysis), "smi");

		private static void Postfix(OvercrowdingMonitor.RegionAnalysis __instance, ref bool __result)
		{
			var smi = smiField.GetValue(__instance) as OvercrowdingMonitor.Instance;

			if (smi.cavity != null)
				{
					__result = smi.cavity.creatures.Count + smi.cavity.eggs.Count > 大一统.大一统控制台UI.Instance.无限繁殖上限;

				}
			
		}
	}
}
