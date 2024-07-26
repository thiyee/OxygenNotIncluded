using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 最后的基地
{
	[HarmonyPatch(typeof(CarePackageInfo), MethodType.Constructor, new Type[] { typeof(string), typeof(float), typeof(Func<bool>) })]
	public class 打印舱无需发现物品
	{
		private static void Prefix(ref string ID, ref float amount, ref Func<bool> requirement)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.最后的基地)
				requirement = null;
		}
	}
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	[HarmonyPatch(typeof(HeadquartersConfig), "ConfigureBuildingTemplate")]
	public class 打印舱添加新的物品{
		private static void Postfix(ref GameObject go){
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.最后的基地){
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

				Tag[] tags = new Tag[]{
					"BasicFabricMaterialPlantSeed".ToTag(),		  //"顶针芦苇种子",
					"BasicSingleHarvestPlantSeed".ToTag(),		  //"米虱木种子",
					"BeanPlantSeed".ToTag(),					  //"小吃豆",
					"BulbPlantSeed".ToTag(),					  //"同伴芽种子",
					"CactusPlantSeed".ToTag(),					  //"雀跃掌种子",
					"ColdBreatherSeed".ToTag(),					  //"冰息萝卜种子",
					"ColdWheatSeed".ToTag(),					  //"冰霜麦粒",
					"CritterTrapPlantSeed".ToTag(),				  //"土星动物捕捉草种子",
					"CylindricaSeed".ToTag(),					  //"极乐刺种子",
					"EvilFlowerSeed".ToTag(),					  //"孢子兰种子",
					"FilterPlantSeed".ToTag(),					  //"仙水掌种子",
					"ForestTreeSeed".ToTag(),					  //"乔木橡实",
					"GasGrassSeed".ToTag(),						  //"释气草种子",
					"LeafyPlantSeed".ToTag(),					  //"欢乐叶种子",
					"MushroomSeed".ToTag(),						  //"真菌孢子",
					"OxyfernSeed".ToTag(),						  //"氧齿蕨种子",
					"PrickleFlowerSeed".ToTag(),				  //"毛刺花种子",
					"PrickleGrassSeed".ToTag(),					  //"诱人荆棘种子",
					"SaltPlantSeed".ToTag(),					  //"沙盐藤种子",
					"SeaLettuceSeed".ToTag(),					  //"水草种子",
					"SpiceVineSeed".ToTag(),					  //"火椒种子",
					"SwampHarvestPlantSeed".ToTag(),			  //"沼浆笼种子",
					"SwampLilySeed".ToTag(),					  //"芳香百合种子",
					"ToePlantSeed".ToTag(),						  //"安止宁种子",
					"WineCupsSeed".ToTag(),						  //"醇锦菇种子",
					"WormPlantSeed".ToTag(),                      //"虫果种子"
					"Hatch".ToTag(),							  //哈奇
					"LightBug".ToTag(),							  //发光虫
					"OilFloater".ToTag(),						  //浮游生物
					"Drecko".ToTag(),							  //
					"Glom".ToTag(),								  //
					"Puft".ToTag(),								  //
					"Pacu".ToTag(),								  //帕库鱼
					"Moo".ToTag(),								  //
					"Mole".ToTag(),								  //
					"Squirrel".ToTag(),							  //
					"Crab".ToTag(),								  //
					"Staterpillar".ToTag(),						  //
					"BeeBaby".ToTag(),							  //
					"DivergentBeetle".ToTag(),					  //
					"OrbitalResearchDataBank".ToTag()
				};
				String[] strings = new String[] {
				"顶针芦苇种子",
				"米虱木种子",
				"小吃豆",
				"同伴芽种子",
				"雀跃掌种子",
				"冰息萝卜种子",
				"冰霜麦粒",
				"土星动物捕捉草种子",
				"极乐刺种子",
				"孢子兰种子",
				"仙水掌种子",
				"乔木橡实",
				"释气草种子",
				"欢乐叶种子",
				"真菌孢子",
				"氧齿蕨种子",
				"毛刺花种子",
				"诱人荆棘种子",
				"沙盐藤种子",
				"水草种子",
				"火椒种子",
				"沼浆笼种子",
				"芳香百合种子",
				"安止宁种子",
				"醇锦菇种子",
				"虫果种子"
				};
				int i = 0;
				foreach (Tag tag in tags){
					Element element = ElementLoader.FindElementByHash((SimHashes)SimHashes.Niobium);
					ComplexRecipe.RecipeElement[] array1 = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(element.tag, 1000f) };
					ComplexRecipe.RecipeElement[] array2; 
					if (tag != "OrbitalResearchDataBank".ToTag()) array2 = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(tag, 1f,false) };
					else array2 = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(tag, 100f, false) };
					string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("Headquarters", array1[0].material);
					string text = ComplexRecipeManager.MakeRecipeID("Headquarters", array1, array2);
					ComplexRecipe complexRecipe = new ComplexRecipe(text, array1, array2);
					complexRecipe.time = 1f;
					if (i < strings.Count()) complexRecipe.description = string.Format("兑换 {0}", strings[i]);
					else complexRecipe.description = string.Format("兑换 {0}", tag.Name);
					complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
					complexRecipe.fabricators = new List<Tag> { TagManager.Create("Headquarters") };
					ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
					i++;
				}

			}
		}
	}

	[HarmonyPatch(typeof(HeadquartersConfig), "CreateBuildingDef")]
	public class 最后的基地1{
		private static void Postfix(ref BuildingDef __result){
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.最后的基地) { 
				__result.GeneratorWattageRating = 1000f;
				__result.GeneratorBaseCapacity = 20000f;
				__result.RequiresPowerOutput = true;
				__result.PowerOutputOffset = new CellOffset(0, 0);
				__result.ViewMode = OverlayModes.Power.ID;
			}
		} 
	}
	[HarmonyPatch(typeof(HeadquartersConfig), "ConfigureBuildingTemplate")]
	public class 最后的基地2
	{
		private static void Postfix(ref GameObject go, ref Tag prefab_tag){
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.最后的基地){
				CellOffset cellOffset = new CellOffset(0, 1);
				ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
				elementEmitter.outputElement = new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true);
				elementEmitter.emissionFrequency = 1f;
				elementEmitter.maxPressure = 2.5f;
				DevGenerator devGenerator = go.AddOrGet<DevGenerator>();
				devGenerator.powerDistributionOrder = 9;
				devGenerator.wattageRating = 1000f;
			}
		}
	}
}
