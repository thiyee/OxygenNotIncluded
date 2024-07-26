using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 效果修改
{
	[HarmonyPatch(typeof(Klei.AI.AttributeModifier), MethodType.Constructor, new Type[] { typeof(string), typeof(float), typeof(string), typeof(bool), typeof(bool), typeof(bool) })]
	public class 效果修改
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
		private static void Prefix(string attribute_id, ref float value, ref string description, ref bool is_multiplier, ref bool uiOnly, ref bool is_readonly)
		{
			
			if (attribute_id == Db.Get().Amounts.Wildness.deltaAttribute.Id)
			{
				if (value < 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.驯化速度;//驯化更快
			}

			if (attribute_id == Db.Get().Amounts.Maturity.deltaAttribute.Id)
			{
				if (value > 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.植物生长速度;//植物生长更快
			}
			if (attribute_id == Db.Get().Amounts.Fertility.deltaAttribute.Id)
			{
				//Debug.Log("产蛋效果:"+value);
				if (value == 9) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物产蛋速度; //动物生长/产蛋更快
			}
			if (attribute_id == Db.Get().Amounts.Incubation.deltaAttribute.Id)
			{
				if (value > 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.孵化速度; // 将孵化速度增加的倍数替换为你所需的值
			}
		}
	}
}
