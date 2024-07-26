using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace 强化液体加热器
{


    public class 液体加热器滑块组件 : KMonoBehaviour, IDualSliderControl
    {
        int[] DecimalPlaces = new int[] { 0, 0 };
        int[] SliderMin = new int[] { 358, 4000 };
        int[] SliderMax = new int[] { 9750, 400000 };
        int[] SliderValue = new int[] { 358, 4000 };
        string[] SliderTooltipKey = new string[] { "目标温度", "发热效率" };
        public int SliderDecimalPlaces(int index) { return DecimalPlaces[index]; }
        public float GetSliderMin(int index) { return SliderMin[index]; }
        public float GetSliderMax(int index) { return SliderMax[index]; }
        public float GetSliderValue(int index) { return SliderValue[index]; }
        public void SetSliderValue(float percent, int index) { SliderValue[index] = Convert.ToInt32(percent); OnValueChange(); }
        public string GetSliderTooltipKey(int index) { return SliderTooltipKey[index]; }
        public string GetSliderTooltip() { return "SliderTooltip"; }
        public string SliderTitleKey
        {
            get { return "STRINGS.UI.UISIDESCREENS.LIQUIDHEATERSIDESCREEN.SLIDERTITLE"; }
        }
        public string SliderUnits
        {
            get { return ""; }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
        }

        private void OnCopySettings(object data)
        {
            液体加热器滑块组件 component = ((GameObject)data).GetComponent<液体加热器滑块组件>();
            if (component != null){
                this.SliderValue = component.SliderValue;
                OnValueChange();
            }
        }
        private void OnValueChange(){
            this.LiquidHeater.targetTemperature = SliderValue[0];
            this.gameObject.GetComponent<Building>().Def.ExhaustKilowattsWhenActive = SliderValue[1];
        }
        [MyCmpReq]
        public SpaceHeater LiquidHeater;
    }

    class 强化液体加热器
    {

        [HarmonyPatch(typeof(LiquidHeaterConfig), "CreateBuildingDef")]
        public class 防止加热器过热
        {
            public static void Postfix(ref BuildingDef __result )
            {
                __result.OverheatTemperature = 10000f;
            }
        }

        [HarmonyPatch(typeof(LiquidHeaterConfig), "DoPostConfigureComplete")]
        public class 为加热器添加滑块组件
        {
            public static void Postfix(GameObject go)
            {
                go.GetComponent<Building>().Def.Overheatable = false;
                go.AddOrGet<液体加热器滑块组件>();
            }
        }

        [HarmonyPatch(typeof(DualSliderSideScreen), "SetTarget")]
        public class DualSliderSideScreen_SetTarget_Patch {
            public static void Prefix(DualSliderSideScreen __instance, GameObject new_target)
            {
                bool flag = new_target.GetComponent<液体加热器滑块组件>() == null;
                if (!flag)
                {
                    List<SliderSet> sliderSets = __instance.sliderSets;
                    for (int i = 0; i < sliderSets.Count; i++)
                    {
                        sliderSets[i].index = i;
                    }
                }
            }
        }
    }
}
