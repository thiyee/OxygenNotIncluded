using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 按摩床恢复速度
{
	[HarmonyPatch(typeof(MassageTableConfig), "DoPostConfigureComplete")]
	public class 按摩床恢复速度{
		public static void Postfix(GameObject go)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.按摩床恢复速度){

				MassageTable massageTable = go.GetComponent<MassageTable>();
				massageTable.stressModificationValue *= 10f;                //按摩床效率*10
				massageTable.roomStressModificationValue *= 10f;                //按摩床效率*10
			}

		}
	}
}
