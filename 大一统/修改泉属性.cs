using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GeyserConfigurator;

namespace 修改泉属性
{
	[HarmonyPatch(typeof(GeyserConfigurator.GeyserType), MethodType.Constructor, new Type[] { typeof(string), typeof(SimHashes), typeof(GeyserShape), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(string) })]
	class 修改泉属性
	{
		public static void Prefix(ref string id, ref SimHashes element, ref GeyserShape shape, ref float temperature, ref float minRatePerCycle, ref float maxRatePerCycle, ref float maxPressure, ref float minIterationLength, ref float maxIterationLength, ref float minIterationPercent, ref float maxIterationPercent, ref float minYearLength, ref float maxYearLength, ref float minYearPercent, ref float maxYearPercent, ref float geyserTemperature, ref string DlcID)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.修改泉属性&& DlcManager.IsExpansion1Active())
			{
				//if (minIterationLength > 1000)
				//{
				//	minIterationLength = 480f;
				//	maxIterationLength = 1080f;
				//}

				minRatePerCycle *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发量;
				maxRatePerCycle *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发量;
				minIterationPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发期占比;
				maxIterationPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.喷发期占比;
				//minYearLength *= 0.1f;
				//maxYearLength *= 0.1f;
				minYearPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.活跃期占比;
				maxYearPercent = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.活跃期占比;
				if (maxPressure < 500 && ElementLoader.FindElementByHash(element).IsLiquid)
				{
					maxPressure = 500f;
				}
			}
		}
	}
}
