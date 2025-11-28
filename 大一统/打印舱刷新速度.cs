using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统
{
    [AnyHarmonyPatch(typeof(Immigration), "Sim200ms",Prefix:nameof(Sim200ms), ControlName: new string[] { nameof(大一统.大一统控制台UI.打印舱刷新速度) })]

    class 打印舱刷新速度
    {
        public static void Sim200ms(ref float dt)
        {
            dt *= 大一统.大一统控制台UI.Instance.打印舱刷新速度;
        }
    }
}
