using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 永不串气电解器
{
	[HarmonyPatch(typeof(ElectrolyzerConfig), "ConfigureBuildingTemplate")]
	public class 永不串气电解器
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Postfix(GameObject go)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.永不串气电解器)
			{
				Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
				electrolyzer.maxMass *= 100f;
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.consumptionRate *= 100f;
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg *= 200f;
				ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
				elementConverter.consumedElements[0].MassConsumptionRate *= 100;
				elementConverter.outputElements[0].massGenerationRate *= 100;
				elementConverter.outputElements[0].outputElementOffset.y -= 2;
				elementConverter.outputElements[1].massGenerationRate *= 100;
			}
		}
	}
}
