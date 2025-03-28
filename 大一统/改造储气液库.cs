﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace 改造储气液库
{
	[AnyHarmonyPatch(typeof(LiquidReservoirConfig), "ConfigureBuildingTemplate")]
	public class 储液库修改
	{
		private static void Postfix(ref GameObject go, ref Tag prefab_tag)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.改造储气液库)
			{
				Prioritizable.AddRef(go);
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg = 1000000f;
				storage.allowItemRemoval = true;
				storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
				storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = storage.capacityKg;
				go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
				go.AddOrGet<StorageLocker>();
				go.AddOrGet<UserNameable>();
				go.AddOrGetDef<RocketUsageRestriction.Def>();
			}
		}
	}

	[AnyHarmonyPatch(typeof(GasReservoirConfig), "ConfigureBuildingTemplate")]
	public class 储气库修改
	{
		private static void Postfix(ref GameObject go, ref Tag prefab_tag)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.改造储气液库)
			{
				Prioritizable.AddRef(go);
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg = 1000000f;
				storage.allowItemRemoval = true;
				storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
				storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = storage.capacityKg;
				go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
				go.AddOrGet<StorageLocker>();
				go.AddOrGet<UserNameable>();
				go.AddOrGetDef<RocketUsageRestriction.Def>();
			}
		}
	}

}
