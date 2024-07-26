using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 强制隔热
{
    class 强制隔热
    {
		[HarmonyPatch(typeof(InsulatedLiquidConduitConfig), "CreateBuildingDef")]
		public class 绝热液体管道
		{
			private static void Postfix(ref BuildingDef __result)
			{
				__result.ThermalConductivity = 0f;
			}
		}

		[HarmonyPatch(typeof(InsulatedGasConduitConfig), "CreateBuildingDef")]
		public class 绝热气体管道
		{
			private static void Postfix(ref BuildingDef __result)
			{
				__result.ThermalConductivity = 0f;
			}
		}

		[HarmonyPatch(typeof(InsulationTileConfig), "CreateBuildingDef")]
		public class 绝热砖
		{
			private static void Postfix(ref BuildingDef __result)
			{
				__result.ThermalConductivity = 0f;
			}
		}
	}
}
