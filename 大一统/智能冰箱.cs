using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.强化冰箱) })]
	[AnyHarmonyPatch(typeof(RefrigeratorController.Def), ".ctor",Postfix:nameof(Postfix), ControlName: new string[] { nameof(大一统.大一统控制台UI.强化冰箱) })]

	public class 强化冰箱
	{
		private static void ExecuteOnInit()
		{
			GlobalBuildingConfig.DoPostConfigureComplete<RefrigeratorConfig>(null, (config, go) =>
			{
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg *= 1000f;
			});
		}
		private static void Postfix(ref float ___simulatedInternalTemperature)
		{
			___simulatedInternalTemperature = 253.15f;
		}
	}

}
