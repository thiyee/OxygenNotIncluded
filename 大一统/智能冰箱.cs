using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 智能冰箱
{
	[HarmonyPatch(typeof(RefrigeratorConfig), "DoPostConfigureComplete")]
	public class 冰箱储存容量
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Postfix(GameObject go)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.智能冰箱)
			{
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg *= 1000f;
			}
		}
	}
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	[HarmonyPatch(typeof(RefrigeratorController.Def), MethodType.Constructor)]
	public class 冰箱储存温度
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref float ___simulatedInternalTemperature)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.智能冰箱)
				___simulatedInternalTemperature = 253.15f;
		}
	}
}
