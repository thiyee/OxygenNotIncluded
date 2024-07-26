using HarmonyLib;
using PeterHan.PLib.Options;
using UnityEngine;
using 大一统;

namespace 按摩床恢复速度
{
	[HarmonyPatch(typeof(MassageTableConfig), "ConfigureBuildingTemplate")]
	public class 按摩床恢复速度
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Postfix(GameObject go)
		{
			if (SingletonOptions<大一统控制台UI>.Instance.按摩床恢复速度)
			{
				MassageTable massageTable = go.AddOrGet<MassageTable>();
				massageTable.stressModificationValue *= 10f;                //按摩床效率*10
				massageTable.roomStressModificationValue *= 10f;                //按摩床效率*10
			}
		}
	}
}
