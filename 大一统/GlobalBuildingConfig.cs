using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using static ComplexRecipe;

namespace 大一统
{
    [AnyHarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding", Transpiler: nameof(RegisterBuilding))]

    public class GlobalBuildingConfig
    {
        static HashSet<ComplexRecipe> recipesinit = new HashSet<ComplexRecipe>();
        public static void BuildingComplete(BuildingDef def)
        {
            switch (def.PrefabID)
            {
                case HighEnergyParticleRedirectorConfig.ID:
                    {
                        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                    }
                    break;
                //case PressureDoorConfig.ID:
                //    {
                //        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                //        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0]) * 10000;
                //        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                //    }
                //    break;
                //case ManualPressureDoorConfig.ID:
                //    {
                //        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                //        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0]) * 10000;
                //        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                //    }
                //    break;
                default: break;
            }


            ComplexFabricator complex = null;
            def.BuildingComplete?.TryGetComponent(out complex);
            if (大一统.大一统控制台UI.Instance.自动化)
            {
                if (complex != null)
                    complex.duplicantOperated = false;
                List<ComplexRecipe> newComplexRecipe = ComplexRecipeManager.Get().recipes.Concat(ComplexRecipeManager.Get().preProcessRecipes).Where(r => !recipesinit.Contains(r)).ToList();
                
                foreach (var r in newComplexRecipe)
                {
                    recipesinit.Add(r);
                    r.time /= 10;
                }
            }
        }
        static Dictionary<MethodInfo, MethodInfo> ProxyHandler = new Dictionary<MethodInfo, MethodInfo>()
            {
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.CreateBuildingDef)),typeof(GlobalBuildingConfig).GetMethod(nameof(CreateBuildingDef),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.ConfigureBuildingTemplate)),typeof(GlobalBuildingConfig).GetMethod(nameof(ConfigureBuildingTemplate),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigureComplete)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigureComplete),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigurePreview)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigurePreview),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigureUnderConstruction)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigureUnderConstruction),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },

            };

        private static List<Func<IBuildingConfig, BuildingDef, bool>> CreateBuildingDefPrefix = new List<Func<IBuildingConfig, BuildingDef, bool>>();
        private static List<Func<IBuildingConfig, GameObject, Tag, bool>> ConfigureBuildingTemplatePrefix = new List<Func<IBuildingConfig, GameObject, Tag, bool>>();
        private static List<Func<IBuildingConfig, GameObject, bool>> DoPostConfigureCompletePrefix = new List<Func<IBuildingConfig, GameObject, bool>>();
        private static List<Func<IBuildingConfig, BuildingDef, GameObject, bool>> DoPostConfigurePreviewPrefix = new List<Func<IBuildingConfig, BuildingDef, GameObject, bool>>();
        private static List<Func<IBuildingConfig, GameObject, bool>> DoPostConfigureUnderConstructionPrefix = new List<Func<IBuildingConfig, GameObject, bool>>();

        private static List<Action<IBuildingConfig, BuildingDef>> CreateBuildingDefPostfix = new List<Action<IBuildingConfig, BuildingDef>>();
        private static List<Action<IBuildingConfig, GameObject, Tag>> ConfigureBuildingTemplatePostfix = new List<Action<IBuildingConfig, GameObject, Tag>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureCompletePostfix = new List<Action<IBuildingConfig, GameObject>>();
        private static List<Action<IBuildingConfig, BuildingDef, GameObject>> DoPostConfigurePreviewPostfix = new List<Action<IBuildingConfig, BuildingDef, GameObject>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureUnderConstructionPostfix = new List<Action<IBuildingConfig, GameObject>>();


        public static void CreateBuildingDef<T>(Func<T, BuildingDef, bool> prefix = null, Action<T, BuildingDef> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) CreateBuildingDefPrefix.Add((c, d) => { if (c is T t) return prefix(t, null); else return true; });
            if (postfix != null) CreateBuildingDefPostfix.Add((c, d) => { if (c is T t) postfix(t, d); });
        }
        public static void ConfigureBuildingTemplate<T>(Func<T, GameObject, Tag, bool> prefix = null, Action<T, GameObject, Tag> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) ConfigureBuildingTemplatePrefix.Add((c, g, t) => { if (c is T tc) return prefix(tc, g, t); else return true; });
            if (postfix != null) ConfigureBuildingTemplatePostfix.Add((c, g, t) => { if (c is T tc) postfix(tc, g, t); });
        }
        public static void DoPostConfigureComplete<T>(Func<T, GameObject, bool> prefix = null, Action<T, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigureCompletePrefix.Add((c, g) => { if (c is T t) return prefix(t, g); else return true; });
            if (postfix != null) DoPostConfigureCompletePostfix.Add((c, g) => { if (c is T t) postfix(t, g); });
        }
        public static void DoPostConfigurePreview<T>(Func<T, BuildingDef, GameObject, bool> prefix = null, Action<T, BuildingDef, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigurePreviewPrefix.Add((c, d, g) => { if (c is T t) return prefix(t, d, g); else return true; });
            if (postfix != null) DoPostConfigurePreviewPostfix.Add((c, d, g) => { if (c is T t) postfix(t, d, g); });
        }
        public static void DoPostConfigureUnderConstruction<T>(Func<T, GameObject, bool> prefix = null, Action<T, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigureUnderConstructionPrefix.Add((c, g) => { if (c is T t) return prefix(t, g); else return true; });
            if (postfix != null) DoPostConfigureUnderConstructionPostfix.Add((c, g) => { if (c is T t) postfix(t, g); });
        }


        public static IEnumerable<CodeInstruction> RegisterBuilding(IEnumerable<CodeInstruction> instructions)
        {

            foreach (var instruction in instructions)
            {
                // 检查是否需要替换此指令
                var originalMethod = ProxyHandler.Keys.FirstOrDefault(m => instruction.Calls(m));
                if (originalMethod != null && ProxyHandler.TryGetValue(originalMethod, out var replacementMethod))
                {
                    yield return new CodeInstruction(OpCodes.Call, replacementMethod);
                }
                else
                {
                    // 保留原始指令
                    yield return instruction;
                }
            }
        }

        private static BuildingDef CreateBuildingDef(IBuildingConfig config)
        {
            BuildingDef result = null;
            bool prefixResult = true;
            CreateBuildingDefPrefix.Do(n => prefixResult = prefixResult && n.Invoke(config, null));
            if (prefixResult) result = config.CreateBuildingDef();
            CreateBuildingDefPostfix.Do(n => n.Invoke(config, result));
            return result;
        }
        private static void ConfigureBuildingTemplate(IBuildingConfig config, GameObject go, Tag prefab_tag)
        {
            bool prefixResult = true;

            ConfigureBuildingTemplatePrefix.Do(n => prefixResult = prefixResult && n.Invoke(config, go, prefab_tag));
            if (prefixResult) config.ConfigureBuildingTemplate(go, prefab_tag);
            ConfigureBuildingTemplatePostfix.Do(n => n.Invoke(config, go, prefab_tag));

        }
        private static void DoPostConfigureComplete(IBuildingConfig config, GameObject go)
        {
            bool prefixResult = true;

            DoPostConfigureCompletePrefix.Do(n => prefixResult = prefixResult && n.Invoke(config, go));
            if (prefixResult) config.DoPostConfigureComplete(go);
            DoPostConfigureCompletePostfix.Do(n => n.Invoke(config, go));
        }
        private static void DoPostConfigurePreview(IBuildingConfig config, BuildingDef def, GameObject go)
        {
            bool prefixResult = true;

            DoPostConfigurePreviewPrefix.Do(n => prefixResult = prefixResult && n.Invoke(config, def, go));
            if (prefixResult) config.DoPostConfigurePreview(def, go);
            DoPostConfigurePreviewPostfix.Do(n => n.Invoke(config, def, go));
        }
        private static void DoPostConfigureUnderConstruction(IBuildingConfig config, GameObject go)
        {
            bool prefixResult = true;

            DoPostConfigureUnderConstructionPrefix.Do(n => prefixResult = prefixResult && n.Invoke(config, go));
            if (prefixResult) config.DoPostConfigureUnderConstruction(go);
            DoPostConfigureUnderConstructionPostfix.Do(n => n.Invoke(config, go));
        }

        [AnyHarmonyPatch(typeof(Assets), "AddBuildingDef")]
        public class AddBuildingDefPatch
        {
            public static void Postfix(BuildingDef def)
            {
                BuildingComplete(def);
            }
        }
        [AnyHarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
        public class CreateBuildingDefPatch
        {
            public static void Postfix(BuildingDef __result)
            {
                BuildingComplete(__result);
            }
        }
    }

}
