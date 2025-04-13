using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null,ExecuteOnInit:nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.改造储气液库) })]
	public class 改造储气液库
	{
		private static void ExecuteOnInit()
        {

			GlobalBuildingConfig.ConfigureBuildingTemplate<LiquidReservoirConfig>(null, (config, go, tag) =>
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
			});			
			GlobalBuildingConfig.ConfigureBuildingTemplate<GasReservoirConfig>(null, (config, go, tag) =>
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
			});
		}
	}

}
