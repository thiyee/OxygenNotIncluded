using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(Storage), ".ctor", ControlName: new string[] { nameof(大一统.大一统控制台UI.储物箱容量) })]
	public class 储物箱容量
	{
		private static void Postfix(ref float ___capacityKg)
		{
			___capacityKg = 大一统.大一统控制台UI.Instance.储物箱容量 * 1000f;
		}
	}

}
