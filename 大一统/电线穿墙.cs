using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 电线穿墙
{
	[HarmonyPatch(typeof(WireHighWattageConfig), "CreateBuildingDef")]
	public class 电线穿墙
	{
		private static void Postfix(ref BuildingDef __result)
		{
            if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.电线穿墙)
            {
				__result.BuildLocationRule = BuildLocationRule.Anywhere;


			}
		}
	}	
	[HarmonyPatch(typeof(WireRefinedHighWattageConfig), "CreateBuildingDef")]
	public class 电线穿墙2
	{
		private static void Postfix(ref BuildingDef __result)
		{
            if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.电线穿墙)
            {
				__result.BuildLocationRule = BuildLocationRule.Anywhere;


			}
		}
	}
}
