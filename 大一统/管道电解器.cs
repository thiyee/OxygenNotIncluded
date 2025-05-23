﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit),ControlName:new string[] { nameof(大一统控制台UI.管道电解器) })]
    class 管道电解器
    {
        public static void ExecuteOnInit()
        {


            GlobalBuildingConfig.CreateBuildingDef<ElectrolyzerConfig>(null, (config, __result) =>
            {
                __result.OutputConduitType = ConduitType.Gas;
                __result.UtilityOutputOffset = new CellOffset(0, 1);
            });
            GlobalBuildingConfig.ConfigureBuildingTemplate<ElectrolyzerConfig>(null, (config, go, tag) =>
            {
                Storage storage = go.AddOrGet<Storage>();
                ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
                for (int i = 0; i < elementConverter.outputElements.Count(); i++)
                {
                    elementConverter.outputElements[i].storeOutput = true;
                }
                ConduitDispenser conduitDispenser2 = go.AddComponent<ConduitDispenser>();
                conduitDispenser2.storage = storage;
                conduitDispenser2.conduitType = ConduitType.Gas;
                conduitDispenser2.elementFilter = new SimHashes[] { SimHashes.Oxygen };
                conduitDispenser2.alwaysDispense = true;
                ConduitDispenser conduitDispenser = go.AddComponent<ConduitDispenser>();
                conduitDispenser.storage = storage;
                conduitDispenser.conduitType = ConduitType.Gas;
                conduitDispenser.elementFilter = new SimHashes[] { SimHashes.Hydrogen };
                conduitDispenser.alwaysDispense = true;
                conduitDispenser.useSecondaryOutput = true;
            });
            GlobalBuildingConfig.DoPostConfigureComplete<ElectrolyzerConfig>(null, (config, go) =>
            {
                go.AddComponent<ConduitSecondaryOutput>().portInfo = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 1));
            });

        }       
    }
}
