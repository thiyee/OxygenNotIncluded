using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统控制台UI.蒸汽时代) })]
	[AnyHarmonyPatch(typeof(SteamTurbine), ".ctor",Postfix:nameof(SteamTurbine), ControlName: new string[] { nameof(大一统.大一统控制台UI.蒸汽时代) })]

	public class 蒸汽时代
	{
		public static void ExecuteOnInit()
		{
			GlobalBuildingConfig.DoPostConfigureComplete<SteamTurbineConfig2>(null, (config, go) =>
			{
				SteamTurbine steamTurbine = go.AddOrGet<SteamTurbine>();
				steamTurbine.pumpKGRate *= 5f;
				steamTurbine.wasteHeatToTurbinePercent *= 0.1f;
			});
		}
		private static void SteamTurbine(ref float ___minActiveTemperature, ref float ___idealSourceElementTemperature, ref float ___maxBuildingTemperature)
		{
			___minActiveTemperature = 370.15f;
			___idealSourceElementTemperature = 373.15f;
			___maxBuildingTemperature = 473.15f;

		}
	}

}
