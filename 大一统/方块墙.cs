using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x020000B9 RID: 185
namespace 大一统{
	public class ExteriorWallConfig : IBuildingConfig
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0008C60C File Offset: 0x0008A80C
		public override BuildingDef CreateBuildingDef()
		{
			string id = "方块墙";
			int width = 1;
			int height = 1;
			string anim = "walls_kanim";
			int hitpoints = 1;
			float construction_time = 3f;
			
			Element[] elements=ElementLoader.elements.ToArray();

			string[] raw_MINERALS = elements.Select(e => e.id.ToString()).ToArray();
			float[] tier = elements.Select(e => e.maxMass).ToArray();

			for (int i = 0; i < elements.Length; i++){
				if (elements[i].IsGas) tier[i] = 1;
				else if (elements[i].IsLiquid) tier[i] = 100;
				else if (elements[i].IsSolid) tier[i] = 1000;
				else tier[i] = 500;
			}
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[]{1000}, MATERIALS.ANY_BUILDABLE, melting_point, build_location_rule, new EffectorValues
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
			//buildingDef.Mass = new float[] { 666.66666666f };
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



        // Token: 0x060002EC RID: 748 RVA: 0x00003769 File Offset: 0x00001969
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00003799 File Offset: 0x00001999
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall, false);
			GeneratedBuildings.RemoveLoopingSounds(go);
		}

		[AnyHarmonyPatch(typeof(BuildingComplete), "OnSpawn", ControlName: new string[] { nameof(大一统.大一统控制台UI.方块墙) })]
		public static class 方块墙建造Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				GameObject gameObject = __instance.gameObject;
				if (__instance.Def.PrefabID == "方块墙")
				{

					PrimaryElement element = gameObject.GetComponent<PrimaryElement>();
					float temperature = element.Temperature;

					TracesExtesions.DeleteObject(gameObject);
					SimMessages.ReplaceAndDisplaceElement(Grid.PosToCell(gameObject.transform.position), element.ElementID, null, __instance.Def.Mass[0], temperature, byte.MaxValue, 0, -1);
				}
			}

		}
		[AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", ControlName: new string[] { nameof(大一统.大一统控制台UI.方块墙) })]
		class 添加建筑
		{
			public static void Prefix()
			{
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.方块墙.NAME", "方块墙" });
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.方块墙.EFFECT", "方块墙" });
					Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.方块墙.DESC", "建造一个自然土块" });
					ModUtil.AddBuildingToPlanScreen("Base", "方块墙");
				
			}
		}

		public const string ID = "方块墙";

    }
}