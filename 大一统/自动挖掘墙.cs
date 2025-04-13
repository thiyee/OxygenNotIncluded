using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace 大一统{
	public class AutoMinerWall : StateMachineComponent<AutoMinerWall.Instance>, ISim1000ms{
		int digcell;
		public float damage;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			GameObject gameObject = base.gameObject;
			digcell = Grid.PosToCell(gameObject);

		}
		public void Sim1000ms(float dt)
		{
            if (Grid.Element[digcell].IsSolid){
				Diggable.DoDigTick(digcell, damage, WorldDamage.DamageType.NoBuildingDamage);
			}
		}
		public class Instance : GameStateMachine<AutoMinerWall.States, AutoMinerWall.Instance, AutoMinerWall, object>.GameInstance
		{
			public Instance(AutoMinerWall master) : base(master)
			{
			}
		}
		public class States : GameStateMachine<AutoMinerWall.States, AutoMinerWall.Instance, AutoMinerWall>
		{
			// Token: 0x06007F74 RID: 32628 RVA: 0x002F5C28 File Offset: 0x002F3E28
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.on;
			}
			public AutoMiner.States.ReadyStates on;
		}


	}


	public class AutoDiggingWallConfig : IBuildingConfig
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0008C60C File Offset: 0x0008A80C
		public override BuildingDef CreateBuildingDef()
		{
			string id = "自动挖掘墙";
			int width = 1;
			int height = 1;
			string anim = "walls_kanim";
			int hitpoints = 1;
			float construction_time = 3f;

			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[] { 1000 }, MATERIALS.ANY_BUILDABLE, melting_point, build_location_rule, new EffectorValues
			{
				amount = 10,
				radius = 0
			}, none, 0.2f);
			buildingDef.Entombable = false;
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;

			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DefaultAnimState = "off";
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
			buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
			buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>
		{
			ObjectLayer.FoundationTile,
			ObjectLayer.Backwall
		};
			buildingDef.ReplacementTags = new List<Tag>
		{
			GameTags.FloorTiles,
			GameTags.Backwall
		};
			return buildingDef;
		}
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall, false);
			GeneratedBuildings.RemoveLoopingSounds(go);
			AutoMinerWall autoMiner = go.AddOrGet<AutoMinerWall>();
			autoMiner.damage = 10;
		}
		[AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", ControlName: new string[] { nameof(大一统.大一统控制台UI.自动挖掘墙) })]
		class 添加建筑
		{
			public static void Prefix()
			{
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.自动挖掘墙.NAME", "自动挖掘墙" });
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.自动挖掘墙.EFFECT", "自动挖掘墙" });
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.自动挖掘墙.DESC", "自动挖掘覆盖在其上的固体" });
					ModUtil.AddBuildingToPlanScreen("Base", "自动挖掘墙");
				
			}
		}
		public const string ID = "自动挖掘墙";
	}
}
