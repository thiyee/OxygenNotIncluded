using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{

	[AnyHarmonyPatch(typeof(HeadquartersConfig), "CreateBuildingDef", Postfix: nameof(CreateBuildingDef),ControlName:new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]
	[AnyHarmonyPatch(typeof(HeadquartersConfig), "DoPostConfigureComplete", Postfix: nameof(DoPostConfigureComplete), ControlName: new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]
	[AnyHarmonyPatch(typeof(CarePackageInfo), ".ctor", Prefix: nameof(CarePackageInfo), ControlName: new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]

	public class 最后的基地{
		private static void CreateBuildingDef(ref BuildingDef __result){
				__result.GeneratorWattageRating = 1000f;
				__result.GeneratorBaseCapacity = 20000f;
				__result.RequiresPowerOutput = true;
				__result.PowerOutputOffset = new CellOffset(0, 0);
				__result.ViewMode = OverlayModes.Power.ID;
		}
		private static void DoPostConfigureComplete(ref GameObject go)
		{

				CellOffset cellOffset = new CellOffset(0, 1);
				ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
				elementEmitter.outputElement = new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true);
				elementEmitter.emissionFrequency = 1f;
				elementEmitter.maxPressure = 2.5f;
				DevGenerator devGenerator = go.AddOrGet<DevGenerator>();
				devGenerator.powerDistributionOrder = 9;
				devGenerator.wattageRating = 1000f;


				go.AddOrGet<DropAllWorkable>();
				go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
				ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
				complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
				complexFabricator.duplicantOperated = true;
				go.AddOrGet<FabricatorIngredientStatusManager>();
				go.AddOrGet<CopyBuildingSettings>();
				ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
				BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
				complexFabricatorWorkable.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_rockrefinery_kanim") };
				complexFabricatorWorkable.workingPstComplete = new HashedString[] { "working_pst_complete" };

				var items = typeof(IEntityConfig).Assembly.GetTypes()
					.Where(t => typeof(IEntityConfig).IsAssignableFrom(t) && !t.IsAbstract)
					.Select(t => t.GetField("SEED_ID", BindingFlags.Public | BindingFlags.Static) ??
								t.GetField("EGG_ID", BindingFlags.Public | BindingFlags.Static))
					.Where(field => field != null)
					.Select(field => {
						string id = field.GetValue(null) as string;
						string name = string.Empty;

						if (id.EndsWith("Egg"))
						{
							name ="动物蛋";
						}
						else if (id.EndsWith("Seed"))
						{
							string baseId = id.Substring(0, id.Length - 4);
							name = Strings.Get($"STRINGS.CREATURES.SPECIES.SEEDS.{baseId.ToUpper()}.NAME");
							if (name.StartsWith("STRINGS.")) // 如果获取失败
							{
								name = Strings.Get($"STRINGS.CREATURES.SPECIES.SEEDS.{string.Concat(baseId.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToUpper()}.NAME");
							}
						}

						return (Tag: new Tag(id), Name: name, IngredientAmount: 1000f, ResultAmount: 1f);
					}).ToList();

				items.Add((Tag: new Tag("OrbitalResearchDataBank"), Name: "数据磁盘", IngredientAmount: 100f, ResultAmount: 100f));
				items.Add((Tag: new Tag("CritterTrapPlantSeed"), Name: "土星动物捕草种子", IngredientAmount: 1000f, ResultAmount: 1f));

				Tag headquarters = TagManager.Create("Headquarters");
				Element niobium = ElementLoader.FindElementByHash(SimHashes.Niobium);
				foreach (var item in items)
				{
					var input = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(niobium.tag, item.IngredientAmount, false) };
					var output = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(item.Tag, item.ResultAmount, false) };
					string obsoleteId = ComplexRecipeManager.MakeObsoleteRecipeID("Headquarters", input[0].material);
					string recipeId = ComplexRecipeManager.MakeRecipeID("Headquarters", input, output);
					var recipe = new ComplexRecipe(recipeId, input, output)
					{
						time = 1f,
						description = string.IsNullOrEmpty(item.Name) ?"": $"兑换 {item.Name}",
						nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
						fabricators = new List<Tag> { headquarters }
					};

					ComplexRecipeManager.Get().AddObsoleteIDMapping(obsoleteId, recipeId);
				}
				return;

			
		}
		private static void CarePackageInfo(ref string ID, ref float amount, ref Func<bool> requirement)
		{
				requirement = null;
		}
	}
}
