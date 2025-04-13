using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
	[AnyHarmonyPatch(typeof(SpeedControlScreen), "OnChanged", ControlName: new string[] { nameof(大一统.大一统控制台UI.无级变速) })]
	public class 无极变速
	{
		private static void Postfix(ref SpeedControlScreen __instance)
		{
				if (__instance.IsPaused)
				{
					Time.timeScale = 0f;
					return;
				}
				if (__instance.GetSpeed() == 0)
				{
					Time.timeScale = __instance.normalSpeed;
					return;
				}
				if (__instance.GetSpeed() == 1)
				{
					Time.timeScale = __instance.fastSpeed * 2;
					return;
				}
				if (__instance.GetSpeed() == 2)
				{
					Time.timeScale = __instance.ultraSpeed * 4;
				}
			
		}
	}
}
