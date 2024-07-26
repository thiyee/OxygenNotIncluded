using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 冷光吸顶灯
{
	[HarmonyPatch(typeof(CeilingLightConfig), "CreateBuildingDef")]
	public class 冷光吸顶灯
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		static void Postfix(CeilingLightConfig __instance, ref BuildingDef __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.冷光吸顶灯)
			{
				__result.SelfHeatKilowattsWhenActive = 0;
			}
		}
	}
}
