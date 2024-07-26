using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 蒸汽时代
{
	[HarmonyPatch(typeof(SteamTurbineConfig2), "DoPostConfigureComplete")]
	public class 蒸汽机吸取速度
	{
		private static void Postfix(ref GameObject go)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.蒸汽时代)
			{
				SteamTurbine steamTurbine = go.AddOrGet<SteamTurbine>();
				steamTurbine.pumpKGRate *= 5f;
				steamTurbine.wasteHeatToTurbinePercent *= 0.1f;
			}
		}
	}
	[HarmonyPatch(typeof(SteamTurbine), MethodType.Constructor)]
	public class 蒸汽机吸取最小温度
	{
		private static void Postfix(ref float ___minActiveTemperature, ref float ___idealSourceElementTemperature, ref float ___maxBuildingTemperature)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.蒸汽时代)
			{
				___minActiveTemperature = 370.15f;
				___idealSourceElementTemperature = 373.15f;
				___maxBuildingTemperature = 473.15f;
			}
		}
	}
}
