using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统
{
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.隔热线材) })]

    class 隔热线材
    {
        public static void ExecuteOnInit()
        {
            GlobalBuildingConfig.CreateBuildingDef<BaseWireConfig>(null, (config, def) => {
                if(config is BaseWireConfig){
                    def.ThermalConductivity = 0f;
                }
            });
            GlobalBuildingConfig.CreateBuildingDef<BaseLogicWireConfig>(null, (config, def) => {
                if(config is BaseLogicWireConfig)
                {
                    def.ThermalConductivity = 0f;
                }
            });
        }

    }
}
