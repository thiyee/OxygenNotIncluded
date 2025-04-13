using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            float f;
            float.TryParse("∞", out f);
            f = float.PositiveInfinity;
            return;
        }
    }
}
