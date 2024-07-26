using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 无限拖把
{
	[HarmonyPatch(typeof(MopTool), "OnPrefabInit")]
	public class 无限拖把
	{
		public static void Postfix()
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.无限拖把)
			{
				MopTool.maxMopAmt = float.PositiveInfinity;     //拖把无限制
				
			}
		}
	}
}
