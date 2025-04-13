using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(typeof(BaseMinionConfig), "BaseMinion", ControlName: new string[] { nameof(大一统.大一统控制台UI.铁砂掌) })]
    class 铁砂掌{
        private static void Postfix(ref GameObject __result)
        {
                Storage storage = __result.GetComponent<Storage>();
                storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>{
                    Storage.StoredItemModifier.Seal,
                    Storage.StoredItemModifier.Insulate
                });   
            
        }


    }

}
