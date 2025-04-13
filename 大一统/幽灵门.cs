using UnityEngine;
using TUNING;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using System;

public class GhostDoorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        string id = GhostDoorConfig.ID;
        int width = 1;
        int height = 2;
        string anim = "door_external_kanim";
        int hitpoints = 30;
        float construction_time = 60f;
        float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
        string[] all_METALS = MATERIALS.ANY_BUILDABLE;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.Tile;
        EffectorValues none = NOISE_POLLUTION.NONE;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 1f);
        buildingDef.Overheatable = false;
        //buildingDef.RequiresPowerInput = true;
        //buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.Floodable = false;
        buildingDef.Entombable = false;
        buildingDef.IsFoundation = true;
        buildingDef.TileLayer = ObjectLayer.FoundationTile;
        buildingDef.AudioCategory = "Metal";
        buildingDef.PermittedRotations = PermittedRotations.R90;
        buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
        buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
        buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
        SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
        SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
        return buildingDef;
    }
    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
        simCellOccupier.setLiquidImpermeable = true;
        simCellOccupier.setGasImpermeable = true;
        simCellOccupier.doReplaceElement = false;
    }
    public override void DoPostConfigureComplete(GameObject go)
    {
        Door door = go.AddOrGet<Door>();
        door.hasComplexUserControls = true;
        door.unpoweredAnimSpeed = 20f;
        door.poweredAnimSpeed = 20f;
        door.doorClosingSoundEventName = "MechanizedAirlock_closing";
        door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
        go.AddOrGet<ZoneTile>();
        go.AddOrGet<AccessControl>();
        go.AddOrGet<KBoxCollider2D>();
        Prioritizable.AddRef(go);
        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
        go.AddOrGet<Workable>().workTime = 0f;
        UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
        go.GetComponent<AccessControl>().controlEnabled = true;
        go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
    }

    public const string ID = "GhostDoor";
}




[HarmonyPatch(typeof(HighEnergyParticle),nameof(HighEnergyParticle.CheckCollision))]
class 辐射粒子碰撞Patch
{
    public static bool Prefix(HighEnergyParticle __instance)
    {
        int cell = Grid.PosToCell(__instance.smi.master.transform.GetPosition());
        if (Grid.IsSolidCell(cell))
        {
            GameObject gameObject5 = Grid.Objects[cell, 9];
            if (gameObject5!=null&&gameObject5.name.Contains(GhostDoorConfig.ID))
            {
                return false;
            }
        }
        return true;
    }
}


[AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", ControlName: new string[] { nameof(大一统.大一统控制台UI.幽灵门) })]
class 添加建筑
{
    public static void Prefix()
    {
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.GHOSTDOOR.NAME", "幽灵门" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.GHOSTDOOR.EFFECT", "幽灵门" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.GHOSTDOOR.DESC", "复制人可通过 但气体与液体不可通过的门" });
            ModUtil.AddBuildingToPlanScreen("Base", GhostDoorConfig.ID);
            
        
    }
}