using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 超视望远镜
{
	[HarmonyPatch(typeof(ClusterTelescope.Def), MethodType.Constructor)]
	public class 望远镜范围
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000218B File Offset: 0x0000038B
		private static void Postfix(ref ClusterTelescope.Def __instance)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.超视望远镜)
				__instance.analyzeClusterRadius = 15;
		}
	}
}
