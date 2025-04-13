using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.货舱扩容) })]

    class 货舱扩容
    {
        public static void ExecuteOnInit()
        {
            Action<IBuildingConfig, GameObject> Action = (config, go) =>
             {
                 Storage storage = go.AddOrGet<Storage>();
                 storage.capacityKg *= (大一统.大一统控制台UI.Instance.货舱扩容 / 100f);
             };
            GlobalBuildingConfig.DoPostConfigureComplete<GasCargoBayClusterConfig>(null, Action);
            GlobalBuildingConfig.DoPostConfigureComplete<GasCargoBaySmallConfig>(null, Action);
            GlobalBuildingConfig.DoPostConfigureComplete<LiquidCargoBayClusterConfig>(null, Action);
            GlobalBuildingConfig.DoPostConfigureComplete<LiquidCargoBaySmallConfig>(null, Action);
            GlobalBuildingConfig.DoPostConfigureComplete<SolidCargoBayClusterConfig>(null, Action);
            GlobalBuildingConfig.DoPostConfigureComplete<SolidCargoBaySmallConfig>(null, Action);

        }

    }
}
