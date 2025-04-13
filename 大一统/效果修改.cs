using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(Klei.AI.Effect), ".ctor",Postfix: nameof(EffectCreate),ControlName:new string[] { nameof(大一统.大一统控制台UI.动物产蛋速度)})]
	[AnyHarmonyPatch(typeof(Klei.AI.AttributeModifier), ".ctor", Prefix: nameof(AttributeModifier), ControlName: new string[] { nameof(大一统.大一统控制台UI.驯化速度), nameof(大一统.大一统控制台UI.植物生长速度), nameof(大一统.大一统控制台UI.孵化速度),  nameof(大一统.大一统控制台UI.祖宗人) })]

	public class 效果修改
	{
		private static void EffectCreate(ref Klei.AI.Effect __instance,string id, string name)
		{
			if (name == STRINGS.CREATURES.MODIFIERS.TAME.NAME)
			{
				bool needtoadd = !__instance.SelfModifiers.Exists(e => e.AttributeId == Db.Get().Amounts.Fertility.deltaAttribute.Id);
				if (大一统.大一统控制台UI.Instance.动物产蛋速度 != 0 && needtoadd)
				{
					__instance.Add(new Klei.AI.AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, 大一统.大一统控制台UI.Instance.动物产蛋速度 * 0.2F, STRINGS.CREATURES.MODIFIERS.AGE.NAME, false, false, true));
					__instance.Add(new Klei.AI.AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 大一统.大一统控制台UI.Instance.动物产蛋速度, STRINGS.CREATURES.MODIFIERS.TAME.NAME, true, false, true));
					__instance.Add(new Klei.AI.AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f* 大一统.大一统控制台UI.Instance.动物产蛋速度*0.4F, STRINGS.CREATURES.MODIFIERS.AGE.NAME, false, false, true));

				}
			}
		}
		private static void AttributeModifier(string attribute_id, ref float value)
		{
			
			if (attribute_id == Db.Get().Amounts.Wildness.deltaAttribute.Id)
			{
				if (value < 0) value *= 大一统.大一统控制台UI.Instance.驯化速度;//驯化更快
			}

			if (attribute_id == Db.Get().Amounts.Maturity.deltaAttribute.Id)
			{
				if (value > 0) value *= 大一统.大一统控制台UI.Instance.植物生长速度;//植物生长更快
			}
			if (attribute_id == Db.Get().Amounts.Incubation.deltaAttribute.Id)
			{
				if (value > 0) value *= 大一统.大一统控制台UI.Instance.孵化速度; // 将孵化速度增加的倍数替换为你所需的值
			}

			if(attribute_id== "ScaldingThreshold"|| attribute_id== "ScoldingThreshold")
            {
				if (大一统.大一统控制台UI.Instance.祖宗人)
				{
					if (attribute_id == "ScaldingThreshold") value = 10000;
					if (attribute_id == "ScoldingThreshold") value = 0;
				}
			}

		}

	}
}
