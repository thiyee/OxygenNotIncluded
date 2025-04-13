using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(MopTool), "OnPrefabInit", ControlName: new string[] { nameof(大一统.大一统控制台UI.无限拖把) })]
	public class 无限拖把
	{
		public static void Postfix()
		{
				MopTool.maxMopAmt = float.PositiveInfinity;     //拖把无限制
				
			
		}
	}
}
