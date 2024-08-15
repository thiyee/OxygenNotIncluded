using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using Newtonsoft.Json;

namespace 大一统{
    [JsonObject(MemberSerialization.OptIn)]
    [ConfigFile("大一统控制台.json", true, true)]
    [RestartRequired]
    public class 大一统控制台UI : SingletonOptions<大一统控制台UI>{
        
        [Option("更多泉与火山", "在创建新地图时生效 生成以下火山/泉 \n铀火山;钢火山;玻璃火山;超冷泉;石油泉;液磷泉;碳火山;蔗糖泉 ", "地图特质")] [JsonProperty] public bool 更多泉与火山 { get; set; }
        [Option("末日浩劫", "陨石群在星图上的移动速度-90%", "地图特质")] [JsonProperty] public bool 末日浩劫 { get; set; }
        [Option("虚空入侵", "在创建新地图时生效 随机替换初始生态以外的生态为太空暴露", "地图特质")] [JsonProperty] public bool 虚空入侵 { get; set; }
        [Option("入侵程度", "控制被替换的生态占总生态的多少 初始生态不会被替换", "地图特质", Format = "F2")] [Limit(0.0, 1.0)] [JsonProperty] public float 虚空入侵程度 { get; set; }
        [Option("辐星高照", "控制主世界太空辐射浓度", "地图特质", Format = "F2")] [Limit(1.0, 30.0)] [JsonProperty] public float 辐星高照 { get; set; }

        [Option("方块墙", "建造一个自然土块", "新的建筑")] [JsonProperty] public bool 方块墙 { get; set; }
        [Option("自动挖掘墙", "挖掘在其上的固体", "新的建筑")] [JsonProperty] public bool 自动挖掘墙 { get; set; }
        [Option("动物猎场", "自动处死进入的动物 可通过信号控制", "新的建筑")] [JsonProperty] public bool 动物猎场 { get; set; }
        [Option("中子湮灭发生器", "发射大量辐射粒子", "新的建筑")] [JsonProperty] public bool 中子湮灭发生器 { get; set; }

        [Option("按摩床恢复速度", "按摩床恢复速度30%/秒", "新建筑特性")] [JsonProperty] public bool 按摩床恢复速度 { get; set; }
        [Option("超视望远镜", "望远镜范围增加到15", "新建筑特性")] [JsonProperty] public bool 超视望远镜 { get; set; }
        [Option("改造储气液库", "储气库储液库能手动操作", "新建筑特性")] [JsonProperty] public bool 改造储气液库 { get; set; }
        [Option("改造液氢引擎", "液氢引擎能灌入燃料", "新建筑特性")] [JsonProperty] public bool 改造液氢引擎 { get; set; }
        [Option("高频电炉", "玻璃熔炉能融化任何固体物质", "新建筑特性")] [JsonProperty] public bool 高频电炉 { get; set; }
        [Option("冷光吸顶灯", "吸顶灯不再发热", "新建筑特性")] [JsonProperty] public bool 冷光吸顶灯 { get; set; }
        [Option("永不串气电解器", "氢气生成于[0,1] 氧气生成于[0,-1] 中间铺一层水就可隔开气体", "新建筑特性")] [JsonProperty] public bool 永不串气电解器 { get; set; }
        [Option("智能冰箱", "储存温度为-20℃ 更大容量", "新建筑特性")] [JsonProperty] public bool 智能冰箱 { get; set; }
        [Option("最后的基地", "打印舱提供基础维生设备和电力设施 对新手友好", "新建筑特性")] [JsonProperty] public bool 最后的基地 { get; set; }
        [Option("电线穿墙", "粗电线/导线穿墙", "新建筑特性")] [JsonProperty] public bool 电线穿墙 { get; set; }
        [Option("光电效应", "太阳能板发电量增强 没有上限 告别CPU发电", "新建筑特性", Format = "F0")] [Limit(1, 10)] [JsonProperty] public float 光电效应 { get; set; }
        [Option("储物箱容量", "储物箱容量(吨)", "新建筑特性", Format = "F0")] [Limit(20,2000)] [JsonProperty] public float 储物箱容量 { get; set; }
        [Option("蒸汽时代", "蒸汽机能吸取速度*5 过热温度=200℃ 发热降低90%", "新建筑特性")] [JsonProperty] public bool 蒸汽时代 { get; set; }
        [Option("强制隔热", "隔热砖 液体/气体管道 不发生热交换", "新建筑特性")] [JsonProperty] public bool 强制隔热 { get; set; }
        
        //[Option("捕捉飞行动物和鱼", "", "功能性修改")] [JsonProperty] public bool 捕捉飞行动物和鱼 { get; set; }
        [Option("动物更耐高低温", "比原来更耐热/耐寒", "功能性修改")] [JsonProperty] public bool 动物更耐高低温 { get; set; }
        [Option("辐射蜂巢耐热", "辐射蜂巢能在常温下生存", "功能性修改")] [JsonProperty] public bool 辐射蜂巢耐热 { get; set; }
        [Option("动物无限繁殖", "动物不会拥挤和封闭", "功能性修改")] [JsonProperty] public bool 动物无限繁殖 { get; set; }
        [Option("获得所有好特质", "小人获得所有好特质", "功能性修改")] [JsonProperty] public bool 获得所有好特质 { get; set; }
        [Option("精准采集", "挖掘矿物掉落全部质量", "功能性修改")] [JsonProperty] public bool 精准采集 { get; set; }
        [Option("树鼠种植密度", "树鼠种植更快 密度更高", "功能性修改")] [JsonProperty] public bool 树鼠种植密度 { get; set; }
        [Option("无级变速", "游戏中速*2 快速*4", "功能性修改")] [JsonProperty] public bool 无级变速 { get; set; }
        [Option("无限拖把", "拖把无视液体质量", "功能性修改")] [JsonProperty] public bool 无限拖把 { get; set; }
        [Option("自动收获", "植物成熟后自动掉落", "功能性修改")] [JsonProperty] public bool 自动收获 { get; set; }
        [Option("强制建造", "按住SHIFT键强制部署建造蓝图", "功能性修改")] [JsonProperty] public bool 强制建造 { get; set; }
        [Option("超时空传送", "复制人可以瞬间到达可到达的地点", "功能性修改")] [JsonProperty] public bool 超时空传送 { get; set; }
        [Option("铁砂掌", "复制人拾起的东西在放下前不会发生换热", "功能性修改")] [JsonProperty] public bool 铁砂掌 { get; set; }
        [Option("祖宗人", "复制人无视外部极端环境", "功能性修改")] [JsonProperty] public bool 祖宗人 { get; set; }



        [Option("修改泉属性", "控制以下三项是否生效", "属性控制")] [JsonProperty] public bool 修改泉属性 { get; set; }
        [Option("喷发量", "", "属性控制", Format = "F2")] [Limit(0.0, 10.0)] [JsonProperty] public float 喷发量 { get; set; }
        [Option("喷发期占比", "喷发时间/闲置时间", "属性控制", Format = "F2")] [Limit(0.0, 1.0)] [JsonProperty] public float 喷发期占比 { get; set; }
        [Option("活跃期占比", "活跃时间/休眠时间", "属性控制", Format = "F2")] [Limit(0.0, 1.0)] [JsonProperty] public float 活跃期占比 { get; set; }
        [Option("动物驯化速度", "加速/减速以任何形式对动物的驯化速度", "属性控制", Format = "F2")] [Limit(0.0, 10.0)] [JsonProperty] public float 驯化速度 { get; set; }
        [Option("植物生长速度", "加速/减速任何植物的生长速度", "属性控制", Format = "F2")] [Limit(0.0, 10.0)] [JsonProperty] public float 植物生长速度 { get; set; }
        [Option("动物产蛋速度", "加速被驯化的动物的产蛋速度", "属性控制", Format = "F2")] [Limit(0.0, 1000.0)] [JsonProperty] public float 动物产蛋速度 { get; set; }
        [Option("孵化速度", "加速/减速任何蛋的孵化速度", "属性控制", Format = "F2")] [Limit(0.0, 1000.0)] [JsonProperty] public float 孵化速度 { get; set; }
        [Option("一键设置长周期植物", "小吃豆小麦等长周期植物 生长周期-75% 果实-50%", "属性控制")] [JsonProperty] public bool 一键设置长周期植物 { get; set; }
        [Option("物质导热系数", "实际导热率=原导热率*物质导热系数", "属性控制", Format = "F2")] [Limit(1.0, 1000.0)] [JsonProperty] public float 物质导热系数 { get; set; }
        [Option("最低结块质量", "当结块质量小于最低结块质量时 实际结块质量为最低结块质量", "属性控制", Format = "F2")] [Limit(0.0, 10000.0)] [JsonProperty] public float 最低结块质量 { get; set; }
        [Option("物质大一统", "铌 导热质熔点+3000 陶瓷熔点+5000 隔热质导热率=0 熔融铀凝点物质为浓缩铀\n超级冷却剂熔点+8000℃ 比热容*30", "属性控制")] [JsonProperty] public bool 物质大一统 { get; set; }
        [Option("繁茂核心", "所有植物对温度与气压不再敏感", "属性控制")] [JsonProperty] public bool 繁茂核心 { get; set; }

        [Option("小人初始技能点", "控制小人在被打印或创建时获取的用于学习技能的技能点", "属性控制")] [Limit(0, 1000)] [JsonProperty] public int 小人初始技能点 { get; set; }
        [Option("小人初始天赋点", "允许你控制小人各项属性的初始点数", "属性控制")] [Limit(0, 100)] [JsonProperty] public int 小人初始天赋点 { get; set; }
        //[Option("小人工作范围", "控制小人挖掘 建造 操作等范围 \n该属性在原有工作距离上增加", "属性控制")] [Limit(0, 10)] [JsonProperty] public int 小人工作范围 { get; set; }
        [Option("小人工作速度", "控制小人工作速度 数值越大 工作速度越快", "属性控制")] [Limit(0, 10)] [JsonProperty] public int 小人工作速度 { get; set; }

        //[Option("真·荧光棒", "开启 使生成的小人带有荧光棒属性", "属性控制")] [JsonProperty] public bool 真荧光棒 { get; set; }
        //[Option("荧光棒强度", "调整荧光棒辐射强度 (真·荧光棒启用时生效)", "属性控制")] [Limit(1, 1000000)] [JsonProperty] public float 荧光棒强度 { get; set; }
        //[Option("荧光棒范围", "调整荧光棒辐射范围 (真·荧光棒启用时生效)", "属性控制")] [Limit(3, short.MaxValue)] [JsonProperty] public int 荧光棒范围 { get; set; }

        public 大一统控制台UI()
        {
            this.按摩床恢复速度 = false;
            //this.捕捉飞行动物和鱼 = false;
            this.超视望远镜 = false;
            this.动物更耐高低温 = false;
            this.动物无限繁殖 = false;
            this.方块墙 = false;
            this.自动挖掘墙 = false;
            this.动物猎场 = false;
            this.中子湮灭发生器 = false;
            this.辐射蜂巢耐热 = false;
            this.改造储气液库 = false;
            this.改造液氢引擎 = false;
            this.高频电炉 = false;
            this.更多泉与火山 = false;
            this.获得所有好特质 = false;
            this.精准采集 = false;
            this.冷光吸顶灯 = false;
            this.末日浩劫 = false;
            this.树鼠种植密度 = false;
            this.无级变速 = false;
            this.无限拖把 = false;
            this.修改泉属性 = false;
            this.喷发量 = 1f;
            this.喷发期占比=0.5f;
            this.活跃期占比 = 0.5f;
            this.虚空入侵 = false;
            this.虚空入侵程度 = 1f;
            this.永不串气电解器 = false;
            this.智能冰箱 = false;
            this.自动收获 = false;
            this.强制建造= false;
            this.超时空传送 = false;
            this.最后的基地 = false;
            this.驯化速度 = 1;
            this.植物生长速度 = 1;
            this.动物产蛋速度 = 0;
            this.孵化速度 = 1;
            this.一键设置长周期植物 = false;
            this.电线穿墙 = false;
            this.光电效应 = 1;
            this.储物箱容量 = 20;
            this.蒸汽时代 = false;
            this.强制隔热 = false;
            this.辐星高照 = 1;
            this.物质导热系数 = 1f;
            this.最低结块质量 = 500f;
            this.物质大一统 = false;
            this.繁茂核心 = false;
            this.小人初始技能点 = 0;
            this.小人初始天赋点 = 0;
            //this.小人工作范围 = 3;
            this.小人工作速度 = 1;
            this.铁砂掌 = false;
            this.祖宗人 = false;

            //this.真荧光棒 = false;
            //this.荧光棒强度 = 1;
            //this.荧光棒范围 = 3;
        }
    }
        class 控制台 : UserMod2{
        public override void OnLoad(Harmony harmony)
        {
            PUtil.InitLibrary(true);
            new POptions().RegisterOptions(this, typeof(大一统控制台UI));
            base.OnLoad(harmony);

            强制建造.强制建造 强制建造ins = new 强制建造.强制建造();
            //真荧光棒.真荧光棒 荧光棒属性ins = new 真荧光棒.真荧光棒();
            动物更耐高低温.动物体质增强 动物体质增强ins = new 动物更耐高低温.动物体质增强();
            效果修改.效果修改 效果修改ins = new 效果修改.效果修改();
        }
    }
}
