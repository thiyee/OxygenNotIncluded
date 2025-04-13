using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(null, null, ExecuteOnInit : nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.千里眼) })]
    public class 千里眼
    {
        public static void ExecuteOnInit()
        {
			GlobalBuildingConfig.ConfigureBuildingTemplate<ClusterTelescopeConfig>(null, (config, go, tag) =>
			{
                ClusterTelescope.Def Teledef = go.AddOrGetDef<ClusterTelescope.Def>();
                Teledef.clearScanCellRadius = 15;
                Teledef.analyzeClusterRadius = 15;
            });

		}
	}
}
