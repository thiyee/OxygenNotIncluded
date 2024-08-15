using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 效果修改
{
	public class 效果修改
	{
        public 效果修改(){
			MemberInfo[] memberInfo = typeof(Klei.AI.AttributeModifier).GetMembers().Where(m => m.Name == ".ctor").ToArray();
			Harmony harmony = new Harmony("效果修改");
			foreach (ConstructorInfo constructorInfo in memberInfo){
				harmony.Patch(constructorInfo, prefix: new HarmonyMethod(typeof(效果修改), nameof(AttributeModifier)));
			}

			MemberInfo[] Effects = typeof(Klei.AI.Effect).GetMembers().Where(m => m.Name == ".ctor").ToArray();
			Harmony Effectsharmony = new Harmony("效果修改");
			Strings.Add(new string[] { "STRINGS.CREATURES.ATTRIBUTES.AGEDELTA.NAME", "这个动物的产蛋速度增加了\n同时也会更快的老去." });
			foreach (ConstructorInfo constructorInfo in Effects){
				harmony.Patch(constructorInfo, postfix: new HarmonyMethod(typeof(效果修改), nameof(EffectCreate)));
			}
		}

		private static void EffectCreate(ref Klei.AI.Effect __instance,string id, string name)
		{
			if (name == STRINGS.CREATURES.MODIFIERS.TAME.NAME)
			{
				bool needtoadd = !__instance.SelfModifiers.Exists(e => e.AttributeId == Db.Get().Amounts.Fertility.deltaAttribute.Id);
				if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物产蛋速度 != 0 && needtoadd)
				{
					__instance.Add(new Klei.AI.AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物产蛋速度, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, true, false, true));
					__instance.Add(new Klei.AI.AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f* PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.动物产蛋速度*0.4F, STRINGS.CREATURES.MODIFIERS.AGE.NAME, false, false, true));

				}
			}
		}
		
			// Token: 0x06000007 RID: 7 RVA: 0x000020B0 File Offset: 0x000002B0
			private static void AttributeModifier(string attribute_id, ref float value)
		{
			
			if (attribute_id == Db.Get().Amounts.Wildness.deltaAttribute.Id)
			{
				if (value < 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.驯化速度;//驯化更快
			}

			if (attribute_id == Db.Get().Amounts.Maturity.deltaAttribute.Id)
			{
				if (value > 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.植物生长速度;//植物生长更快
			}
			if (attribute_id == Db.Get().Amounts.Incubation.deltaAttribute.Id)
			{
				if (value > 0) value *= PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.孵化速度; // 将孵化速度增加的倍数替换为你所需的值
			}

			if(attribute_id== "ScaldingThreshold"|| attribute_id== "ScoldingThreshold")
            {
				if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.祖宗人)
				{
					if (attribute_id == "ScaldingThreshold") value = 10000;
					if (attribute_id == "ScoldingThreshold") value = 0;
				}
			}

		}

	}
}
