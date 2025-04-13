using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GeyserConfigurator;

namespace 大一统{
	[AnyHarmonyPatch(typeof(GeyserType), ".ctor", ControlName: new string[] { nameof(大一统.大一统控制台UI.修改泉属性) })]
	class 修改泉属性
	{
		public static void Postfix(ref GeyserType __instance){
			if (DlcManager.IsExpansion1Active())
			{
				__instance.minRatePerCycle *= 大一统.大一统控制台UI.Instance.喷发量;
				__instance.maxRatePerCycle *= 大一统.大一统控制台UI.Instance.喷发量;
				__instance.minIterationPercent = 大一统.大一统控制台UI.Instance.喷发期占比;
				__instance.maxIterationPercent = 大一统.大一统控制台UI.Instance.喷发期占比;
				__instance.minYearPercent = 大一统.大一统控制台UI.Instance.活跃期占比;
				__instance.maxYearPercent = 大一统.大一统控制台UI.Instance.活跃期占比;
				if (__instance.maxPressure < 500 && ElementLoader.FindElementByHash(__instance.element).IsLiquid)
				{
					__instance.maxPressure = 500f;
				}
			}
		}
	}
}
