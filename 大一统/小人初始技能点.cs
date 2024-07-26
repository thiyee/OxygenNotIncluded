using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 小人初始技能点
{
    [HarmonyPatch(typeof(MinionStartingStats), "ApplyAptitudes")]
    class 小人初始技能点
    {
		public static void Postfix(ref MinionStartingStats __instance,ref GameObject go){
			MinionResume component = go.GetComponent<MinionResume>();
			for (int i = 1; i <= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.小人初始技能点; i++){
				component.ForceAddSkillPoint();
			}
		}


	}
}
