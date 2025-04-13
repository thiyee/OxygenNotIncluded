using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
	[AnyHarmonyPatch(typeof(ElementLoader), "CopyEntryToElement", ControlName: new string[] { nameof(大一统.大一统控制台UI.物质大一统),nameof(大一统.大一统控制台UI.最低结块质量),nameof(大一统.大一统控制台UI.物质导热系数) })]
	public class 物质大一统
	{
		private static void Prefix(ref ElementLoader.ElementEntry entry, ref Element elem)
		{
			if (entry.thermalConductivity > 0f&& entry.elementId!= "Creature")
			{
				entry.thermalConductivity *= 大一统.大一统控制台UI.Instance.物质导热系数;
			}
			if (大一统.大一统控制台UI.Instance.最低结块质量!=0&&entry.defaultMass < 大一统.大一统控制台UI.Instance.最低结块质量 && entry.defaultMass > 1f) 
				entry.defaultMass = 大一统.大一统控制台UI.Instance.最低结块质量;

				if (entry.elementId == "Niobium") entry.highTemp += 3000;
				if (entry.elementId == "TempConductorSolid") entry.highTemp += 3000;
				if (entry.elementId == "Ceramic") entry.highTemp += 5000;
				if (entry.elementId == "SuperInsulator") entry.thermalConductivity = 0.0000001f;
				if (entry.elementId == "MoltenUranium") entry.lowTempTransitionTarget = "EnrichedUranium";
				if (entry.elementId == "SuperCoolant") { entry.highTemp = 8500f; entry.specificHeatCapacity *= 30.0f; }
				if (entry.elementId == "ViscoGel") { entry.lowTemp = 20.15f; }
			
			if (entry.tags == null)
            {
				entry.tags = new string[] { "BuildableAny" };

			}
            if (!entry.tags.Contains("BuildableAny"))
            {
				entry.tags.AddItem("BuildableAny");

			}


		}
	}




}