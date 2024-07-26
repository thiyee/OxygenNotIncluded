using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace 自动收获
{
	[HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicPlant")]
	public class 自动收获
	{
		static bool init=false;
		private static void Prefix(ref GameObject template, ref float max_age, string crop_id, ref bool can_tinker, ref float temperature_lethal_low, ref float temperature_warning_low, ref float temperature_warning_high, ref float temperature_lethal_high, ref bool pressure_sensitive)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.自动收获)
				max_age = 3;//植物自动掉落

			if(PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.一键设置长周期植物)
			if (!init)
			{
				init = !init;
				Crop.CropVal[] crops = new Crop.CropVal[] {
					CROPS.CROP_TYPES.Find(i => i.cropId == "ColdWheatSeed") ,
					CROPS.CROP_TYPES.Find(i => i.cropId == "PlantMeat"),
					CROPS.CROP_TYPES.Find(i => i.cropId == "BeanPlantSeed"),
					CROPS.CROP_TYPES.Find(i => i.cropId == "Lettuce"),
					CROPS.CROP_TYPES.Find(i => i.cropId == "SpiceNut"),
					};


				for (int i = 0; i < crops.Count(); i++)
				{
					CROPS.CROP_TYPES.RemoveAll(t => t.cropId == crops[i].cropId);
					crops[i].cropDuration /= 4;
					crops[i].numProduced /= 2;
					CROPS.CROP_TYPES.Add(crops[i]);
				}
			}
            if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.繁茂核心)
            {
				if (temperature_lethal_low > 101f) temperature_lethal_low -= 100f;
				if (temperature_warning_low > 101f) temperature_warning_low -= 100f;
				temperature_warning_high += 100f;
				temperature_lethal_high += 100f;
				pressure_sensitive = false;

			}
		}
	}
}
