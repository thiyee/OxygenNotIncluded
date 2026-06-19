using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] AllDlcs = new string[] { DlcManager.EXPANSION1_ID, DlcManager.DLC2_ID, DlcManager.DLC3_ID, DlcManager.DLC4_ID };
            var items = typeof(IEntityConfig).Assembly.GetTypes()
                .Where(t =>
                {
                    // 筛选：是实体配置、非抽象类
                    if (typeof(IEntityConfig).IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        var obj = Activator.CreateInstance(t);
                        if (obj is IEntityConfig config)
                        {

                            List<string> dlcs = new List<string>();
                            if (obj is IHasDlcRestrictions dlcRestrictions)
                            {
                                dlcs.AddRange(dlcRestrictions.GetRequiredDlcIds());
                            }
                            else
                            {
                                dlcs.AddRange(config.GetDlcIds() ?? new string[] { });
                            }
                            Console.WriteLine($"{t.Name} dlcs:{string.Join(" ", dlcs)}");

                            return true;
                        }
                    }
                    // 不满足实体配置条件，过滤掉
                    return false;
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


            foreach (var i in items)
            {
                Console.WriteLine($"{i.Tag} {i.Name}");
            }
            return;
        }
    }
}
