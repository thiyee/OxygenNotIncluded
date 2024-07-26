using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 储物箱容量
{
	[HarmonyPatch(typeof(Storage), MethodType.Constructor)]
	public class 储物箱容量
	{
		private static void Postfix(ref float ___capacityKg)
		{
			___capacityKg = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.储物箱容量 * 1000f;
		}
	}

}
