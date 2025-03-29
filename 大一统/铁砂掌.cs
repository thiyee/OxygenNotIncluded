using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 铁砂掌
{
    [AnyHarmonyPatch(typeof(BaseMinionConfig), "BaseMinion")]
    class 铁砂掌{
        private static void Postfix(ref GameObject __result)
        {
            if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.铁砂掌){
                Storage storage = __result.GetComponent<Storage>();
                storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>{
                    Storage.StoredItemModifier.Seal,
                    Storage.StoredItemModifier.Insulate
                });   
            }
        }


    }

}
