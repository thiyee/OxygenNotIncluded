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
using System.Diagnostics;

namespace 大一统{


	[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.更多泉与火山) })]
	[AnyHarmonyPatch(typeof(GeyserGenericConfig), "GenerateConfigs", Postfix: nameof(GenerateConfigs), ControlName: new string[] { nameof(大一统.大一统控制台UI.更多泉与火山) })]
	[AnyHarmonyPatch(typeof(WorldGen), "RenderOffline", Prefix: nameof(RenderOffline), ControlName: new string[] { nameof(大一统.大一统控制台UI.更多泉与火山) })]
	[AnyHarmonyPatch(typeof(TemplateCache), "GetTemplate", Prefix: nameof(GetTemplate), ControlName: new string[] { nameof(大一统.大一统控制台UI.更多泉与火山) })]

	public class 更多泉与火山
	{
		static List<Cell> 火山填充物 = new List<Cell> {
			new Cell(-1,-2,SimHashes.Unobtanium,9000f,20000,null,0,false),
			new Cell(0 ,-2,SimHashes.Unobtanium,9000f,20000,null,0,false),
			new Cell(1 ,-2,SimHashes.Unobtanium,9000f,20000,null,0,false),
			new Cell(2 ,-2,SimHashes.Unobtanium,9000f,20000,null,0,false),
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
					new Cell(-1,-1,SimHashes.Unobtanium,9000f,20000,null,0,false),
					new Cell(0 ,-1,SimHashes.Unobtanium,9000f,20000,null,0,false),
					new Cell(1 ,-1,SimHashes.Unobtanium,9000f,20000,null,0,false),
					new Cell(2 ,-1,SimHashes.Unobtanium,9000f,20000,null,0,false),
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
		static TemplateContainer.Info 火山容器Info = new TemplateContainer.Info() { tags = null , min = new Vector2f(-1, -1) , size = new Vector2f(4, 4) , area = 16 };
		static TemplateContainer.Info 泉容器Info = new TemplateContainer.Info() { tags = null , min = new Vector2f(-1, -2), size = new Vector2f(4, 5), area = 20 };
		static (string Name, TemplateContainer.Info ContainerInfo, List<Cell> Padding, List<Prefab> Prefab)[] TemplateInfo = new []{
			(Name: "geyser_hot_water"		,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_hot_water", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_super_coolant"	,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_super_coolant", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_LiquidPhosphorus",ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_LiquidPhosphorus", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_Petroleum"		,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_Petroleum", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_sucrose"	,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_sucrose", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_resin"			,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_resin", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_milk"			,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_milk", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_sugarwater"		,ContainerInfo:泉容器Info	,Padding: 泉填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_sugarwater", TemplateClasses.Prefab.Type.Other, default, default, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_niobium"	,ContainerInfo:火山容器Info	,Padding: 火山填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_carbon", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_uranium"	,ContainerInfo:火山容器Info	,Padding: 火山填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_glass", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_steel"	,ContainerInfo:火山容器Info	,Padding: 火山填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_niobium", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_glass"	,ContainerInfo:火山容器Info	,Padding: 火山填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_steel", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
			(Name: "geyser_molten_carbon"	,ContainerInfo:火山容器Info	,Padding: 火山填充物,Prefab: new List<Prefab> { new Prefab("GeyserGeneric_molten_uranium", TemplateClasses.Prefab.Type.Other, default, -1, SimHashes.Katairite, default, 1, default, default, default, default, default) }),
		};
		static bool isinit = false;
		static bool templateisinit = false;
		static List<TemplateContainer> container = new List<TemplateContainer>();

		public static void ExecuteOnInit(){
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
			Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MILK" + ".NAME", "咸乳泉" });
			Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "MILK" + ".DESC", "一座大型泉,定期喷发出咸乳" });
			Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "SUGARWATER" + ".NAME", "花蜜泉" });
			Strings.Add(new string[] { "STRINGS.CREATURES.SPECIES.GEYSER." + "SUGARWATER" + ".DESC", "一座大型泉,定期喷发出花蜜" });
		}
		public static void GenerateConfigs(ref List<GeyserGenericConfig.GeyserPrefabParams> __result)
		{

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
				__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 3, 3, new GeyserConfigurator.GeyserType("Milk", SimHashes.Milk, GeyserConfigurator.GeyserShape.Molten, 310f, 1000f, 2000f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
				__result.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 3, 3, new GeyserConfigurator.GeyserType("SugarWater", SimHashes.SugarWater, GeyserConfigurator.GeyserShape.Molten, 233.15f, 1000f, 2000f, 500f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
			
		}
		public static void RenderOffline(ref WorldGen __instance)
		{
				if (__instance.isStartingWorld && !isinit)
				{
					isinit = !isinit;
					ProcGen.World.TemplateSpawnRules templateSpawnRules = new ProcGen.World.TemplateSpawnRules();
					List<AllowedCellsFilter> allowedCellsFilterList = new List<AllowedCellsFilter>() { new AllowedCellsFilter(), new AllowedCellsFilter(), new AllowedCellsFilter() };

					AllowedCellsFilterReflectionHelper.SetPrivateProperties(allowedCellsFilterList[0],
						TagCommand.DistanceFromTag,
						"AtStart",
						2,
						200,
						Command.Replace,
						new List<Temperature.Range>(),
						new List<SubWorld.ZoneType>(),
						new List<string>());
					//虚空入侵时 允许火山生成在太空暴露的生态中
					if ((大一统.大一统控制台UI.Instance.虚空入侵))
						AllowedCellsFilterReflectionHelper.SetPrivateProperties(
							allowedCellsFilterList[1],
							TagCommand.Default,
							"",
							0,
							0,
							Command.ExceptWith,
							new List<Temperature.Range>(),
							new List<SubWorld.ZoneType> { SubWorld.ZoneType.RocketInterior },
							new List<string>());
					//否则 火山不允许生成在太空暴露生态
					else
						AllowedCellsFilterReflectionHelper.SetPrivateProperties(
							allowedCellsFilterList[1],
							TagCommand.Default,
							"",
							0,
							0,
							Command.ExceptWith,
							new List<Temperature.Range>(),
							new List<SubWorld.ZoneType> { SubWorld.ZoneType.Space },
							new List<string>());

					AllowedCellsFilterReflectionHelper.SetPrivateProperties(
						allowedCellsFilterList[2],
						TagCommand.AtTag,
						"NoGlobalFeatureSpawning",
						0,
						0,
						Command.ExceptWith,
						new List<Temperature.Range>(),
						new List<SubWorld.ZoneType>(),
						new List<string>());

					TemplateSpawnRulesReflectionHelper.SetPrivateProperties(
						templateSpawnRules,
						null,
						TemplateInfo.Select(n=>n.Name).ToList(),
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

			return;


		}
		public static bool GetTemplate(ref TemplateContainer __result, string templatePath)
		{

				if (!templateisinit)//初始化 附加模板
				{
					templateisinit = !templateisinit;
					container.AddRange(TemplateInfo.Select(e => inittamplate(e.Name, 100, e.ContainerInfo, e.Padding, null, null, null, e.Prefab)));

				}
				__result = container.Where(e => e.name == templatePath).FirstOrDefault();
				//Console.WriteLine($"GetTemplate:{templatePath}={__result}");
				return __result == null;
			
			return true;
		}


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


	}
}
