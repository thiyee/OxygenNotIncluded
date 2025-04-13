using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using 大一统;

namespace 大一统{
	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.改造液氢引擎) })]
	public class 改造液氢引擎
	{
		private static void ExecuteOnInit()
		{
			GlobalBuildingConfig.CreateBuildingDef<HydrogenEngineClusterConfig>(null, (config, __result) =>
			{
				__result.UtilityInputOffset = new CellOffset(2, 3);
				__result.InputConduitType = ConduitType.Liquid;
			});			
			GlobalBuildingConfig.DoPostConfigureComplete<HydrogenEngineClusterConfig>(null, (config, go) =>
			{
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg = 10f * TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
				storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
					{
						Storage.StoredItemModifier.Hide,
						Storage.StoredItemModifier.Seal,
						Storage.StoredItemModifier.Insulate
					});
				FuelTank fuelTank = go.AddOrGet<FuelTank>();
				fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
				fuelTank.storage = storage;
				fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
				fuelTank.physicalFuelCapacity = storage.capacityKg;
				go.AddOrGet<CopyBuildingSettings>();
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.conduitType = ConduitType.Liquid;
				conduitConsumer.consumptionRate = 1000f;
				conduitConsumer.capacityTag = fuelTank.FuelType;
				conduitConsumer.capacityKG = storage.capacityKg;
				conduitConsumer.forceAlwaysSatisfied = true;
				conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
			});
		}
	}
}
