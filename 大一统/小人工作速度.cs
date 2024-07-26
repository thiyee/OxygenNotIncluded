using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 小人工作速度
{
	[HarmonyPatch(typeof(Database.AttributeConverters), "Create")]
	public class 小人工作速度
	{
		private static void Prefix(ref float multiplier, ref float base_value)
		{
			base_value = 3f;
			multiplier *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.小人工作速度;
		}
	}
}
