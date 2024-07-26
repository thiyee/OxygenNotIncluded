using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 辐射蜂巢耐热
{
	[HarmonyPatch(typeof(BaseBeeHiveConfig), "CreatePrefab")]
	public class 辐射蜂巢耐热
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Postfix(ref GameObject __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.辐射蜂巢耐热)
				__result.AddOrGet<TemperatureVulnerable>().Configure(273.15f - 90f, 273.15f - 90f, 273.15f + 90f, 273.15f + 90f);
		}
	}
}
