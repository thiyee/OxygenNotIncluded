using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace 大一统
{

    [SerializationConfig(MemberSerialization.OptIn)]
    public class SuperSpaceHeater : StateMachineComponent<SuperSpaceHeater.StatesInstance>, IGameObjectEffectDescriptor, IDualSliderControl, ISliderControl
    {
        private float[] SliderMin = new float[2] { -273.14f, 0f };
        private float[] SliderMax = new float[2] { 9999f - 273.15f, 100f };
        private float[] SliderValue = new float[2] { 100f, 0.1f };
        private int[] SliderDecimalPlace = new int[2] { 4, 5 };
        private string[] SliderTooltip = new string[2] { "目标温度(℃)", "发热速度(℃/s)" };
        private string[] SliderTooltipKey = new string[2] { "℃", "℃/s" };

        [Serialize]
        public float TargetTemperature
        {
            get { return SliderValue[0]; }
            set { SliderValue[0] = Mathf.Clamp(value,SliderMin[0], SliderMax[0]); }
        }
        public float targetTemperature => TargetTemperature + 273.15f;
        [Serialize]
        public float HeatingRate
        {
            get { return SliderValue[1]; }
            set { SliderValue[1] = Mathf.Clamp(value,SliderMin[1], SliderMax[1] ); }
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate (object obj)
            {
                Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
            }, null, null);
            var rangeVisualizer = base.GetComponent<RangeVisualizer>();
            HeatCells = new int[rangeVisualizer.RangeMax.x - rangeVisualizer.RangeMin.x + 1, rangeVisualizer.RangeMax.y - rangeVisualizer.RangeMin.y + 1];
            HeatEnableMap = new bool[rangeVisualizer.RangeMax.x - rangeVisualizer.RangeMin.x + 1, rangeVisualizer.RangeMax.y - rangeVisualizer.RangeMin.y + 1];
            WorldID = base.gameObject.GetMyWorldId();
            var thispos = Grid.CellToPos2D(Grid.PosToCell(this));
            for (int i = 0; i <= rangeVisualizer.RangeMax.x - rangeVisualizer.RangeMin.x; i++)
            {
                for (int j = 0; j <= rangeVisualizer.RangeMax.y - rangeVisualizer.RangeMin.y; j++)
                {
                    HeatCells[i, j] = Grid.XYToCell((int)(thispos.x + rangeVisualizer.RangeMin.x + i), (int)(thispos.y + rangeVisualizer.RangeMin.y + j));
                    HeatEnableMap[i, j] = true;
                }
            }

            CellHeatData = new float[HeatCells.GetLength(0), HeatCells.GetLength(1)];
            this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
            //Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "SuperSpaceHeaterCellChanged");
            base.smi.StartSM();
        }
        protected override void OnPrefabInit()
        {
            this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
            this.heatStatusItem.resolveStringCallback = delegate (string str, object data)
            {
                SuperSpaceHeater.StatesInstance statesInstance = (SuperSpaceHeater.StatesInstance)data;
                return $"发热速度:{statesInstance.master.HeatingRate}℃/s";
            };
            this.heatStatusItem.resolveTooltipCallback = delegate (string str, object data)
            {
                SuperSpaceHeater.StatesInstance smi = (SuperSpaceHeater.StatesInstance)data;

                StringBuilder result = new StringBuilder();
                int width = smi.master.HeatCells.GetLength(0);
                int height = smi.master.HeatCells.GetLength(1);

                for (int y = height - 1; y >= 0; y--)
                {

                    for (int x = 0; x < width; x++)
                    {
                        float heat = CellHeatData[x, y];
                        // 二叉搜索找到合适的单位规则
                        int left = 0, right = unitRules.Length - 1;
                        while (left <= right)
                        {
                            int mid = left + (right - left) / 2;
                            if (heat < unitRules[mid].threshold)
                                right = mid - 1;
                            else
                                left = mid + 1;

                        }

                        // 处理找到的规则
                        var rule = unitRules[left < unitRules.Length ? left : unitRules.Length - 1];
                        string value = rule.divisor > 0 ? (heat / rule.divisor).ToString(rule.format) : rule.unit;

                        result.Append($"[{value}{rule.unit}]");
                    }
                    result.AppendLine();
                }
                result.Append("DTU");
                return result.ToString();
            };

            base.OnPrefabInit();
            base.Subscribe<SuperSpaceHeater>((int)GameHashes.CopySettings, SuperSpaceHeater.OnCopySettingsDelegate);
        }
        private void OnCopySettings(object data)
        {
            SuperSpaceHeater cmp = ((GameObject)data).GetComponent<SuperSpaceHeater>();
            this.TargetTemperature = cmp.TargetTemperature;
            this.HeatingRate = cmp.HeatingRate;
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            //Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
        }
        public static void GenerateHeat(SuperSpaceHeater.StatesInstance smi, float dt)
        {
            int width = smi.master.HeatCells.GetLength(0);
            int height = smi.master.HeatCells.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (smi.master.HeatEnableMap[i, j])
                    {
                        int cell = smi.master.HeatCells[i, j];
                        if (!Grid.IsValidCell(cell)) break;
                        float currentTemp = Grid.Temperature[cell];
                        float targetTemp = smi.master.targetTemperature;

                        // 计算温度差
                        float tempDifference = targetTemp - currentTemp;

                        // 计算需要的温度变化量（考虑加热速率限制）
                        float tempDelta = Mathf.Sign(tempDifference) * Mathf.Min(Mathf.Abs(tempDifference), dt * smi.master.HeatingRate);

                        // 计算需要的热能
                        float heatEnergy = Grid.Element[cell].specificHeatCapacity * Grid.Mass[cell] * tempDelta * 1000f;

                        SimMessages.ModifyEnergy(cell, kilojoules: heatEnergy / 1000f, targetTemp, SimMessages.EnergySourceID.StructureTemperature);
                        smi.master.CellHeatData[i, j] = heatEnergy;
                    }
                    else
                    {
                        smi.master.CellHeatData[i, j] = 0;
                    }
                }
            }
        }
        public static void RefreshHeatEffect(SuperSpaceHeater.StatesInstance smi)
        {
            if (smi.master.heatEffect != null)
            {
                smi.master.heatEffect.SetHeatBeingProducedValue(0);
            }
        }

        static (float threshold, float divisor, string format, string unit)[] unitRules = new (float threshold, float divisor, string format, string unit)[]
        {
            (1e1f, 1f, "F3", ""),      //#.###
            (1e2f, 1f, "F2", ""),      //##.##
            (1e3f, 1f, "F1", ""),      //###.#
            (1e4f, 1e3f, "F2", "K"),   //#.##K
            (1e5f, 1e3f, "F1", "K"),   //##.#K
            (1e6f, 1e3f, "F0", "K "),  //###K 
            (1e7f, 1e6f, "F2", "M"),   //#.##M
            (1e8f, 1e6f, "F1", "M"),   //##.#M
            (1e9f, 1e6f, "F0", "M "),  //###M 
            (1e10f, 1e9f, "F2", "G"),  //#.##G
            (1e11f, 1e9f, "F1", "G"),  //##.#G
            (1e12f, 1e9f, "F0", "G "), //###G 
            (1e13f, 1e12f, "F2", "T"), //#.##T
            (1e14f, 1e12f, "F1", "T"), //##.#T
            (1e15f, 1e12f, "F0", "T "),//###T 
            (1e16f, 1e15f, "F2", "KT"),//#.#KT
            (1e17f, 1e15f, "F1", "KT"),//##.KT
            (1e18f, 1e15f, "F0", "KT"),//###KT
            (1e19f, 1e18f, "F2", "MT"),//#.#MT
            (1e20f, 1e18f, "F1", "MT"),//##.MT
            (1e21f, 1e18f, "F0", "MT"),//###MT
            (1e22f, 1e21f, "F2", "GT"),//#.#GT
            (1e23f, 1e21f, "F1", "GT"),//##.GT
            (1e24f, 1e21f, "F0", "GT"),//###GT
            (float.MaxValue, 0f, "", "huge!") // 超大值
        };

        public void SetLiquidHeater()
        {
            this.heatLiquid = true;
        }
        public bool IsCellVisible(int cell)
        {
            Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
            Vector2I vector2I2 = Grid.CellToXY(cell);
            return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, (c) => { return Grid.IsSolidCell(c); }, true, false);
        }
        private void UpdateHeatEnableMap()
        {
            Grid.PosToCell(this);
            int width = smi.master.HeatCells.GetLength(0);
            int height = smi.master.HeatCells.GetLength(1);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int num = HeatCells[i, j];
                    HeatEnableMap[i, j] =
                        (Grid.IsValidCellInWorld(num, this.WorldID) && this.IsCellVisible(num)) &&
                        ((smi.master.heatLiquid && Grid.IsLiquid(num)) || (!smi.master.heatLiquid && !Grid.IsLiquid(num)));
                    // 移除温度比较条件，允许高于目标温度时降温
                }
            }
        }
        private MonitorState MonitorHeating(float dt)
        {
            this.monitorCells.Clear();
            GameUtil.GetNonSolidCells(Grid.PosToCell(base.transform.GetPosition()), this.radius, this.monitorCells);
            int num = 0;
            for (int i = 0; i < this.monitorCells.Count; i++)
            {
                if (Grid.Mass[this.monitorCells[i]] > this.minimumCellMass &&
                        (
                        (Grid.Element[this.monitorCells[i]].IsGas && !this.heatLiquid) ||
                        (Grid.Element[this.monitorCells[i]].IsLiquid && this.heatLiquid)
                        )
                   )
                {
                    num++;
                }
            }
            if (num == 0)
            {
                if (!this.heatLiquid)
                {
                    return MonitorState.NotEnoughGas;
                }
                return MonitorState.NotEnoughLiquid;
            }
            return MonitorState.ReadyToHeat; // 移除过热检查，允许降温
        }
        public List<Descriptor> GetDescriptors(GameObject go)
        {
            List<Descriptor> list = new List<Descriptor>();
            Descriptor item = default(Descriptor);
            item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
            list.Add(item);
            return list;
        }

        public string SliderTitleKey
        {
            get
            {
                return "STRINGS.UI.UISIDESCREENS.SUPERSPACEHEATERSIDESCREEN.TITLE";
            }
        }

        public string SliderUnits
        {
            get
            {
                return "";
            }
        }

        public int SliderDecimalPlaces(int index)
        {
            return SliderDecimalPlace[index];
        }

        public float GetSliderMin(int index)
        {
            return SliderMin[index];
        }

        public float GetSliderMax(int index)
        {
            return SliderMax[index];
        }

        public float GetSliderValue(int index)
        {
            return SliderValue[index];
        }

        public void SetSliderValue(float value, int index)
        {
            SliderValue[index] = value;
        }

        public string GetSliderTooltipKey(int index)
        {
            return SliderTooltipKey[index];
        }

        string ISliderControl.GetSliderTooltip(int index)
        {
            return SliderTooltip[index];
        }


        public float minimumCellMass;

        public int radius = 2;

        [SerializeField]
        private bool heatLiquid;

        [Serialize]
        public float UserSliderSetting;


        private StatusItem heatStatusItem;

        private HandleVector<int>.Handle structureTemperature;

        private int[,] HeatCells;
        private bool[,] HeatEnableMap;
        private float[,] CellHeatData;

        [MyCmpReq]
        private Operational operational;

        [MyCmpGet]
        private KBatchedAnimHeatPostProcessingEffect heatEffect;
        private List<int> monitorCells = new List<int>();
        private int WorldID;

        private static readonly EventSystem.IntraObjectHandler<SuperSpaceHeater> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SuperSpaceHeater>(delegate (SuperSpaceHeater component, object data)
        {
            component.OnCopySettings(data);
        });



        public class StatesInstance : GameStateMachine<SuperSpaceHeater.States, SuperSpaceHeater.StatesInstance, SuperSpaceHeater, object>.GameInstance
        {
            public StatesInstance(SuperSpaceHeater master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, SuperSpaceHeater>
        {
            public override void InitializeStates(out BaseState default_state)
            {
                default_state = this.offline;
                base.serializable = SerializeType.Never;
                this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
                this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
                this.statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
                this.statusItemOverTemp.resolveStringCallback = delegate (string str, object obj)
                {
                    StatesInstance statesInstance = (StatesInstance)obj;
                    return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
                };
                this.offline.Enter(new StateMachine<States, StatesInstance, SuperSpaceHeater, object>.State.Callback(RefreshHeatEffect)).EventTransition(GameHashes.OperationalChanged, this.online, (SuperSpaceHeater.StatesInstance smi) => smi.master.operational.IsOperational);
                this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.online.heating).Update("spaceheater_online", delegate (SuperSpaceHeater.StatesInstance smi, float dt)
                {
                    switch (smi.master.MonitorHeating(dt))
                    {
                        case MonitorState.ReadyToHeat:
                            smi.master.UpdateHeatEnableMap();
                            smi.GoTo(this.online.heating);
                            return;
                        case MonitorState.TooHot:
                            smi.GoTo(this.online.overtemp);
                            return;
                        case MonitorState.NotEnoughLiquid:
                            smi.GoTo(this.online.undermassliquid);
                            return;
                        case MonitorState.NotEnoughGas:
                            smi.GoTo(this.online.undermassgas);
                            return;
                        default:
                            return;
                    }
                }, UpdateRate.SIM_4000ms, false);
                this.online.heating.Enter(new StateMachine<SuperSpaceHeater.States, SuperSpaceHeater.StatesInstance, SuperSpaceHeater, object>.State.Callback(SuperSpaceHeater.RefreshHeatEffect)).Enter(delegate (SuperSpaceHeater.StatesInstance smi)
                {
                    smi.master.operational.SetActive(true, false);
                })
                    .ToggleStatusItem((SuperSpaceHeater.StatesInstance smi) => smi.master.heatStatusItem, (SuperSpaceHeater.StatesInstance smi) => smi)
                .Update(new Action<SuperSpaceHeater.StatesInstance, float>(SuperSpaceHeater.GenerateHeat), UpdateRate.SIM_200ms, false)
                .Exit(delegate (SuperSpaceHeater.StatesInstance smi)
                {
                    smi.master.operational.SetActive(false, false);
                })
                .Exit(new StateMachine<SuperSpaceHeater.States, SuperSpaceHeater.StatesInstance, SuperSpaceHeater, object>.State.Callback(SuperSpaceHeater.RefreshHeatEffect));
                this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, null);
                this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, null);
                this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, null);
            }

            public State offline;

            public OnlineStates online;

            private StatusItem statusItemUnderMassLiquid;

            private StatusItem statusItemUnderMassGas;

            private StatusItem statusItemOverTemp;

            public class OnlineStates : State
            {
                public State heating;

                public State overtemp;

                public State undermassliquid;

                public State undermassgas;
            }
        }

        private enum MonitorState
        {
            ReadyToHeat,
            TooHot,
            NotEnoughLiquid,
            NotEnoughGas
        }
    }
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.超级加热器) })]
    [AnyHarmonyPatch(typeof(SliderSet), "SetTarget", Postfix: nameof(SetTarget), ControlName: new string[] { nameof(大一统.大一统控制台UI.超级加热器) })]
    class 超级加热器
    {
        public static void SetTarget(SliderSet __instance)
        {
            int integerDigits = (__instance.numberInput.maxValue == 0) ? 1 : Mathf.FloorToInt(Mathf.Log10(__instance.numberInput.maxValue)) + 1;
            int decimalChars = (__instance.numberInput.decimalPlaces > 0) ? 1 + __instance.numberInput.decimalPlaces : 3; // 1 for '.'  
            __instance.numberInput.field.characterLimit = integerDigits + decimalChars;
        }
        private static void SpaceAddVisualizer(GameObject go)
        {
            RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
            rangeVisualizer.RangeMax = SpaceHeaterConfig.MAX_RANGE;
            rangeVisualizer.RangeMin = SpaceHeaterConfig.MIN_RANGE;
            rangeVisualizer.BlockingTileVisible = true;
            rangeVisualizer.BlockingCb=c=> Grid.IsSolidCell(c);
            go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
        }
        private static void LiquidAddVisualizer(GameObject go)
        {
            RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
            rangeVisualizer.RangeMax = new Vector2I(3, 1);
            rangeVisualizer.RangeMin = new Vector2I(-2, -1);
            rangeVisualizer.BlockingTileVisible = false;
            go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
        }


        public static readonly Tag SuperSpaceHeater = TagManager.Create("SuperSpaceHeater");
        public static void ExecuteOnInit()
        {

            GlobalBuildingConfig.CreateBuildingDef<SpaceHeaterConfig>(null, (config, def) =>
            {
                def.Overheatable = false;
                def.OverheatTemperature = 10000f;
                def.ExhaustKilowattsWhenActive = 0f;
                def.SelfHeatKilowattsWhenActive = 0f;
                def.ThermalConductivity = 0f;
            });
            GlobalBuildingConfig.CreateBuildingDef<LiquidHeaterConfig>(null, (config, def) =>
            {
                def.Overheatable = false;
                def.OverheatTemperature = 10000f;
                def.ExhaustKilowattsWhenActive = 0f;
                def.SelfHeatKilowattsWhenActive = 0f;
                def.ThermalConductivity = 0f;

            });

            GlobalBuildingConfig.ConfigureBuildingTemplate<SpaceHeaterConfig>((config, go, tag) =>
           {
               go.AddOrGet<LoopingSounds>();
               go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation, false);
               go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
               WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
               def.RangeMax = SpaceHeaterConfig.MAX_RANGE;
               def.RangeMin = SpaceHeaterConfig.MIN_RANGE;
               go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[][]{
                    new CellOffset[]{new CellOffset(-1, 0),new CellOffset(2, 0)},
                    new CellOffset[]{new CellOffset(0, 0),new CellOffset(1, 0)}
               };
               SpaceAddVisualizer(go);
               SuperSpaceHeater spaceHeater = go.AddOrGet<SuperSpaceHeater>();
               spaceHeater.TargetTemperature = 100f;
               spaceHeater.HeatingRate = 0.1f;
               return false;
           }, null);
            GlobalBuildingConfig.ConfigureBuildingTemplate<LiquidHeaterConfig>((config, go, tag) =>
            {
                go.AddOrGet<LoopingSounds>();
                go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation, false);
                go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
                WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
                def.RangeMax = new Vector2I(3, 1);
                def.RangeMin = new Vector2I(-2, -1);
                go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[][]{
                    new CellOffset[]{new CellOffset(-1, 0),new CellOffset(2, 0)},
                    new CellOffset[]{new CellOffset(0, 0),new CellOffset(1, 0)}
                };
                LiquidAddVisualizer(go);
                SuperSpaceHeater spaceHeater = go.AddOrGet<SuperSpaceHeater>();
                spaceHeater.TargetTemperature = 100f;
                spaceHeater.HeatingRate = 0.1f;
                spaceHeater.SetLiquidHeater();
                return false;
            }, null);
            GlobalBuildingConfig.DoPostConfigureUnderConstruction<LiquidHeaterConfig>(null, (config, go) => { LiquidAddVisualizer(go); });
            GlobalBuildingConfig.DoPostConfigurePreview<LiquidHeaterConfig>(null, (config, def, go) => { LiquidAddVisualizer(go); });


            GlobalBuildingConfig.DoPostConfigureComplete<SpaceHeaterConfig>(null, (config, go) => {
                go.AddOrGet<CopyBuildingSettings>().copyGroupTag = SuperSpaceHeater;
            });
            GlobalBuildingConfig.DoPostConfigureComplete<LiquidHeaterConfig>(null, (config, go) => {
                go.AddOrGet<CopyBuildingSettings>().copyGroupTag = SuperSpaceHeater;

            });


            Strings.Add(new string[] { "STRINGS.UI.UISIDESCREENS.SUPERSPACEHEATERSIDESCREEN.TITLE", "发热控制" });

        }
    }
}
