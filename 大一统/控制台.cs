using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace 大一统{
    [JsonObject(MemberSerialization.OptIn)]    
    [ConfigFile("大一统控制台.json", true, true)]    
    [RestartRequired]    
    public class 大一统控制台UI : SingletonOptions<大一统控制台UI>
    {

        [Option("更多泉与火山", "在创建新地图时生效 生成以下火山/泉 \n铀火山;钢火山;玻璃火山;超冷泉;石油泉;液磷泉;碳火山;蔗糖泉 ", "地图特质") ][JsonProperty] [DefaultValue(false)]public  bool 更多泉与火山 { get; set; }
        [Option("末日浩劫", "陨石群在星图上的移动速度-90%", "地图特质") ][JsonProperty] [DefaultValue(false)]public  bool 末日浩劫 { get; set; }
        [Option("虚空入侵", "在创建新地图时生效 随机替换初始生态以外的生态为太空暴露", "地图特质") ][JsonProperty] [DefaultValue(false)]public  bool 虚空入侵 { get; set; }
        [Option("入侵程度", "控制被替换的生态占总生态的多少 初始生态不会被替换", "地图特质", Format = "F2") ][Limit(0.0, 1.0) ][JsonProperty] public  float 虚空入侵程度 { get; set; }
        [Option("辐星高照", "控制主世界太空辐射浓度", "地图特质", Format = "F2") ][Limit(1.0, 30.0) ][JsonProperty] [DefaultValue(1.0f)] public  float 辐星高照 { get; set; }

        [Option("方块墙", "建造一个自然土块", "新的建筑") ][JsonProperty] [DefaultValue(false)]public  bool 方块墙 { get; set; }
        [Option("自动挖掘墙", "挖掘在其上的固体", "新的建筑") ][JsonProperty] [DefaultValue(false)]public  bool 自动挖掘墙 { get; set; }
        [Option("动物猎场", "自动处死进入的动物 可通过信号控制", "新的建筑") ][JsonProperty] [DefaultValue(false)]public  bool 动物猎场 { get; set; }
        [Option("幽灵门", "复制人可通过 但气体与液体不可通过的门", "新的建筑") ][JsonProperty] [DefaultValue(false)]public  bool 幽灵门 { get; set; }
        [Option("中子湮灭发生器", "发射大量辐射粒子", "新的建筑") ][JsonProperty] [DefaultValue(false)]public  bool 中子湮灭发生器 { get; set; }

        [Option("自动化", "所有建筑的配方任务均可自主运行", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 自动化 { get; set; }
        [Option("按摩床恢复速度", "按摩床恢复速度30%/秒", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 按摩床恢复速度 { get; set; }
        [Option("千里眼", "望远镜范围增加到15", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 千里眼 { get; set; }
        [Option("管道电解器", "允许电解器安装气体管道", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 管道电解器 { get; set; }
        [Option("改造储气液库", "储气库储液库能手动操作", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 改造储气液库 { get; set; }
        [Option("改造液氢引擎", "液氢引擎能灌入燃料", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 改造液氢引擎 { get; set; }
        [Option("高频电炉", "玻璃熔炉能融化任何固体物质", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 高频电炉 { get; set; }
        [Option("冷光吸顶灯", "吸顶灯不再发热", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 冷光吸顶灯 { get; set; }
        [Option("智能冰箱", "储存温度为-20℃ 更大容量", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 强化冰箱 { get; set; }
        [Option("最后的基地", "打印舱提供基础维生设备和电力设施 对新手友好", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 最后的基地 { get; set; }
        [Option("电线穿墙", "粗电线/导线穿墙", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 电线穿墙 { get; set; }
        [Option("光电效应", "太阳能板发电量增强 没有上限 告别CPU发电", "新建筑特性", Format = "F0") ][Limit(1, 10) ][JsonProperty] [DefaultValue(1)] public  float 光电效应 { get; set; }
        [Option("储物箱容量", "储物箱容量(吨)", "新建筑特性", Format = "F0") ][Limit(20, float.MaxValue) ][JsonProperty] [DefaultValue(20.0f)] public  float 储物箱容量 { get; set; }
        [Option("蒸汽时代", "蒸汽机能吸取速度*5 过热温度=200℃ 发热降低90%", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 蒸汽时代 { get; set; }
        [Option("强制隔热", "隔热砖 液体/气体管道 不发生热交换", "新建筑特性") ][JsonProperty] [DefaultValue(false)]public  bool 强制隔热 { get; set; }
        [Option("货舱扩容", "火箭上的所有类型货舱容量增加 百分比", "新建筑特性") ][JsonProperty] [Limit(100, 10000)] [DefaultValue(100)]public  int 货舱扩容 { get; set; }

        [Option("动物更耐高低温", "扩展动物的温度范围 单位[倍]", "功能性修改") ][JsonProperty] [Limit(0, 10)] [DefaultValue(0.0f)] public  float 动物更耐高低温 { get; set; }
        [Option("辐射蜂巢耐热", "辐射蜂巢能在常温下生存", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 辐射蜂巢耐热 { get; set; }
        [Option("动物无限繁殖", "动物不会拥挤和封闭", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 动物无限繁殖 { get; set; }
        [Option("无限繁殖上限", "单个房间内 动物加蛋的最大数量,避免驯化的动物指数爆炸", "功能性修改") ][JsonProperty] [Limit(1, 10000)] public  int 无限繁殖上限 { get; set; }

        [Option("获得所有好特质", "小人获得所有好特质", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 获得所有好特质 { get; set; }
        [Option("精准采集", "挖掘矿物掉落全部质量", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 精准采集 { get; set; }
        [Option("树鼠种植密度", "树鼠种植更快 密度更高", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 树鼠种植密度 { get; set; }
        [Option("无级变速", "游戏中速*2 快速*4", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 无级变速 { get; set; }
        [Option("无限拖把", "拖把无视液体质量", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 无限拖把 { get; set; }
        [Option("自动收获", "植物成熟后自动掉落", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 自动收获 { get; set; }
        [Option("强制建造", "按住SHIFT键强制部署建造蓝图", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 强制建造 { get; set; }
        [Option("超时空传送", "复制人可以瞬间到达可到达的地点", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 超时空传送 { get; set; }
        [Option("铁砂掌", "复制人拾起的东西在放下前不会发生换热", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 铁砂掌 { get; set; }
        [Option("祖宗人", "复制人无视外部极端环境", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 祖宗人 { get; set; }
        [Option("繁茂核心", "所有植物对温度与气压不再敏感", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 繁茂核心 { get; set; }
        [Option("一键设置长周期植物", "小吃豆小麦等长周期植物 生长周期-75% 果实-50%", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 一键设置长周期植物 { get; set; }
        [Option("物质大一统", "铌 导热质熔点+3000 陶瓷熔点+5000 隔热质导热率=0 熔融铀凝点物质为浓缩铀\n超级冷却剂熔点+8000℃ 比热容*30", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 物质大一统 { get; set; }
        [Option("白嫖怪", "复制人可以将地上的种子就地种下", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 白嫖怪 { get; set; }

        [Option("更大团物质", "修改物质堆叠最大质量", "功能性修改") ][Limit(25000f, 10000000f) ][JsonProperty] [DefaultValue(25000f)] public  float 更大团物质 { get; set; }
        [Option("中子湮灭", "辐射粒子撞击中子物质将会湮灭其质量", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 中子湮灭 { get; set; }
        [Option("长臂猿", "增加复制人的操作范围,默认0不增加范围", "功能性修改") ][JsonProperty] [Limit(0, 20)] [DefaultValue(0)] public  int 长臂猿 { get; set; }
        [Option("探云手", "允许复制人进行隔墙操作", "功能性修改") ][JsonProperty] [DefaultValue(false)]public  bool 探云手 { get; set; }



        [Option("修改泉属性", "控制以下三项是否生效", "属性控制") ][JsonProperty] [DefaultValue(false)]public bool 修改泉属性 { get; set; }
        [Option("喷发量", "", "属性控制", Format = "F2") ][Limit(0.0, 10.0) ][JsonProperty] public  float 喷发量 { get; set; }
        [Option("喷发期占比", "喷发时间/闲置时间", "属性控制", Format = "F2") ][Limit(0.0, 1.0) ][JsonProperty] public  float 喷发期占比 { get; set; }
        [Option("活跃期占比", "活跃时间/休眠时间", "属性控制", Format = "F2") ][Limit(0.0, 1.0) ][JsonProperty] public  float 活跃期占比 { get; set; }
        [Option("动物驯化速度", "加速/减速以任何形式对动物的驯化速度", "属性控制", Format = "F2") ][Limit(0.0, 10.0) ][JsonProperty] [DefaultValue(0f)] public  float 驯化速度 { get; set; }
        [Option("植物生长速度", "加速/减速任何植物的生长速度", "属性控制", Format = "F2") ][Limit(0.0, 10.0) ][JsonProperty] [DefaultValue(0f)] public  float 植物生长速度 { get; set; }
        [Option("动物产蛋速度", "加速被驯化的动物的产蛋速度", "属性控制", Format = "F2") ][Limit(0.0, 1000.0) ][JsonProperty] [DefaultValue(0f)] public  float 动物产蛋速度 { get; set; }
        [Option("种子掉落概率", "增加种子额外的掉落概率", "属性控制", Format = "F2") ][Limit(0, 100) ][JsonProperty] [DefaultValue(0)] public  int 种子掉落概率 { get; set; }
        [Option("孵化速度", "加速/减速任何蛋的孵化速度", "属性控制", Format = "F2") ][Limit(0.0, 1000.0) ][JsonProperty] [DefaultValue(0f)] public  float 孵化速度 { get; set; }
        [Option("物质导热系数", "实际导热率=原导热率*物质导热系数", "属性控制", Format = "F2") ][Limit(1.0, 1000.0) ][JsonProperty] [DefaultValue(1f)] public  float 物质导热系数 { get; set; }
        [Option("最低结块质量", "当结块质量小于最低结块质量时 实际结块质量为最低结块质量", "属性控制", Format = "F2") ][Limit(0.0, 10000.0) ][JsonProperty] [DefaultValue(0f)] public  float 最低结块质量 { get; set; }

        [Option("小人初始技能点", "控制小人在被打印或创建时获取的用于学习技能的技能点", "属性控制") ][Limit(0, 1000) ][JsonProperty] [DefaultValue(0)] public  int 小人初始技能点 { get; set; }
        [Option("小人初始天赋点", "允许你控制小人各项属性的初始点数", "属性控制") ][Limit(0, 100) ][JsonProperty] [DefaultValue(0)] public  int 小人初始天赋点 { get; set; }
        [Option("小人工作速度", "控制小人工作速度 数值越大 工作速度越快", "属性控制") ][Limit(0, 10) ][JsonProperty] [DefaultValue(0)] public  int 小人工作速度 { get; set; }

        public 大一统控制台UI()
        {

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties){
                var defaultValueAttr = prop.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttr != null && defaultValueAttr.Value != null){
                    prop.SetValue(this, defaultValueAttr.Value);
                    continue;
                }
            }
        }
    }
    class 控制台 : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {

            PUtil.InitLibrary(true);
            new POptions().RegisterOptions(this, typeof(大一统控制台UI));
            
            base.OnLoad(harmony);
            new AnyHarmony(harmony, base.assembly, 大一统控制台UI.Instance);


        }
    }
}
