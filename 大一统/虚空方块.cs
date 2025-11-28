using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace 虚空方块
{
	[AnyHarmonyPatch(typeof(BuildingComplete), "OnSpawn", Postfix: nameof(OnSpawn), ControlName: new string[] { nameof(大一统.大一统控制台UI.虚空方块) })]
	[AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", Prefix: nameof(LoadGeneratedBuildings), ControlName: new string[] { nameof(大一统.大一统控制台UI.虚空方块) })]
	[AnyHarmonyPatch(typeof(SaveGame), "OnPrefabInit",Postfix:nameof(OnPrefabInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.虚空方块) })]
	[AnyHarmonyPatch(typeof(SubworldZoneRenderData), "GenerateTexture", Postfix:nameof(GenerateTexture), ControlName: new string[] { nameof(大一统.大一统控制台UI.虚空方块) })]
	public class 虚空方块
	{
		public class 虚空方块Data : KMonoBehaviour, ISaveLoadable,ISaveLoadableDetails
        {
			public static HashSet<int> SpaceExposure=new HashSet<int>();

            public void Deserialize(IReader reader)
            {
				SpaceExposure.Clear();

				int count = reader.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					int cell = reader.ReadInt32();
					SpaceExposure.Add(cell);
				}
			}

			public void Serialize(BinaryWriter writer)
            {
				writer.Write(SpaceExposure.Count);
				foreach (int cell in SpaceExposure){
					writer.Write(cell);
				}
			}


        }

		public static void OnPrefabInit(SaveGame __instance)
		{
			__instance.gameObject.AddOrGet<虚空方块Data>();
		}
		public static void GenerateTexture(SubworldZoneRenderData __instance)
        {
			//如果网格原本就是太空暴露的就不再应用
			虚空方块Data.SpaceExposure.RemoveWhere(cell => __instance.worldZoneTypes[cell] == SubWorld.ZoneType.Space);
			foreach (var cell in 虚空方块Data.SpaceExposure)
            {
				SimMessages.ModifyCellWorldZone(cell, byte.MaxValue);
				__instance.worldZoneTypes[cell] = SubWorld.ZoneType.Space;
			}
		}
		public static void OnSpawn(BuildingComplete __instance)
		{
			GameObject gameObject = __instance.gameObject; 

			if (__instance.Def.PrefabID == 虚空方块Config.ID)
			{
				TracesExtesions.DeleteObject(gameObject);
				int num = Grid.PosToCell(gameObject);



				if (global::World.Instance.zoneRenderData.worldZoneTypes[num]!= SubWorld.ZoneType.Space)
                {
					虚空方块Data.SpaceExposure.Add(num);
					SimMessages.ModifyCellWorldZone(num, byte.MaxValue);
					global::World.Instance.zoneRenderData.worldZoneTypes[num] = SubWorld.ZoneType.Space;
                }
                else
                {
                    if (虚空方块Data.SpaceExposure.Contains(num))
                    {
						虚空方块Data.SpaceExposure.Remove(num);
						SimMessages.ModifyCellWorldZone(num, (byte)SubWorld.ZoneType.Sandstone);
						global::World.Instance.zoneRenderData.worldZoneTypes[num] = SubWorld.ZoneType.Sandstone;

					}
				}
			}
		}
		public static void LoadGeneratedBuildings()
		{
			Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.虚空方块.NAME", "虚空方块" });
			Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.虚空方块.EFFECT", "虚空方块" });
			Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.虚空方块.DESC", "使当前位置成为太空暴露,重复建造可移除" });
			ModUtil.AddBuildingToPlanScreen("Base", "虚空方块");

		}



        public class 虚空方块Config : IBuildingConfig
		{
			public override BuildingDef CreateBuildingDef()
			{
				string id = ID;
				int width = 1;
				int height = 1;
				string anim = "walls_kanim";
				int hitpoints = 1;
				float construction_time = 3f;

				Element[] elements = ElementLoader.elements.ToArray();

				string[] raw_MINERALS = elements.Select(e => e.id.ToString()).ToArray();
				float[] tier = elements.Select(e => e.maxMass).ToArray();

				for (int i = 0; i < elements.Length; i++)
				{
					if (elements[i].IsGas) tier[i] = 1;
					else if (elements[i].IsLiquid) tier[i] = 100;
					else if (elements[i].IsSolid) tier[i] = 1000;
					else tier[i] = 500;
				}
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

			public override void DoPostConfigureComplete(GameObject go)
			{
				GeneratedBuildings.RemoveLoopingSounds(go);
			}
			public static string ID = "虚空方块";

		}

	}

}
