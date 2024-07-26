using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 树鼠种植密度
{
	[HarmonyPatch(typeof(PlantableCellQuery), MethodType.Constructor)]
	public class 树鼠种植密度修改
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref int ___plantDetectionRadius, ref int ___maxPlantsInRadius)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.树鼠种植密度)
			{
				___plantDetectionRadius = 100;
				___maxPlantsInRadius = 10000;
			}
		}
	}
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	[HarmonyPatch(typeof(SeedPlantingMonitor.Def), MethodType.Constructor)]
	public class 树鼠种植速度修改
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref float ___searchMinInterval, ref float ___searchMaxInterval)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.树鼠种植密度)
			{
				___searchMinInterval = 3;
				___searchMaxInterval = 6;
			}
		}
	}
}
