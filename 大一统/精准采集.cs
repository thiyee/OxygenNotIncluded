using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(WorldDamage), "OnDigComplete", ControlName: new string[] { nameof(大一统.大一统控制台UI.精准采集) })]
	public class 挖掘不损失质量
	{
		private static void Prefix(ref float mass)
		{
				mass *= 2;
		}
	}
}
