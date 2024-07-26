using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 无极变速
{
	[HarmonyPatch(typeof(SpeedControlScreen), "OnChanged")]
	public class 无极变速
	{
		private static void Postfix(ref SpeedControlScreen __instance)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.无级变速)
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
}
