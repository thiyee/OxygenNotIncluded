using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicCreature", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物更耐高低温) })]

	public class 动物体质增强
	{
		public static void Prefix(ref float warningLowTemperature, ref float warningHighTemperature, ref float lethalLowTemperature, ref float lethalHighTemperature)
		{
			float f = 大一统.大一统控制台UI.Instance.动物更耐高低温;
			if (f>0)
			{

				var t1 = (warningLowTemperature + warningHighTemperature) / 2;
				var t2 = (lethalLowTemperature + lethalHighTemperature) / 2;


				var maxf = warningLowTemperature / (t1 - warningLowTemperature);
				maxf = maxf < f ? maxf : f;
				warningLowTemperature -=  ((t1 - warningLowTemperature) * maxf);
				warningHighTemperature +=  ((warningHighTemperature - t1) * maxf);


				maxf = lethalLowTemperature / (t2 - lethalLowTemperature);
				maxf = maxf < f ? maxf : f;
				lethalLowTemperature -=  ((t2 - lethalLowTemperature) * maxf);
				lethalHighTemperature +=  ((lethalHighTemperature - t2) * maxf);

			}
		}
	}
}
