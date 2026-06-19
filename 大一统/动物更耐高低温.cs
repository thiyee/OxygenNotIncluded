using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static EntityTemplates;

namespace 大一统
{
    [AnyHarmonyPatch(typeof(EntityTemplates),
        "ExtendEntityToBasicCreature",
        ControlName: new string[] { nameof(大一统.大一统控制台UI.动物更耐高低温) },
        ArgumentTypes: new Type[] { typeof(ExtendEntityToBasicCreatureData) }
        )]

    public class 动物体质增强
    {
        public static void Prefix(ExtendEntityToBasicCreatureData data)
        {
            float f = 大一统.大一统控制台UI.Instance.动物更耐高低温;
            if (f > 0)
            {

                var t1 = (data.warningLowTemperature + data.warningHighTemperature) / 2;
                var t2 = (data.lethalLowTemperature + data.lethalHighTemperature) / 2;


                var maxf = data.warningLowTemperature / (t1 - data.warningLowTemperature);
                maxf = maxf < f ? maxf : f;
                data.warningLowTemperature -= ((t1 - data.warningLowTemperature) * maxf);
                data.warningHighTemperature += ((data.warningHighTemperature - t1) * maxf);


                maxf = data.lethalLowTemperature / (t2 - data.lethalLowTemperature);
                maxf = maxf < f ? maxf : f;
                data.lethalLowTemperature -= ((t2 - data.lethalLowTemperature) * maxf);
                data.lethalHighTemperature += ((data.lethalHighTemperature - t2) * maxf);

            }
        }
    }
}
