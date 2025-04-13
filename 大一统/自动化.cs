using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
    [AnyHarmonyPatch(typeof(ComplexRecipeManager), "Add", Replace: nameof(Add), ControlName: new string[] { nameof(大一统.大一统控制台UI.自动化) })]
    public class 自动化
    {
        static Dictionary<ComplexRecipe, bool> recipeInits = new Dictionary<ComplexRecipe, bool>();
        public void Add(ComplexRecipe recipe)
        {
            ComplexRecipeManager __instance = ComplexRecipeManager.Get();
            if (__instance.recipes.Exists(e => e.id == recipe.id))
            {
                Debug.LogError(string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", recipe.id));
            }
            __instance.recipes.Add(recipe);
            recipeInits.Add(recipe, false);
            if (recipe.FabricationVisualizer != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
            }
            var u = recipeInits.Where(r => !r.Value && r.Key.time != 0f).ToList();
            foreach (var v in u)
            {
                recipeInits[v.Key] = true;
                v.Key.time = v.Key.time / 10f > 1f ? v.Key.time / 10f : 1f;

            }

        }
    }

}
