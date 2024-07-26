using ProcGen;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcGenGame;
using static ProcGen.SubWorld;

namespace 虚空入侵
{
	[HarmonyPatch(typeof(WorldGen), "RenderOffline")]
	public class 虚空入侵
    {
		static bool isinit = false;

		private static void Prefix(ref WorldGen __instance){
            if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.虚空入侵)
                if (!isinit && __instance.isStartingWorld)
                SetRandomFifthNodesToSpace(ref __instance);
        }

        public static void SetRandomFifthNodesToSpace(ref WorldGen __instance)
        {
            // 获取 __instance.data.overworldCells 的引用
            List<TerrainCell> overworldCells = __instance.data.overworldCells;

            // 创建一个列表，用于存储满足条件的节点
            List<int> validIndices = new List<int>();

            // 将满足条件的节点索引添加到 validIndices 列表中
            for (int i = 1; i < overworldCells.Count; i++)
            {
                ZoneType currentNodeZoneType = SettingsCache.GetCachedSubWorld(overworldCells[i].node.type).zoneType;

                if (currentNodeZoneType != ZoneType.Space)
                {
                    validIndices.Add(i);
                }
            }

            // 计算需要更改的节点数量
            int numNodesToChange = (int)(validIndices.Count * PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.虚空入侵程度);

            // 创建一个随机数生成器
            Random random = new Random();

            // 从 validIndices 列表中随机选择 numNodesToChange 个节点
            for (int i = 0; i < numNodesToChange && validIndices.Count > 0; i++)
            {
                int randomIndexInList = random.Next(validIndices.Count);
                int randomIndex = validIndices[randomIndexInList];

                // 设置选定节点的类型为 "subworlds/space/Space"
                overworldCells[randomIndex].node.SetType("subworlds/space/Space");

                // 从 validIndices 列表中移除已经设置过的节点索引，防止重复选择
                validIndices.RemoveAt(randomIndexInList);
            }
        }


    }
}
