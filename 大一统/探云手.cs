using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大一统{
    [AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.探云手) })]
    class 探云手
    {

        public static void ExecuteOnInit()
        {
                List<List<CellOffset>> InvertedStandardTableNew = OffsetGroups.InvertedStandardTable.Select(path => path.ToList()).ToList();
                List<List<CellOffset>> InvertedStandardTableWithCornersNew = OffsetGroups.InvertedStandardTableWithCorners.Select(path => path.ToList()).ToList();
                for (int i = 0; i < InvertedStandardTableNew.Count; i++)
                {
                    InvertedStandardTableNew[i].RemoveAll(n => n != InvertedStandardTableNew[i].First());
                }
                for (int i = 0; i < InvertedStandardTableWithCornersNew.Count; i++)
                {
                    InvertedStandardTableWithCornersNew[i].RemoveAll(n => n != InvertedStandardTableWithCornersNew[i].First());
                }
                OffsetGroups.InvertedStandardTable = InvertedStandardTableNew.Select(list => list.ToArray()).ToArray();
                OffsetGroups.InvertedStandardTableWithCorners = InvertedStandardTableWithCornersNew.Select(list => list.ToArray()).ToArray();

            
        }


    }
}
