using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using PatchProcessor = HarmonyLib.PatchProcessor;
using CodeInstruction = HarmonyLib.CodeInstruction;

// Harmony补丁标记：绑定到自定义控制台模组「大一统.大一统控制台UI.最后的基地」
// 1. 对 HeadquartersConfig.CreateBuildingDef 后置补丁：修改基地建筑基础属性
[AnyHarmonyPatch(typeof(HeadquartersConfig), "CreateBuildingDef", Postfix: nameof(CreateBuildingDef), ControlName: new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]
// 2. 对 HeadquartersConfig.DoPostConfigureComplete 后置补丁：给基地挂载组件、生成全物种/种子兑换配方
[AnyHarmonyPatch(typeof(HeadquartersConfig), "DoPostConfigureComplete", Postfix: nameof(DoPostConfigureComplete), ControlName: new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]
// 3. 对 CarePackageInfo 构造函数 前置补丁：删除空投物资解锁限制
[AnyHarmonyPatch(typeof(CarePackageInfo), ".ctor", Prefix: nameof(CarePackageInfo), ControlName: new string[] { nameof(大一统.大一统控制台UI.最后的基地) })]


//[AnyHarmonyPatch(typeof(LegacyModMain),"Load",Postfix:nameof(Load))]
/// <summary>
/// 自定义基地建筑补丁类
/// 作用：修改原版总部建筑，做成全能兑换台，支持用铌兑换所有种子、生物蛋、数据磁盘等
/// </summary>
public class 最后的基地
{

    private static void Load()
    {
        //不直接patch HeadquartersConfig.DoPostConfigureComplete 是因为 动物蛋实体在建筑加载之后才会加载
        
        var obj = Assets.GetBuildingDef(HeadquartersConfig.ID)?.BuildingComplete;
        DoPostConfigureComplete(ref obj);
    }
    /// <summary>
    /// HeadquartersConfig.CreateBuildingDef 后置补丁
    /// 作用：修改建筑基础定义(BuildingDef)，重写发电、电力相关参数
    /// </summary>
    /// <param name="__result">原版方法返回的建筑定义对象，ref修改全局属性</param>
    private static void CreateBuildingDef(ref BuildingDef __result)
    {
        // 发电机额定功率1000瓦
        __result.GeneratorWattageRating = 1000f;
        // 电池储能容量20000焦耳
        __result.GeneratorBaseCapacity = 20000f;
        // 建筑需要向外输出电力
        __result.RequiresPowerOutput = true;
        // 电力输出格偏移(0,0)，自身格子放电
        __result.PowerOutputOffset = new CellOffset(0, 0);
        // 选中建筑时默认显示电力 overlay 图层
        __result.ViewMode = OverlayModes.Power.ID;
    }

    /// <summary>
    /// HeadquartersConfig.DoPostConfigureComplete 后置补丁
    /// 作用：建筑实体GameObject创建完成后，挂载所有功能组件、自动生成全物种兑换配方
    /// 功能：制氧、内置发电机、复杂制造台、全种子/蛋/特殊物品兑换配方
    /// </summary>
    /// <param name="go">建筑对应的游戏实体GameObject</param>
    private static void DoPostConfigureComplete(ref GameObject go)
    {
        // 氧气排放口偏移：建筑上方一格(0,1)
        CellOffset cellOffset = new CellOffset(0, 1);
        // 给建筑添加气体排放组件
        ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
        // 设置排放参数：0.5kg/s氧气，303.15K常温，无压力/辐射过滤，偏移(0,1)，最大浓度100%
        elementEmitter.outputElement = new ElementConverter.OutputElement(
            0.5f, SimHashes.Oxygen, 303.15f,
            false, false,
            (float)cellOffset.x, (float)cellOffset.y,
            1f, byte.MaxValue, 0, true);
        elementEmitter.emissionFrequency = 1f; // 每秒发射一次
        elementEmitter.maxPressure = 2.5f;     // 环境气压超过2.5kg则停止排放氧气

        // 添加开发发电机组件，实现自主发电
        DevGenerator devGenerator = go.AddOrGet<DevGenerator>();
        devGenerator.powerDistributionOrder = 9; // 电力分配优先级
        devGenerator.wattageRating = 1000f;     // 发电功率1000W

        // 拆除建筑时掉落全部内部物品
        go.AddOrGet<DropAllWorkable>();
        // 建筑不需要复制人手动开关，自动运行
        go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

        // 复杂制造台核心组件（原版精炼厂/冶炼机同款UI）
        ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
        // UI样式：列表+队列混合界面
        complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        // 需要复制人操作生产
        complexFabricator.duplicantOperated = true;
        // 配方原料状态显示面板
        go.AddOrGet<FabricatorIngredientStatusManager>();
        // 支持复制建筑复制配方设置
        go.AddOrGet<CopyBuildingSettings>();

        // 制造台工作动画控制器
        ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
        // 创建制造台配套储物仓
        BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
        // 使用岩石精炼厂的工作动画
        complexFabricatorWorkable.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_rockrefinery_kanim") };
        // 生产完成播放的动画状态
        complexFabricatorWorkable.workingPstComplete = new HashedString[] { "working_pst_complete" };

        #region 反射遍历所有实体配置，筛选可用种子/生物蛋
        // 反射遍历程序集所有实现IEntityConfig的实体配置类（全部动植物、蛋、种子）
        var items = typeof(IEntityConfig).Assembly.GetTypes()
            .Where(t =>
            {
                // 筛选：是实体配置、非抽象类
                if (!typeof(IEntityConfig).IsAssignableFrom(t) || t.IsAbstract)
                {
                    return false;
                }
                List<string> dlcs = new List<string>();
                var obj = Activator.CreateInstance(t) as IEntityConfig;
                if (!(obj is IEntityConfig config))
                {
                    return false;
                }

                if (obj is IHasDlcRestrictions dlcRestrictions)
                {
                    dlcs.AddRange(dlcRestrictions.GetRequiredDlcIds() ?? new string[] { });
                }
                else
                {
                    dlcs.AddRange(obj.GetDlcIds() ?? new string[] { });
                }

                bool DlcSupport= DlcManager.IsAllContentSubscribed(dlcs);
                //if (DlcSupport)
                //{
                //    Console.WriteLine($"{t.Name} dlcs:{string.Join(" ", dlcs)}");
                //}
                return DlcSupport;
            })
            // 从配置类取出静态常量：SEED_ID(种子) / EGG_ID(生物蛋)
            .Select(t => t.GetField("SEED_ID", BindingFlags.Public | BindingFlags.Static) ??
                        t.GetField("EGG_ID", BindingFlags.Public | BindingFlags.Static))
            // 过滤掉找不到ID字段的类
            .Where(field => field != null)
            // 封装兑换物品信息元组：物品Tag、分类名称、消耗铌数量、产出物品数量
            .Select(field =>
            {
                string id = field.GetValue(null) as string;
                string name = string.Empty;

                // ID后缀判断分类
                if (id.EndsWith("Egg"))
                {
                    name = "动物蛋";
                }
                else if (id.EndsWith("Seed"))
                {
                    name = "种子";
                }

                return (Tag: new Tag(id), Name: name, IngredientAmount: 1000f, ResultAmount: 1f);
            }).ToList();
        #endregion

        if (DlcManager.IsAllContentSubscribed(DlcManager.EXPANSION1))
        {
            items.Add((Tag: new Tag(OrbitalResearchDatabankConfig.ID), Name: "数据磁盘", IngredientAmount: 100f, ResultAmount: 100f));
            items.Add((Tag: new Tag(BabyBeeConfig.ID), Name: "辐射蜂幼崽", IngredientAmount: 1000f, ResultAmount: 1f));
            items.Add((Tag: new Tag(CritterTrapPlantConfig.ID + "Seed"), Name: "土星动物捕草种子", IngredientAmount: 1000f, ResultAmount: 1f));
        }

        // 控制台打印所有可兑换物品，调试用
        //foreach (var i in items)
        //{
        //    Console.WriteLine($"{i.Tag} {i.Name}");
        //}

        // 总部建筑专属制造标签，绑定配方归属
        Tag headquarters = TagManager.Create(HeadquartersConfig.ID);
        // 获取铌元素（兑换消耗材料）
        Element niobium = ElementLoader.FindElementByHash(SimHashes.Niobium);

        #region 循环批量生成兑换配方
        foreach (var item in items)
        {
            // 配方输入：消耗铌，固定消耗数量
            var input = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(niobium.tag, item.IngredientAmount, false) };
            // 配方输出：对应种子/蛋/特殊物品
            var output = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(item.Tag, item.ResultAmount, false) };

            // 旧版配方兼容ID（废弃配方映射用）
            string obsoleteId = ComplexRecipeManager.MakeObsoleteRecipeID(HeadquartersConfig.ID, input[0].material);
            // 生成唯一配方ID
            string recipeId = ComplexRecipeManager.MakeRecipeID(HeadquartersConfig.ID, input, output);

            // 创建制造配方实例
            var recipe = new ComplexRecipe(recipeId, input, output)
            {
                time = 1f, // 制造耗时1秒，秒出成品
                description = string.IsNullOrEmpty(item.Name) ? "" : $"兑换 {item.Name}", // 配方描述
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult, // 配方名称显示：原料→产物
                fabricators = new List<Tag> { headquarters } // 仅本基地建筑可生产
            };

            // 绑定旧配方ID到新配方，兼容旧存档
            ComplexRecipeManager.Get().AddObsoleteIDMapping(obsoleteId, recipeId);
        }
        #endregion

        return;
    }

    /// <summary>
    /// CarePackageInfo 构造函数前置补丁
    /// 作用：清空空投物资解锁条件，所有空投物资无解锁限制直接可用
    /// </summary>
    /// <param name="ID">空投ID</param>
    /// <param name="amount">空投物资数量</param>
    /// <param name="requirement">空投解锁条件委托，置null取消限制</param>
    private static void CarePackageInfo(ref string ID, ref float amount, ref Func<bool> requirement)
    {
        requirement = null;
    }
}