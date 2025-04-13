using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace 大一统{
    [AnyHarmonyPatch(typeof(WorldContainer), "GetCosmicRadiationValueFromFixedTrait", Postfix:nameof(GetCosmicRadiationValueFromFixedTrait) ,ControlName: new string[] { nameof(大一统.大一统控制台UI.辐星高照) })]
    public class 辐星高照
    {
        static void GetCosmicRadiationValueFromFixedTrait(WorldContainer __instance,ref int __result)
        {
            __result*= (int)大一统.大一统控制台UI.Instance.辐星高照;
        }
    }
}
