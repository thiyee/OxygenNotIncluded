using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Test
{
    class Program
    {
		public class FieldAccess
		{
			FieldInfo info;
			object Instance;
			public FieldAccess(object Instance, string field)
			{
				this.Instance = Instance;
				info = Instance.GetType().GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			}
			public object Get()
			{
				return info.GetValue(Instance);
			}
			public void Set(object value)
			{
				info.SetValue(Instance, value);
			}
			public static void Initialize(object This, object Instance)
			{
				FieldInfo[] fields = This.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(x => x.FieldType == typeof(FieldAccess)).ToArray();
				foreach (FieldInfo field in fields)
				{
					field.SetValue(This, new FieldAccess(Instance, field.Name));
				}
			}
		}
		public class FuncAccess
		{
			MethodInfo info;
			object Instance;
			public FuncAccess(object Instance, string func)
			{
				this.Instance = Instance;
				info = Instance.GetType().GetMethod(func, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			}
			public object Invoke(object[] Parameters)
			{
				ParameterInfo[] paraminfos=info.GetParameters();

				return info.Invoke(Instance, Parameters);
			}
			public object Invoke()
			{
				return info.Invoke(Instance, null);
			}
			public static void Initialize(object This, object Instance)
			{
				FieldInfo[] funcs = This.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(x => x.FieldType == typeof(FuncAccess)).ToArray();
				foreach (FieldInfo func in funcs)
				{
					func.SetValue(This, new FuncAccess(Instance, func.Name));
				}
			}
		}
		public class NavigatorAccess
		{
			public Navigator navigator;
			public FieldAccess reservedCell;
			public FieldAccess tactic;
			public FuncAccess ValidatePath;
			public FuncAccess SetReservedCell;
			public FuncAccess ClearReservedCell;
			public void SetReservedCellProxy(int cell)
			{
				this.SetReservedCell.Invoke(new object[] { cell });
			}
			public void ClearReservedCellProxy()
			{
				this.ClearReservedCell.Invoke();
			}
			public bool ValidatePathProxy(ref PathFinder.Path path, out bool atNextNode)
			{
				bool ret;
				object[] Parameter = { path, null };
				ret=(bool)this.SetReservedCell.Invoke(Parameter);
				path = (PathFinder.Path)Parameter[0];
				atNextNode= (bool)Parameter[1];
				return ret;
			}

			public NavigatorAccess(Navigator navigator)
			{
				this.navigator = navigator;
				FieldAccess.Initialize(this, navigator);
				FuncAccess.Initialize(this, navigator);
			}

		}

		public class TransitionDriverAccess
		{
			public TransitionDriver transitionDriver;

			public FieldAccess navigator;
			public FieldAccess interruptOverrideStack;
			public FieldAccess transition;
			public FieldAccess isComplete;
			public FieldAccess isCompleteCB;
			public FieldAccess brain;
			public FieldAccess targetPos;

			public TransitionDriverAccess(TransitionDriver transitionDriver)
			{
				this.transitionDriver = transitionDriver;
				FieldAccess.Initialize(this, transitionDriver);
				FuncAccess.Initialize(this, transitionDriver);
			}

		}

		static void Main(string[] args)
        {
			MethodInfo[] BuildResultPath = typeof(ScaldingMonitor.Def).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

			FieldInfo[] info = typeof(TransitionDriver).GetFields(BindingFlags.Public | BindingFlags.Instance );

			TransitionDriverAccess navigatorAccess = new TransitionDriverAccess(new TransitionDriver(new Navigator()));
			object o=navigatorAccess.isCompleteCB.Get();
			return;
        }
    }
}
