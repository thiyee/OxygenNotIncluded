using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using 大一统;

namespace 大一统
{
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
                //Storage storage = go.AddOrGet<Storage>();
                //storage.capacityKg = 10f * TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
                //storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
                //{
                //    Storage.StoredItemModifier.Hide,
                //    Storage.StoredItemModifier.Seal,
                //    Storage.StoredItemModifier.Insulate
                //});
                //FuelTank fuelTank = go.AddOrGet<FuelTank>();
                //fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
                //fuelTank.storage = storage;
                //fuelTank.physicalFuelCapacity = storage.capacityKg;
                //go.AddOrGet<CopyBuildingSettings>();
                //go.AddOrGet<DropToUserCapacity>();
                //ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
                //manualDeliveryKG.SetStorage(storage);
                //manualDeliveryKG.refillMass = storage.capacityKg;
                //manualDeliveryKG.capacity = storage.capacityKg;
                //manualDeliveryKG.operationalRequirement = Operational.State.None;
                //manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
                //ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
                //conduitConsumer.conduitType = ConduitType.Liquid;
                //conduitConsumer.consumptionRate = 10f;
                //conduitConsumer.capacityTag = GameTags.Liquid;
                //conduitConsumer.capacityKG = storage.capacityKg;
                //conduitConsumer.forceAlwaysSatisfied = true;
                //conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
                //BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, 0f, 0f);
                //storage.showUnreachableStatus = false;
                //go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject inst)
                //{
                //    Element element = ElementLoader.FindElementByHash(SimHashes.LiquidOxygen);
                //    if (!DiscoveredResources.Instance.IsDiscovered(element.tag))
                //    {
                //        DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
                //    }
                //};

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
