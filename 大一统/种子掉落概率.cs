using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(typeof(SeedProducer), "CropPicked",ControlName:new string[] { nameof(大一统.大一统控制台UI.种子掉落概率) })]
    public class 种子掉落概率{
		static MethodInfo ProduceSeed = typeof(SeedProducer).GetMethod("ProduceSeed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

		public static bool Prefix(SeedProducer __instance,object data){
			if (__instance.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				WorkerBase completed_by = __instance.GetComponent<Harvestable>().completed_by;
				float num = (float)大一统.大一统控制台UI.Instance.种子掉落概率/100f;
				if (completed_by != null)
				{
					num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
				}
				int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
				if (num2 > 0){
					object[] param = { __instance.seedInfo.seedId, num2, true};
					((GameObject)ProduceSeed.Invoke(__instance, param)).Trigger((int)GameHashes.WorkableEntombOffset, completed_by);
				}
				return false;
			}
			return true;
		}

	}
}
