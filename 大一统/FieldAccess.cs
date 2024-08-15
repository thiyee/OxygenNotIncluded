using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
			foreach (FieldInfo field in fields){
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
		public object Invoke(object[] Parameters){
			return info.Invoke(Instance, Parameters);
		}		
		public object Invoke(){
			return info.Invoke(Instance,null);
		}
		public static void Initialize(object This, object Instance){
			FieldInfo[] funcs = This.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(x => x.FieldType == typeof(FuncAccess)).ToArray();
			foreach (FieldInfo func in funcs){
				func.SetValue(This, new FuncAccess(Instance, func.Name));
			}
		}
	}

