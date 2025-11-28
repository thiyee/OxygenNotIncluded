using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统
{
    [AnyHarmonyPatch(typeof(ResourceHarvestModule.StatesInstance), "ConsumeDiamond", Prefix: nameof(ConsumeDiamond), ControlName: new string[] { nameof(大一统.大一统控制台UI.火箭采集速度) })]
    [AnyHarmonyPatch(typeof(ResourceHarvestModule.StatesInstance), "GetMaxExtractKGFromDiamondAvailable", Postfix: nameof(GetMaxExtractKGFromDiamondAvailable), ControlName: new string[] { nameof(大一统.大一统控制台UI.火箭采集速度) })]
    [AnyHarmonyPatch(null,null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.火箭采集速度) })]

    public class 火箭采集速度
    {
        public static void GetMaxExtractKGFromDiamondAvailable(ref float __result)
        {
            __result *= 大一统.大一统控制台UI.Instance.火箭采集速度;
        }           
        public static void ConsumeDiamond(ref float amount)
        {
            amount /= 大一统.大一统控制台UI.Instance.火箭采集速度;
        }        
        public static void ExecuteOnInit()
        {
            GlobalBuildingConfig.ConfigureBuildingTemplate<NoseconeHarvestConfig>(null, (config, go, tag) => {
                go.AddOrGetDef<ResourceHarvestModule.Def>().harvestSpeed *= 大一统.大一统控制台UI.Instance.火箭采集速度;
            });
        }
    }
}
