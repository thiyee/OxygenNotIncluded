using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null,ExecuteOnInit:nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.按摩床恢复速度) })]
	public class 按摩床恢复速度{
		public static void ExecuteOnInit()
		{
			GlobalBuildingConfig.DoPostConfigureComplete<MassageTableConfig>(null, (config, go) =>
			{
					MassageTable massageTable = go.GetComponent<MassageTable>();
					massageTable.stressModificationValue *= 10f;                //按摩床效率*10
					massageTable.roomStressModificationValue *= 10f;                //按摩床效率*10
			});
		}		
	}
}
