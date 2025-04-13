using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
	[AnyHarmonyPatch(typeof(BaseBeeHiveConfig), "CreatePrefab", ControlName: new string[] { nameof(大一统.大一统控制台UI.辐射蜂巢耐热) })]
	public class 辐射蜂巢耐热
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Postfix(ref GameObject __result)
		{
				__result.AddOrGet<TemperatureVulnerable>().Configure(273.15f - 90f, 273.15f - 90f, 273.15f + 90f, 273.15f + 90f);
		}
	}
}
