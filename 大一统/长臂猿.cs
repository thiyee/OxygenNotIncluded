using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.长臂猿) })]
    class 长臂猿
    {
        public static void ExternRanges(List<List<CellOffset>> OffsetTable, int Range)
        {
            if (Range > 0)
            {
                for (int r = 0; r < Range; r++)
                {
                    CellOffset[] expansions = new CellOffset[8];
                    List<List<CellOffset>> OffsetTableSnapshot = new List<List<CellOffset>>(OffsetTable);
                    for (int i = 0; i < OffsetTableSnapshot.Count; i++)
                    {
                        var first = OffsetTableSnapshot[i].First();
                        expansions[0] = new CellOffset(first.x + 1, first.y);
                        expansions[1] = new CellOffset(first.x - 1, first.y);
                        expansions[2] = new CellOffset(first.x, first.y - 1);
                        expansions[3] = new CellOffset(first.x, first.y + 1);

                        expansions[4] = new CellOffset(first.x+1, first.y + 1);
                        expansions[5] = new CellOffset(first.x+1, first.y - 1);
                        expansions[6] = new CellOffset(first.x-1, first.y + 1);
                        expansions[7] = new CellOffset(first.x-1, first.y - 1);
                        foreach (var e in expansions)
                        {
                            if (!OffsetTable.Any(p => p.First() == e))
                            {
                                var newpath = new List<CellOffset>(OffsetTable[i]);
                                newpath.Insert(0, (CellOffset)e);
                                OffsetTable.Add(newpath);
                            }
                        }
                    }
                }
            }

        }
        public static void LogTable(List<List<CellOffset>> OffsetTable)
        {
            StringBuilder b = new StringBuilder();
            foreach (var l in OffsetTable)
            {
                b.Append("{");
                foreach (var c in l)
                {
                    b.Append("\t" + c.ToString());
                }
                b.Append("}\n");
            }
            Console.WriteLine(b);
        }
        public static void ExecuteOnInit()
        {
            int ExternRange = 大一统.大一统控制台UI.Instance.长臂猿;

            if (ExternRange > 0)
            {
                List<List<CellOffset>> InvertedStandardTableNew = OffsetGroups.InvertedStandardTable.Select(path => path.ToList()).ToList();
                List<List<CellOffset>> InvertedStandardTableWithCornersNew = OffsetGroups.InvertedStandardTableWithCorners.Select(path => path.ToList()).ToList();

                ExternRanges(InvertedStandardTableNew, ExternRange);
                //LogTable(InvertedStandardTableNew);
                ExternRanges(InvertedStandardTableWithCornersNew, ExternRange);
                //LogTable(InvertedStandardTableWithCornersNew);

                OffsetGroups.InvertedStandardTable = InvertedStandardTableNew.Select(c => c.ToArray()).ToArray();
                OffsetGroups.InvertedStandardTableWithCorners = InvertedStandardTableWithCornersNew.Select(c => c.ToArray()).ToArray();

                if (大一统.大一统控制台UI.Instance.探云手)
                {
                    探云手.ExecuteOnInit();
                }

            }
        }
    }
}
