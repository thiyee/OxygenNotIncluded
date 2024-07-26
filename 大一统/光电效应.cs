using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace 光电效应
{
    [HarmonyPatch(typeof(SolarPanel), "EnergySim200ms")]
    public static class 光电效应
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list=new List<CodeInstruction> (instructions);
            for(int i=0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_R4){
                    if ((float)list[i].operand == 0.00053f){
                        list[i].operand = 0.00053f * PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.光电效应;
                    }
                    if ((float)list[i].operand == 380f){
                        list[i].operand = float.PositiveInfinity;
                        return list.AsEnumerable<CodeInstruction>();
                    }
                }
            }
            return list.AsEnumerable<CodeInstruction>();
        }
    }

}
