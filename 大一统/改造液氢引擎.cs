using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 改造液氢引擎
{
	[HarmonyPatch(typeof(HydrogenEngineClusterConfig), "CreateBuildingDef")]
	public class 液氢引擎改造1
	{
		private static void Postfix(ref BuildingDef __result)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.改造液氢引擎)
			{
				__result.UtilityInputOffset = new CellOffset(2, 3);
				__result.InputConduitType = ConduitType.Liquid;
			}
		}
	}
	[HarmonyPatch(typeof(HydrogenEngineClusterConfig), "DoPostConfigureComplete")]
	public class 液氢引擎改造2
	{
		private static void Postfix(ref GameObject go)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.改造液氢引擎) { 
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
		} }
	}
}
