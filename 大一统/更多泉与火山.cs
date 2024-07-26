using HarmonyLib;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateClasses;
using static ProcGen.World;
using System.Reflection;
using static ProcGen.World.TemplateSpawnRules;
using static ProcGen.World.AllowedCellsFilter;

namespace 更多泉与火山
{
	public class 更多泉与火山
	{
		static List<Cell> 火山填充物 = new List<Cell> {
			new Cell(-1,-2,SimHashes.Unobtanium,1,20000,null,0,false),
			new Cell(0 ,-2,SimHashes.Unobtanium,1,20000,null,0,false),
			new Cell(1 ,-2,SimHashes.Unobtanium,1,20000,null,0,false),
			new Cell(2 ,-2,SimHashes.Unobtanium,1,20000,null,0,false),
			new Cell(-1,-1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(0 ,-1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(1 ,-1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(2 ,-1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(-1,0,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(0 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(1 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(2 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(-1,1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(0 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(1 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(2 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(-1,2,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(0 ,2,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(1 ,2,SimHashes.Katairite,273.15f,500,null,0,false),
			new Cell(2 ,2,SimHashes.Katairite,273.15f,500,null,0,false)
		};
		static List<Cell> 泉填充物 = new List<Cell> {
					new Cell(-1,-1,SimHashes.Unobtanium,1,20000,null,0,false),
					new Cell(0 ,-1,SimHashes.Unobtanium,1,20000,null,0,false),
					new Cell(1 ,-1,SimHashes.Unobtanium,1,20000,null,0,false),
					new Cell(2 ,-1,SimHashes.Unobtanium,1,20000,null,0,false),
					new Cell(-1,0,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(0 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(1 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(2 ,0,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(-1,1,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(0 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(1 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(2 ,1,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(-1,2,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(0 ,2,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(1 ,2,SimHashes.Katairite,273.15f,500,null,0,false),
					new Cell(2 ,2,SimHashes.Katairite,273.15f,500,null,0,false)
				};
		static List<Prefab>[] gprefabs1 = new List<Prefab>[]{
					new List<Prefab> {new Prefab("GeyserGeneric_hot_water", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },
					new List<Prefab> {new Prefab("GeyserGeneric_super_coolant", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },
					new List<Prefab> {new Prefab("GeyserGeneric_LiquidPhosphorus", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },
					new List<Prefab> {new Prefab("GeyserGeneric_Petroleum", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },
					new List<Prefab> {new Prefab("GeyserGeneric_molten_sucrose", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },
					new List<Prefab> {new Prefab("GeyserGeneric_resin", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) },

		};
		static List<Prefab>[] gprefabs2 = new List<Prefab>[]{
				new List<Prefab> {new Prefab("GeyserGeneric_molten_carbon", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) },
				new List<Prefab> {new Prefab("GeyserGeneric_molten_glass", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) },
				new List<Prefab> {new Prefab("GeyserGeneric_molten_niobium", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) },
				new List<Prefab> {new Prefab("GeyserGeneric_molten_steel", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) },
				new List<Prefab> {new Prefab("GeyserGeneric_molten_uranium", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) },
		};

		static string[] TemplateName = new string[]{
				"geyser_hot_water",
				"geyser_super_coolant",
				"geyser_LiquidPhosphorus",
				"geyser_Petroleum",
				"geyser_molten_sucrose",
				"geyser_resin",
				"geyser_molten_niobium",
				"geyser_molten_uranium",
				"geyser_molten_steel",
				"geyser_molten_glass",
				"geyser_molten_carbon",
				};

		public static TemplateContainer inittamplate(string name, int priority, TemplateContainer.Info info, List<Cell> cells, List<Prefab> buildings, List<Prefab> pickupables, List<Prefab> elementalOres, List<Prefab> otherEntities)
		{
			TemplateContainer template = new TemplateContainer();
			template.priority = priority;
			template.name = name;
			template.info = info;
			template.cells = cells;
			template.buildings = buildings;
			template.pickupables = pickupables;
			template.elementalOres = elementalOres;
			template.otherEntities = otherEntities;
			return template;
		}

		[HarmonyPatch(typeof(GeyserGenericConfig), "GenerateConfigs")]
		public class 增加新的泉1
		{
			private static void Postfix(ref List<GeyserGenericConfig.GeyserPrefabParams> __result)
			{
				if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.更多泉与火山)
				{
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_URANIUM" + ".NAME", "铀火山" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_URANIUM" + ".DESC", "一座大型火山,定期喷发出熔融铀" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_STEEL" + ".NAME", "钢火山" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_STEEL" + ".DESC", "一座大型火山,定期喷发出熔融钢" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_GLASS" + ".NAME", "玻璃火山" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_GLASS" + ".DESC", "一座大型火山,定期喷发出熔融玻璃" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "SUPER_COOLANT" + ".NAME", "超冷泉" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "SUPER_COOLANT" + ".DESC", "一座大型泉,定期喷发出超级冷却剂" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "PETROLEUM" + ".NAME", "石油泉" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "PETROLEUM" + ".DESC", "一座大型泉,定期喷发出石油" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "LIQUIDPHOSPHORUS" + ".NAME", "液磷泉" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "LIQUIDPHOSPHORUS" + ".DESC", "一座大型泉,定期喷发出液态磷" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_CARBON" + ".NAME", "碳火山" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_CARBON" + ".DESC", "一座大型火山,定期喷发出熔融碳" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_SUCROSE" + ".NAME", "蔗糖泉" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MOLTEN_SUCROSE" + ".DESC", "一座大型泉,定期喷发出熔融蔗糖" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "RESIN" + ".NAME", "树脂泉" });
					Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "RESIN" + ".DESC", "一座大型泉,定期喷发出液态树脂" });


					if (DlcManager.IsExpansion1Active()) __result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_niobium_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 2900f, 1000f, 2500f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					else __result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 2900f, 1000f, 2500f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_uranium", SimHashes.MoltenUranium, GeyserConfigurator.GeyserShape.Molten, 1000.15f, 800f, 1600f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_steel", SimHashes.MoltenSteel, GeyserConfigurator.GeyserShape.Molten, 2800f, 800f, 2000f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_glass", SimHashes.MoltenGlass, GeyserConfigurator.GeyserShape.Molten, 1800.15f, 800f, 1600f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("super_coolant", SimHashes.SuperCoolant, GeyserConfigurator.GeyserShape.Liquid, 0.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("Petroleum", SimHashes.Petroleum, GeyserConfigurator.GeyserShape.Liquid, 600f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("LiquidPhosphorus", SimHashes.LiquidPhosphorus, GeyserConfigurator.GeyserShape.Liquid, 450.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_carbon", SimHashes.MoltenCarbon, GeyserConfigurator.GeyserShape.Molten, 4800.15f, 800f, 1600f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_sucrose", SimHashes.MoltenSucrose, GeyserConfigurator.GeyserShape.Molten, 458.15f, 800f, 1600f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
					__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_sulfur_kanim", 3, 3, new GeyserConfigurator.GeyserType("Resin", SimHashes.Resin, GeyserConfigurator.GeyserShape.Molten, 373.15f, 1000f, 2000f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));

				}





			}



		}

		public static class AllowedCellsFilterReflectionHelper
		{
			public static void SetPrivateProperties(
				AllowedCellsFilter instance,
				TagCommand tagcommand,
				string tag,
				int minDistance,
				int maxDistance,
				Command command,
				List<Temperature.Range> temperatureRanges,
				List<SubWorld.ZoneType> zoneTypes,
				List<string> subworldNames)
			{
				SetProperty(instance, "tagcommand", tagcommand);
				SetProperty(instance, "tag", tag);
				SetProperty(instance, "minDistance", minDistance);
				SetProperty(instance, "maxDistance", maxDistance);
				SetProperty(instance, "command", command);
				SetProperty(instance, "temperatureRanges", temperatureRanges);
				SetProperty(instance, "zoneTypes", zoneTypes);
				SetProperty(instance, "subworldNames", subworldNames);
			}

			private static void SetProperty<T>(AllowedCellsFilter instance, string propertyName, T value)
			{
				PropertyInfo propertyInfo = typeof(AllowedCellsFilter).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				MethodInfo setterInfo = propertyInfo?.GetSetMethod(true);
				setterInfo?.Invoke(instance, new object[] { value });
			}
		}
		public static class TemplateSpawnRulesReflectionHelper
		{
			public static void SetPrivateProperties(
				TemplateSpawnRules instance,
				string ruleId,
				List<string> names,
				TemplateSpawnRules.ListRule listRule,
				int someCount,
				int moreCount,
				int times,
				float priority,
				bool allowDuplicates,
				bool allowExtremeTemperatureOverlap,
				bool useRelaxedFiltering,
				List<AllowedCellsFilter> allowedCellsFilter)
			{
				SetProperty(instance, "ruleId", ruleId);
				SetProperty(instance, "names", names);
				SetProperty(instance, "listRule", listRule);
				SetProperty(instance, "someCount", someCount);
				SetProperty(instance, "moreCount", moreCount);
				SetProperty(instance, "times", times);
				SetProperty(instance, "priority", priority);
				SetProperty(instance, "allowDuplicates", allowDuplicates);
				SetProperty(instance, "allowExtremeTemperatureOverlap", allowExtremeTemperatureOverlap);
				SetProperty(instance, "useRelaxedFiltering", useRelaxedFiltering);
				SetProperty(instance, "allowedCellsFilter", allowedCellsFilter);
			}

			private static void SetProperty<T>(TemplateSpawnRules instance, string propertyName, T value)
			{
				PropertyInfo propertyInfo = typeof(TemplateSpawnRules).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				MethodInfo setterInfo = propertyInfo?.GetSetMethod(true);
				setterInfo?.Invoke(instance, new object[] { value });
			}
		}

		[HarmonyPatch(typeof(WorldGen), "RenderOffline")]
		public class 增加新的泉2
		{
			static bool isinit = false;

            private static void Prefix(ref WorldGen __instance)
			{
				if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.更多泉与火山)
					if (__instance.isStartingWorld && !isinit)
				{
					isinit = !isinit;
					ProcGen.World.TemplateSpawnRules templateSpawnRules = new ProcGen.World.TemplateSpawnRules();
					List<AllowedCellsFilter> allowedCellsFilterList = new List<AllowedCellsFilter>() { new AllowedCellsFilter() , new AllowedCellsFilter() , new AllowedCellsFilter() };

					AllowedCellsFilterReflectionHelper.SetPrivateProperties(allowedCellsFilterList[0], TagCommand.DistanceFromTag,"AtStart",2,100,Command.Replace,new List<Temperature.Range>(), new List<SubWorld.ZoneType>(),new List<string>());
					//虚空入侵时 允许火山生成在太空暴露的生态中
					if ((PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.虚空入侵)) AllowedCellsFilterReflectionHelper.SetPrivateProperties(allowedCellsFilterList[1], TagCommand.Default, "", 0, 0, Command.ExceptWith, new List<Temperature.Range>(), new List<SubWorld.ZoneType> { SubWorld.ZoneType.RocketInterior }, new List<string>());
					//否则 火山不允许生成在太空暴露生态
					else AllowedCellsFilterReflectionHelper.SetPrivateProperties(allowedCellsFilterList[1], TagCommand.Default, "", 0, 0, Command.ExceptWith, new List<Temperature.Range>(), new List<SubWorld.ZoneType> { SubWorld.ZoneType.Space }, new List<string>());
					
					AllowedCellsFilterReflectionHelper.SetPrivateProperties(allowedCellsFilterList[2], TagCommand.AtTag, "NoGlobalFeatureSpawning", 0,0, Command.ExceptWith, new List<Temperature.Range>(), new List<SubWorld.ZoneType>(), new List<string>());

					TemplateSpawnRulesReflectionHelper.SetPrivateProperties(
						templateSpawnRules,
						null,
						TemplateName.ToList(),
						ListRule.GuaranteeAll,
						0,
						0,
						1,
						20,
						true,
						false,
						false,
						allowedCellsFilterList);
					__instance.Settings.world.worldTemplateRules.Add(templateSpawnRules);
				}

				return ;
				
				
			}



		}

		[HarmonyPatch(typeof(TemplateCache), "GetTemplate")]
		public class 增加新的泉3
		{


			static List<TemplateContainer> container=new List<TemplateContainer>();
			static bool isinit = false;
			private static bool Prefix( ref TemplateContainer __result, string templatePath){
				if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.更多泉与火山)
				{
					if (!isinit)//初始化 附加模板
					{
						isinit = !isinit;
						SeededRandom seeded = new SeededRandom((int)System.DateTime.Now.Ticks);

						TemplateContainer.Info info = new TemplateContainer.Info();

						info.tags = null;

						int i = 0;
						info.min = new Vector2f(-1, -1);
						info.size = new Vector2f(4, 4);
						info.area = 16;
						foreach (List<Prefab> prefab in gprefabs1)//将泉的模板加入到 附加模板
						{
							container.Add(inittamplate(TemplateName[i++], 100, info, 泉填充物, null, null, null, prefab));
						}
						info.min = new Vector2f(-1, -2);
						info.size = new Vector2f(4, 5);
						info.area = 20;
						foreach (List<Prefab> prefab in gprefabs2)//将火山的模板加入到 附加模板
						{
							container.Add(inittamplate(TemplateName[i++], 100, info, 火山填充物, null, null, null, prefab));
						}
					}

					foreach (TemplateContainer t in container)//从附加模板中查找是否存在指定模板名的模板
					{
						if (t.name == templatePath)
						{
							__result = t;
							Debug.Log("get template " + templatePath);
							return false;
						}
					}
				}
				return true;
			}
		}
	}
}
