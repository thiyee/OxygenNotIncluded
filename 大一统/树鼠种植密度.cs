using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(PlantableCellQuery), ".ctor", ControlName: new string[] { nameof(大一统.大一统控制台UI.树鼠种植密度) })]
	public class 树鼠种植密度修改
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref int ___plantDetectionRadius, ref int ___maxPlantsInRadius)
		{
				___plantDetectionRadius = 100;
				___maxPlantsInRadius = 10000;
			
		}
	}
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	[AnyHarmonyPatch(typeof(SeedPlantingMonitor.Def), ".ctor", ControlName: new string[] { nameof(大一统.大一统控制台UI.树鼠种植密度) })]
	public class 树鼠种植速度修改
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref float ___searchMinInterval, ref float ___searchMaxInterval)
		{
				___searchMinInterval = 3;
				___searchMaxInterval = 6;
			
		}
	}
}
