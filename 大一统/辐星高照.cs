using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace 辐星高照
{
    [HarmonyPatch(typeof(WorldContainer))]
    [HarmonyPatch("GetCosmicRadiationValueFromFixedTrait")]
    public class WorldContainer_Patch
    {
        private static bool isInit = false;

        static void Postfix(WorldContainer __instance,ref int __result)
        {
            __result*= (int)PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.辐星高照;
            //__instance.CosmicRadiationFixedTraits[__instance.cosmicRadiationFixedTrait] = 100;
            //if (!isInit)
            //{
            //    isInit = true;
            //
            //    FieldInfo cosmicRadiationFixedTraitsField = typeof(WorldContainer).GetField("cosmicRadiationFixedTraits", BindingFlags.NonPublic | BindingFlags.Instance);
            //    Dictionary<string, int> cosmicRadiationFixedTraits = (Dictionary<string, int>)cosmicRadiationFixedTraitsField.GetValue(__instance);
            //
            //    List<string> keys = new List<string>(cosmicRadiationFixedTraits.Keys);
            //    foreach (string key in keys)
            //    {
            //
            //        Debug.Log("Pre宇宙辐射:"+key+":"+ cosmicRadiationFixedTraits[key].ToString());
            //        // 在此处执行操作，例如修改原始字典
            //        cosmicRadiationFixedTraits[key] *= (int)PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.辐星高照;
            //        Debug.Log("Post宇宙辐射:" + key + ":" + cosmicRadiationFixedTraits[key].ToString());
            //    }
            //}
        }
    }
}
