using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor), "IsConfined", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止封闭
	{
		private static void Postfix(ref bool __result)
		{
			__result = false;
		}
	}
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor), "IsOvercrowded", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止拥挤
	{
		private static void Postfix(ref bool __result, OvercrowdingMonitor.Instance smi)
		{
				if(smi.cavity != null)
				{
					__result = smi.cavity.creatures.Count + smi.cavity.eggs.Count> 大一统.大一统控制台UI.Instance.无限繁殖上限;

				}
			
		}
	}
	[AnyHarmonyPatch(typeof(OvercrowdingMonitor), "IsFutureOvercrowded", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物无限繁殖) })]
	public class 动物防止蛋拥挤
	{
		private static void Postfix(OvercrowdingMonitor.Instance smi, ref bool __result)
		{
				if (smi.cavity != null)
				{
					__result = smi.cavity.creatures.Count + smi.cavity.eggs.Count > 大一统.大一统控制台UI.Instance.无限繁殖上限;

				}
			
		}
	}
}
