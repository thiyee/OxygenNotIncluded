using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 高频电炉
{
	[HarmonyPatch(typeof(GlassForgeConfig), "ConfigureBuildingTemplate")]
	public class 高频电炉
	{
		private static void Postfix(ref GameObject go)
		{
			var optionsInstance = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance;
			if (optionsInstance != null && optionsInstance.高频电炉)
			{
				var glassForge = go?.AddOrGet<GlassForge>();
				if (glassForge != null)
				{
					glassForge.duplicantOperated = false;
				}

				foreach (int hash in Enum.GetValues(typeof(SimHashes)))
				{
					Element element = ElementLoader.FindElementByHash((SimHashes)hash);
					if (element != null)
					{
						if ((SimHashes)hash != SimHashes.Sand && element.IsSolid && element.highTempTransitionTarget != 0)
						{
							Element elementhigh = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
							if (elementhigh != null && elementhigh.IsLiquid)
							{
								ComplexRecipe.RecipeElement[] Array1 = new ComplexRecipe.RecipeElement[]{
								new ComplexRecipe.RecipeElement(element.tag, 100f)};
								ComplexRecipe.RecipeElement[] Array2 = new ComplexRecipe.RecipeElement[]{
								new ComplexRecipe.RecipeElement(elementhigh.tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted, false)};

								string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("GlassForge", Array1[0].material);
								string text = ComplexRecipeManager.MakeRecipeID("GlassForge", Array1, Array2);
								ComplexRecipe complexRecipe = new ComplexRecipe(text, Array1, Array2);
								complexRecipe.time = 5f;
								complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
								complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, ElementLoader.GetElement(Array2[0].material).name, ElementLoader.GetElement(Array1[0].material).name);
								complexRecipe.fabricators = new List<Tag> { TagManager.Create("GlassForge") };
								ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
							}
						}
					}
					else
					{
						Console.Out.WriteLine("Get Element Failed\n");
					}
				}
			}
		}
	}
}

