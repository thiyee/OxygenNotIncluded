using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统控制台UI.强制隔热) })]

	class 强制隔热
    {
		public static void ExecuteOnInit()
		{
			GlobalBuildingConfig.CreateBuildingDef<InsulatedLiquidConduitConfig>(null, (config, __result) =>
			{
				__result.ThermalConductivity = 0f;
			});
			GlobalBuildingConfig.CreateBuildingDef<InsulatedGasConduitConfig>(null, (config, __result) =>
			{
				__result.ThermalConductivity = 0f;
			});
			GlobalBuildingConfig.CreateBuildingDef<InsulationTileConfig>(null, (config, __result) =>
			{
				__result.ThermalConductivity = 0f;
			});
		}
	}
}
