using HarmonyLib;

namespace 捕捉飞行动物和鱼
{
    [HarmonyPatch(typeof(EntityTemplates), "CreateAndRegisterBaggedCreature")]
	public class 九天揽月
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Prefix(ref bool allow_mark_for_capture)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.捕捉飞行动物和鱼)
				allow_mark_for_capture = true;
		}
	}
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	[HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToFertileCreature")]
	public class 下海捞鱼
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		public static void Prefix(ref bool add_fixed_capturable_monitor)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.捕捉飞行动物和鱼)
				add_fixed_capturable_monitor = true;
		}
	}
}
