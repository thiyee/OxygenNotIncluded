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

namespace 大一统{
    [AnyHarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding", Transpiler: nameof(RegisterBuilding))]

    public class GlobalBuildingConfig
    {
        public static void BuildingComplete(BuildingDef def)
        {
            switch (def.PrefabID)
            {
                case HighEnergyParticleRedirectorConfig.ID:
                    {
                        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                    }
                    break;
                case PressureDoorConfig.ID:
                    {
                        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0])*10000;
                        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                    }
                    break;
                case ManualPressureDoorConfig.ID:
                    {
                        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0]) * 10000;
                        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                    }
                    break;
                default: break;
            }


            ComplexFabricator complex = null;
            def.BuildingComplete?.TryGetComponent(out complex);
            if (大一统.大一统控制台UI.Instance.自动化&& complex != null) complex.duplicantOperated = false;
        }
        static Dictionary<MethodInfo, MethodInfo> ProxyHandler = new Dictionary<MethodInfo, MethodInfo>()
            {
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.CreateBuildingDef)),typeof(GlobalBuildingConfig).GetMethod(nameof(CreateBuildingDef),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.ConfigureBuildingTemplate)),typeof(GlobalBuildingConfig).GetMethod(nameof(ConfigureBuildingTemplate),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigureComplete)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigureComplete),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigurePreview)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigurePreview),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },
                { typeof(IBuildingConfig).GetMethod(nameof(IBuildingConfig.DoPostConfigureUnderConstruction)),typeof(GlobalBuildingConfig).GetMethod(nameof(DoPostConfigureUnderConstruction),BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance) },

            };

        private static List<Action<IBuildingConfig, BuildingDef>> CreateBuildingDefPrefix = new List<Action<IBuildingConfig, BuildingDef>>();
        private static List<Action<IBuildingConfig, GameObject, Tag>> ConfigureBuildingTemplatePrefix = new List<Action<IBuildingConfig, GameObject, Tag>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureCompletePrefix = new List<Action<IBuildingConfig, GameObject>>();
        private static List<Action<IBuildingConfig, BuildingDef, GameObject>> DoPostConfigurePreviewPrefix = new List<Action<IBuildingConfig, BuildingDef, GameObject>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureUnderConstructionPrefix = new List<Action<IBuildingConfig, GameObject>>();
        
        private static List<Action<IBuildingConfig, BuildingDef>> CreateBuildingDefPostfix = new List<Action<IBuildingConfig, BuildingDef>>();
        private static List<Action<IBuildingConfig, GameObject, Tag>> ConfigureBuildingTemplatePostfix = new List<Action<IBuildingConfig, GameObject, Tag>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureCompletePostfix = new List<Action<IBuildingConfig, GameObject>>();
        private static List<Action<IBuildingConfig, BuildingDef, GameObject>> DoPostConfigurePreviewPostfix = new List<Action<IBuildingConfig, BuildingDef, GameObject>>();
        private static List<Action<IBuildingConfig, GameObject>> DoPostConfigureUnderConstructionPostfix = new List<Action<IBuildingConfig, GameObject>>();


        public static void CreateBuildingDef<T>(Action<T, BuildingDef> prefix = null,Action<T, BuildingDef> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) CreateBuildingDefPrefix.Add((c, d) => { if (c is T t) prefix(t, null); });
            if (postfix != null) CreateBuildingDefPostfix.Add((c, d) => { if (c is T t) postfix(t, d); });
        }
        public static void ConfigureBuildingTemplate<T>(Action<T, GameObject, Tag> prefix = null,Action<T, GameObject, Tag> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) ConfigureBuildingTemplatePrefix.Add((c, g, t) => { if (c is T tc) prefix(tc, g, t); });
            if (postfix != null) ConfigureBuildingTemplatePostfix.Add((c, g, t) => { if (c is T tc) postfix(tc, g, t); });
        }
        public static void DoPostConfigureComplete<T>(Action<T, GameObject> prefix = null,Action<T, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigureCompletePrefix.Add((c, g) => { if (c is T t) prefix(t, g); });
            if (postfix != null) DoPostConfigureCompletePostfix.Add((c, g) => { if (c is T t) postfix(t, g); });
        }
        public static void DoPostConfigurePreview<T>(Action<T, BuildingDef, GameObject> prefix = null,Action<T, BuildingDef, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigurePreviewPrefix.Add((c, d, g) => { if (c is T t) prefix(t, d, g); });
            if (postfix != null) DoPostConfigurePreviewPostfix.Add((c, d, g) => { if (c is T t) postfix(t, d, g); });
        }
        public static void DoPostConfigureUnderConstruction<T>(Action<T, GameObject> prefix = null,Action<T, GameObject> postfix = null) where T : IBuildingConfig
        {
            if (prefix != null) DoPostConfigureUnderConstructionPrefix.Add((c, g) => { if (c is T t) prefix(t, g); });
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
            BuildingDef result;
            CreateBuildingDefPrefix.Do(n => n.Invoke(config,null));
            result = config.CreateBuildingDef();
            CreateBuildingDefPostfix.Do(n => n.Invoke(config, result));
            return result;
        }
        private static void ConfigureBuildingTemplate(IBuildingConfig config, GameObject go, Tag prefab_tag)
        {
            ConfigureBuildingTemplatePrefix.Do(n => n.Invoke(config, go, prefab_tag));
            config.ConfigureBuildingTemplate(go, prefab_tag);
            ConfigureBuildingTemplatePostfix.Do(n => n.Invoke(config, go, prefab_tag));

        }
        private static void DoPostConfigureComplete(IBuildingConfig config, GameObject go)
        {
            DoPostConfigureCompletePrefix.Do(n => n.Invoke(config, go));
            config.DoPostConfigureComplete(go);
            DoPostConfigureCompletePostfix.Do(n => n.Invoke(config, go));
        }
        private static void DoPostConfigurePreview(IBuildingConfig config, BuildingDef def, GameObject go)
        {
            DoPostConfigurePreviewPrefix.Do(n => n.Invoke(config, def, go));
            config.DoPostConfigurePreview(def, go);
            DoPostConfigurePreviewPostfix.Do(n => n.Invoke(config, def, go));
        }
        private static void DoPostConfigureUnderConstruction(IBuildingConfig config, GameObject go)
        {
            DoPostConfigureUnderConstructionPrefix.Do(n => n.Invoke(config, go));
            config.DoPostConfigureUnderConstruction(go);
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
