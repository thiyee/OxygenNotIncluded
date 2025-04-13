using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Klei.AI;
using TUNING;
namespace 大一统{
	[AnyHarmonyPatch(typeof(MinionStartingStats), "GenerateTraits", ControlName: new string[] { nameof(大一统.大一统控制台UI.获得所有好特质) })]
	public class 小人获得更多特质{
		public static void Postfix(MinionStartingStats __instance)
		{
				if (__instance.personality.model == GameTags.Minions.Models.Bionic)
				{
					// 添加机械升级特质
					foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.BIONICUPGRADETRAITS)
					{
						Trait trait = Db.Get().traits.TryGet(traitVal.id);
						if (trait != null && !__instance.Traits.Any(t => t.Id == traitVal.id))
						{
							__instance.Traits.Add(trait);
						}
					}

					// 移除机械缺陷特质(可选)
					__instance.Traits.RemoveAll(trait =>
						DUPLICANTSTATS.BIONICBUGTRAITS.Any(bugTrait => bugTrait.id == trait.Id));
                }
                else
                {
					foreach (DUPLICANTSTATS.TraitVal traitVal1 in DUPLICANTSTATS.BADTRAITS)
					{
						__instance.Traits.RemoveAll(i => i.Id == traitVal1.id);
					}

					foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
					{
						Trait item2 = Db.Get().traits.TryGet(traitVal2.id);
						if ((traitVal2.id != "GlowStick") && traitVal2.id != "Uncultured")
						{
							if (!item2.IsNullOrDestroyed())
								__instance.Traits.Add(item2);
						}

					}
					foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.GENESHUFFLERTRAITS)
					{
						Trait item3 = Db.Get().traits.TryGet(traitVal3.id);
						if (!item3.IsNullOrDestroyed())
							__instance.Traits.Add(item3);
					}
					foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.JOYTRAITS)
					{
						Trait item4 = Db.Get().traits.TryGet(traitVal4.id);
						if (!item4.IsNullOrDestroyed())
							__instance.Traits.Add(item4);
					}
				}
			
	
		}

	}
}
