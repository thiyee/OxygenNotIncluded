using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 动物更耐高低温
{
	[AnyHarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicCreature")]

	public class 动物体质增强
	{
		public static void Prefix(ref float warningLowTemperature, ref float warningHighTemperature, ref float lethalLowTemperature, ref float lethalHighTemperature)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物更耐高低温)
			{
				if (warningLowTemperature > 150f) warningLowTemperature -= 100f;
				if (warningHighTemperature < 303.15f) warningLowTemperature = 303.15f;
				if (lethalLowTemperature > 150f) lethalLowTemperature -= 100f;
				if (lethalHighTemperature < 275.15f + 55f) lethalHighTemperature = 275.15f + 55f;
			}
		}
	}
}
