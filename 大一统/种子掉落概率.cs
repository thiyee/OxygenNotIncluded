using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 种子掉落概率
{
    [AnyHarmonyPatch(typeof(SeedProducer), "CropPicked")]
    public class 种子掉落概率{
		public static bool Prefix(SeedProducer __instance,object data){
			if (__instance.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				WorkerBase completed_by = __instance.GetComponent<Harvestable>().completed_by;
				float num = 1f;
				if (completed_by != null)
				{
					num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
				}
				int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
				if (num2 > 0){
					MethodInfo ProduceSeed= typeof(SeedProducer).GetMethod("ProduceSeed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
					object[] param = { __instance.seedInfo.seedId, num2, true};
					((GameObject)ProduceSeed.Invoke(__instance, param)).Trigger(580035959, completed_by);
				}
				return false;
			}
			return true;
		}

	}
}
