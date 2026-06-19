using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TUNING;
using static TUNING.DUPLICANTSTATS;
namespace 大一统
{
    [AnyHarmonyPatch(typeof(MinionStartingStats), "GenerateTraits", ControlName: new string[] { nameof(大一统.大一统控制台UI.获得所有好特质) })]
    public class 小人获得更多特质
    {
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
                List<DUPLICANTSTATS.TraitVal> AddedTraits = new List<TraitVal>();
                AddedTraits.AddRange(DUPLICANTSTATS.GOODTRAITS);
                AddedTraits.AddRange(DUPLICANTSTATS.GENESHUFFLERTRAITS);
                AddedTraits.AddRange(DUPLICANTSTATS.JOYTRAITS);
                AddedTraits.RemoveAll(t =>
                {
                    if (t.id == "GlowStick")
                    {
                        return true;
                    }
                    if (t.id == "Uncultured")
                    {
                        return true;
                    }
                    if (DlcManager.IsAllContentSubscribed(t.requiredDlcIds) == false)
                    {
                        return true;
                    }
                    if(__instance.Traits.Any(t1 => t1.Id == t.id))
                    {
                        return true;
                    }
                    return false;
                });

                foreach (DUPLICANTSTATS.TraitVal traitVal in AddedTraits)
                {
                    Trait item3 = Db.Get().traits.TryGet(traitVal.id);
                    if (!item3.IsNullOrDestroyed())
                        __instance.Traits.Add(item3);
                }
            }

        }

    }
}
