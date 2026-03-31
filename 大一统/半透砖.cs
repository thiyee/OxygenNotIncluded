using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace 大一统
{
    public class 单透砖Config : IBuildingConfig
    {
        // Token: 0x060002ED RID: 749 RVA: 0x000155FC File Offset: 0x000137FC
        public override BuildingDef CreateBuildingDef()
        {
            string id = "单透砖";
            int width = 1;
            int height = 1;
            string anim = "farmtilerotating_kanim";
            int hitpoints = 100;
            float construction_time = 30f;
            float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
            string[] farmable = MATERIALS.ANY_BUILDABLE;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Tile;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
            BuildingTemplates.CreateFoundationTileDef(buildingDef);
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Overheatable = false;
            buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.DragBuild = true;
            return buildingDef;
        }

        // Token: 0x060002EE RID: 750 RVA: 0x000156C8 File Offset: 0x000138C8
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.doReplaceElement = true;
            simCellOccupier.notifyOnMelt = true;
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<AnimTileable>();
            Prioritizable.AddRef(go);
        }

        // Token: 0x060002EF RID: 751 RVA: 0x00015786 File Offset: 0x00013986
        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            SetUpFarmPlotTags(go);
        }

        public static void SetUpFarmPlotTags(GameObject go)
        {
            go.GetComponent<KPrefabID>().prefabSpawnFn += delegate (GameObject inst)
            {
                Rotatable component = inst.GetComponent<Rotatable>();
                单透砖 component2 = inst.AddOrGet<单透砖>();
                switch (component.GetOrientation())
                {
                    case Orientation.Neutral:
                    case Orientation.FlipH: component2.Direction = EightDirection.Up; return;
                    case Orientation.R90: component2.Direction = EightDirection.Right; return;
                    case Orientation.R270:
                        component2.Direction = EightDirection.Left; return;
                    case Orientation.R180:
                    case Orientation.FlipV:
                        component2.Direction = EightDirection.Down; return;
                    case Orientation.NumRotations:
                        break;
                    default:
                        return;
                }
            };
        }

        // Token: 0x040001B4 RID: 436
        public const string ID = "单透砖";
    }

    [AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", Prefix: nameof(LoadGeneratedBuildings), ControlName: new string[] { nameof(大一统.大一统控制台UI.单透砖) })]
    public class 单透砖 : KMonoBehaviour, ISaveLoadable, ISim200ms
    {
        public EightDirection Direction;
        private int cell;

        public static void LoadGeneratedBuildings()
        {
            Strings.Add("STRINGS.BUILDINGS.PREFABS.单透砖.NAME", "单透砖");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.单透砖.EFFECT", "单透砖");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.单透砖.DESC", "气体或液体仅从一个方向透过");
            ModUtil.AddBuildingToPlanScreen("Base", "单透砖");
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            cell = Grid.PosToCell(this);
            SimMessages.SetCellProperties(cell, 8);

        }

        public void Sim200ms(float dt)
        {
            int sourceCell = GetSourceCell();
            int targetCell = GetTargetCell();

            if (sourceCell == targetCell || !IsValidCell(sourceCell) || !IsValidCell(targetCell))
                return;

            float gasMass = Grid.Mass[sourceCell];
            if (gasMass > 0)
            {
                float transferMass = gasMass * 0.5f;

                if (gasMass < 1000)
                {
                    transferMass = gasMass;

                }
                SimMessages.ModifyMass(sourceCell, -transferMass, 0, 0, null, Grid.Temperature[sourceCell], Grid.Element[sourceCell].id);
                SimMessages.ModifyMass(targetCell, transferMass, 0, 0, null, Grid.Temperature[sourceCell], Grid.Element[sourceCell].id);
            }
        }

        private int GetSourceCell()
        {
            switch (Direction)
            {
                case EightDirection.Right: return Grid.CellLeft(cell);
                case EightDirection.Left: return Grid.CellRight(cell);
                case EightDirection.Up: return Grid.CellBelow(cell);
                case EightDirection.Down: return Grid.CellAbove(cell);
                default: return cell;
            }
        }

        private int GetTargetCell()
        {
            switch (Direction)
            {
                case EightDirection.Right: return Grid.CellRight(cell);
                case EightDirection.Left: return Grid.CellLeft(cell);
                case EightDirection.Up: return Grid.CellAbove(cell);
                case EightDirection.Down: return Grid.CellBelow(cell);
                default: return cell;
            }
        }

        private bool IsValidCell(int checkCell)
        {
            return Grid.IsValidCell(checkCell) && !Grid.Solid[checkCell];
        }
    }
}