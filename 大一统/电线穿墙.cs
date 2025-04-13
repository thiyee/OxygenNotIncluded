using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null,null,ExecuteOnInit:nameof(ExecuteOnInit),ControlName: new string[] { nameof(大一统控制台UI.电线穿墙)})]
	public class 电线穿墙
	{
		public static void ExecuteOnInit()
        {
			GlobalBuildingConfig.CreateBuildingDef<WireHighWattageConfig>(null, (config, __result) =>
			{
				__result.BuildLocationRule = BuildLocationRule.Anywhere;

			});			
			GlobalBuildingConfig.CreateBuildingDef<WireRefinedHighWattageConfig>(null, (config, __result) =>
			{
				__result.BuildLocationRule = BuildLocationRule.Anywhere;

			});
		}
	}
}
