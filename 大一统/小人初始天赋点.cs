using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;

namespace 大一统{
	[AnyHarmonyPatch(typeof(MinionStartingStats), "GenerateAttributes", ControlName: new string[] { nameof(大一统.大一统控制台UI.小人初始天赋点) })]
	class 小人初始天赋点
	{
		public static void Postfix(ref MinionStartingStats __instance)
		{
			List<string> list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
			for (int i = 0; i < list.Count; i++)
			{
				if (__instance.StartingLevels.ContainsKey(list[i]))
				{
					__instance.StartingLevels[list[i]] += 大一统.大一统控制台UI.Instance.小人初始天赋点;
				}
			}

		}


	}
}
