﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace 全局建筑配置
{
    
    public class 全局建筑配置
    {
        public static void BuildingComplete(BuildingDef def)
        {
            switch (def.PrefabID)
            {
                case HighEnergyParticleRedirectorConfig.ID:
                    {
                        Console.WriteLine(def.PrefabID + "----------");
                        def.MaterialCategory = MATERIALS.ANY_BUILDABLE;
                    }
                    break;
                case PressureDoorConfig.ID:
                    {
                        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0])*10000;
                        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                    }
                    break;
                case ManualPressureDoorConfig.ID:
                    {
                        def.MassForTemperatureModification *= (BUILDINGS.CONSTRUCTION_MASS_KG.TIER7[0] / def.Mass[0]) * 10000;
                        def.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
                    }
                    break;
                default: break;
            }


            ComplexFabricator complex = null;
            def.BuildingComplete?.TryGetComponent(out complex);
            if (complex != null) complex.duplicantOperated = false;
        }


        [HarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding")]
        public class RegisterBuildingPatch
        {
            public static void Postfix(BuildingConfigManager __instance, IBuildingConfig config)
            {
                //if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.自动化) ;

                var field = typeof(BuildingConfigManager).GetField("configTable", BindingFlags.NonPublic | BindingFlags.Instance);
                var configTable = (Dictionary<IBuildingConfig, BuildingDef>)field.GetValue(__instance);
                if (configTable.ContainsKey(config))
                {
                    BuildingComplete(configTable[config]);

                }
            }
        }
        [HarmonyPatch(typeof(Assets), "AddBuildingDef")]
        public class AddBuildingDefPatch
        {
            public static void Postfix(BuildingDef def)
            {
                BuildingComplete(def);
            }
        }
        [HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
        public class CreateBuildingDefPatch
        {
            public static void Postfix(BuildingDef __result)
            {
                BuildingComplete(__result);
            }
        }
    }

}
