using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.冷光吸顶灯) })]
	public class 冷光吸顶灯
	{
		private static void ExecuteOnInit()
		{
			GlobalBuildingConfig.CreateBuildingDef<CeilingLightConfig>(null, (config, __result) =>
			{
				if (config is CeilingLightConfig)
				{
					__result.SelfHeatKilowattsWhenActive = 0;

				}
			});
		}
	}
}
