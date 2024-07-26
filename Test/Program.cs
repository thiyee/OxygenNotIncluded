using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Database;
using Klei;
using Klei.AI;
using ProcGen;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MemberInfo[] memberInfo = typeof(GlowStick.StatesInstance).GetMembers().Where(m => m.Name == ".ctor").ToArray();
            MemberInfo member = memberInfo[0];
            ConstructorInfo ctor = member as ConstructorInfo;

            return;
        }
    }
}
