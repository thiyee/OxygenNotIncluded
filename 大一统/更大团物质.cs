using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 更大团物质
{
	[AnyHarmonyPatch(typeof(ElementSplitterComponents), "CanFirstAbsorbSecond", Prefix: nameof(CanFirstAbsorbSecondHook))]

	public class 更大团物质{
		private static bool CanFirstAbsorbSecondHook(HandleVector<int>.Handle first, HandleVector<int>.Handle second)
		{
			if (first == HandleVector<int>.InvalidHandle || second == HandleVector<int>.InvalidHandle)
			{
				return false;
			}
			ElementSplitter data = GameComps.ElementSplitters.GetData(first);
			ElementSplitter data2 = GameComps.ElementSplitters.GetData(second);
			return data.primaryElement.ElementID == data2.primaryElement.ElementID &&
				data.primaryElement.Units + data2.primaryElement.Units < PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.更大团物质 &&
				!data.kPrefabID.HasTag(GameTags.MarkedForMove) && !data2.kPrefabID.HasTag(GameTags.MarkedForMove);
		}

    }
	[HarmonyPatch(typeof(PrimaryElement), MethodType.Constructor)]
	public class 更大团物质_PrimaryElementPatch
	{
		private static void Prefix(ref float ___MAX_MASS)
		{
			___MAX_MASS = PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.更大团物质;
		}
	}
}
