﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Klei.AI;
using TUNING;
namespace 小人获得更多特质
{
	//[HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
	public class 小人获得更多特质{
		public void GenerateTraitsHook()
		{
			//object ins = this;
			//var __instance = ins as MinionStartingStats;
			//if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.获得所有好特质)
			//{
			//	foreach (DUPLICANTSTATS.TraitVal traitVal1 in DUPLICANTSTATS.BADTRAITS)
			//	{
			//		__instance.Traits.RemoveAll(i => i.Id == traitVal1.id);
			//	}
			//	
			//	foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			//	{
			//		Trait item2 = Db.Get().traits.TryGet(traitVal2.id);
			//		if ( ( traitVal2.id != "GlowStick") && traitVal2.id != "Uncultured")
			//		{
			//			if (!item2.IsNullOrDestroyed())
			//				__instance.Traits.Add(item2);
			//		}
			//	
			//	}
			//	foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.GENESHUFFLERTRAITS)
			//	{
			//		Trait item3 = Db.Get().traits.TryGet(traitVal3.id);
			//		if (!item3.IsNullOrDestroyed())
			//			__instance.Traits.Add(item3);
			//	}
			//	foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.JOYTRAITS)
			//	{
			//		Trait item4 = Db.Get().traits.TryGet(traitVal4.id);
			//		if (!item4.IsNullOrDestroyed())
			//			__instance.Traits.Add(item4);
			//	}
			//}
	
		}

		static MethodInfo[] GenerateTraits = typeof(MinionStartingStats)
			.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
			Where(m => m.Name == "GenerateTraits").ToArray();
		public 小人获得更多特质()
		{
			Harmony harmony = new Harmony("小人获得更多特质");
			Exception ex;
			foreach (MethodInfo methodInfo in GenerateTraits){
				
				harmony.Patch(methodInfo, postfix:new HarmonyMethod( typeof(小人获得更多特质).GetMethod(nameof(GenerateTraitsHook))));
			}
		}
	}
}
