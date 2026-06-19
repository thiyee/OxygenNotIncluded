using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace test
{
    class Program
    {
        static string[] aaaa=new string[]{"asdasd"};
        public static string[] asd()
        {
            return aaaa;
        }
        public static void Main(string[] args)
        {
            string[] AllDlcs = new string[] { DlcManager.EXPANSION1_ID, DlcManager.DLC2_ID, DlcManager.DLC3_ID, DlcManager.DLC4_ID };
            var items = typeof(IEntityConfig).Assembly.GetTypes();
            return;
            //var items = typeof(IEntityConfig).Assembly.GetTypes()
            //    .Where(t =>
            //    {
            //        // 筛选：是实体配置、非抽象类
            //        if (typeof(IEntityConfig).IsAssignableFrom(t) && !t.IsAbstract)
            //        {
            //            // 获取DLC判断方法 GetRequiredDlcIds
            //            var method = t.GetMethod("GetRequiredDlcIds");
            //            if (method != null)
            //            {
            //                List<string> dlcs = new List<string>();
            //                dlcs=method.Invoke(null, new object[] { }) as List<string>;
            //                //// 读取方法IL指令，解析该物品依赖哪些DLC
            //                //List<CodeInstruction> ins = PatchProcessor.GetOriginalInstructions(method);
            //                //// 查找静态字段加载指令（部分DLC存在数组常量字段）
            //                //CodeInstruction ldsfld = ins.FirstOrDefault(i => i.opcode.Name == "ldsfld");
            //                //if (ldsfld != null)
            //                //{
            //                //    var field = ldsfld.operand as FieldInfo;
            //                //    // 读取字段里存的DLC ID数组
            //                //    dlcs = (field.GetValue(null) as string[]).ToList();
            //                //}
            //                //else
            //                //{
            //                //    // 没有静态字段，直接读取IL里硬编码的DLC字符串
            //                //    List<CodeInstruction> ldstrs = ins.Where(i => i.opcode.Name == "ldstr").ToList();
            //                //    dlcs = ldstrs.Select(ldstr => (string)ldstr.operand).ToList();
            //                //}
            //                //// 过滤掉非官方4个DLC的无效ID
            //                //dlcs.RemoveAll(n => !AllDlcs.Contains(n));
            //                //// 判断玩家是否拥有该物品需要的全部DLC，没有则跳过该物品
            //                return DlcManager.IsAllContentSubscribed(dlcs);
            //            }
            //            // 无DLC限制的物品直接保留
            //            return true;
            //        }
            //        // 不满足实体配置条件，过滤掉
            //        return false;
            //    })
            //    // 从配置类取出静态常量：SEED_ID(种子) / EGG_ID(生物蛋)
            //    .Select(t => t.GetField("SEED_ID", BindingFlags.Public | BindingFlags.Static) ??
            //                t.GetField("EGG_ID", BindingFlags.Public | BindingFlags.Static))
            //    // 过滤掉找不到ID字段的类
            //    .Where(field => field != null)
            //    // 封装兑换物品信息元组：物品Tag、分类名称、消耗铌数量、产出物品数量
            //    .Select(field =>
            //    {
            //        string id = field.GetValue(null) as string;
            //        string name = string.Empty;

            //        // ID后缀判断分类
            //        if (id.EndsWith("Egg"))
            //        {
            //            name = "动物蛋";
            //        }
            //        else if (id.EndsWith("Seed"))
            //        {
            //            name = "种子";
            //        }

            //        return (Tag: new Tag(id), Name: name, IngredientAmount: 1000f, ResultAmount: 1f);
            //    }).ToList();


            //foreach (var i in items)
            //{
            //    Console.WriteLine($"{i.Tag} {i.Name}");
            //}
            return;
            
        }
    }
}
