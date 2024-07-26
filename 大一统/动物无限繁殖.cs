using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 动物无限繁殖
{
	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsConfined")]
	public class 动物防止封闭
	{
		private static void Postfix(ref bool __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物无限繁殖)
				__result = false;
		}
	}
	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsOvercrowded")]
	public class 动物防止拥挤
	{
		private static void Postfix(ref bool __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物无限繁殖)
				__result = false;
		}
	}
	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsFutureOvercrowded")]
	public class 动物防止蛋拥挤
	{
		private static void Postfix(ref bool __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物无限繁殖)
				__result = false;
		}
	}
}
