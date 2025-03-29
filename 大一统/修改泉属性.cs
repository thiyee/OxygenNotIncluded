using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GeyserConfigurator;

namespace 修改泉属性
{
	[AnyHarmonyPatch(typeof(GeyserType), ".ctor")]
	class 修改泉属性
	{
		public static void Postfix(ref GeyserType __instance){
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.修改泉属性&& DlcManager.IsExpansion1Active())
			{
				__instance.minRatePerCycle *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发量;
				__instance.maxRatePerCycle *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发量;
				__instance.minIterationPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发期占比;
				__instance.maxIterationPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发期占比;
				__instance.minYearPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.活跃期占比;
				__instance.maxYearPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.活跃期占比;
				if (__instance.maxPressure < 500 && ElementLoader.FindElementByHash(__instance.element).IsLiquid)
				{
					__instance.maxPressure = 500f;
				}
			}
		}
	}
}
