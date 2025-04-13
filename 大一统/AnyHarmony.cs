using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AnyHarmonyPatch : Attribute
{
    public Type TargetType { get; }
    public string MethodName { get; }

    // 新增补丁方法指定属性
    public string Prefix { get; set; }
    public string Postfix { get; set; }
    public string Transpiler { get; set; }
    public string Finalizer { get; set; }
    public string Replace { get; set; }
    public string ExecuteOnInit { get; set; }
    public Type[] ArgumentTypes { get; set; }


    //当AnyHarmony传递Control实例时 允许指定一个类的某字段或属性 该字段或属性应具有[DefaultValue(value)]特性 当该字段或属性值等于DefaultValue值时 则不应用此patch 
    public string[] ControlName { get; set; }

    public AnyHarmonyPatch(Type targetType, string methodName, string Prefix = null, string Postfix = null, string Transpiler = null, string Finalizer = null, string Replace = null, string ExecuteOnInit = null, Type[] ArgumentTypes=null,string[] ControlName=null)
    {
        TargetType = targetType;
        MethodName = methodName;
        this.Prefix = Prefix;
        this.Postfix = Postfix;
        this.Transpiler = Transpiler;
        this.Finalizer = Finalizer;
        this.Replace = Replace;
        this.ExecuteOnInit = ExecuteOnInit;
        this.ArgumentTypes = ArgumentTypes;
        this.ControlName = ControlName;
    }
}



public class AnyHarmony : IDisposable
{
    private readonly Harmony _harmony;
    private readonly Assembly _assembly;
    private readonly object ControlInstance;

    public AnyHarmony(Harmony harmony, Assembly assembly,object ControlInstance)
    {
        Debugger.Break();
        _harmony = harmony;
        _assembly = assembly;
        this.ControlInstance = ControlInstance;
        try
        {
            _harmony = harmony;
            _assembly = assembly;
            PatchAll();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize AnyHarmony: {ex}");
            throw;
        }
    }
    private MethodType GetMethodType(MemberInfo memberInfo)
    {
        switch (memberInfo)
        {
            case MethodInfo method:
                if (method.IsSpecialName)
                {
                    if (method.Name.StartsWith("get_"))
                        return MethodType.Getter;
                    else if (method.Name.StartsWith("set_"))
                        return MethodType.Setter;
                    else
                        return MethodType.Normal; // 其他特殊方法（如事件add/remove）
                }
                else
                {
                    return MethodType.Normal;
                }
                break;

            case ConstructorInfo constructor:
                return constructor.IsStatic
                    ? MethodType.StaticConstructor
                    : MethodType.Constructor;
                break;

            default:
                throw new NotSupportedException($"不支持的方法类型: {memberInfo}");
        }
    }
    private Type[] GetArgumentTypes(MemberInfo memberInfo)
    {
        if(memberInfo is MethodBase m)
        {
            return m.GetParameters().Select(n => n.ParameterType).ToArray();
        }
        throw new NotSupportedException($"不支持的类型: {memberInfo.MemberType}");
    }
    private bool FuncIsArgumentTypes(MemberInfo memberInfo,Type[] ArgumentTypes)
    {
        return (memberInfo is MethodBase method && method.GetParameters().Select(p => p.ParameterType).ToArray().SequenceEqual(ArgumentTypes));

    }
    public void PatchAll()
    {
        foreach (var type in _assembly.GetTypes())
        {
            var AnyHarmonyPatchs = type.GetCustomAttributes<AnyHarmonyPatch>(inherit: false);
            if (!AnyHarmonyPatchs.Any()) continue;

            foreach (var @AnyHarmonyPatch in AnyHarmonyPatchs)
            {
                if (ControlInstance != null&& @AnyHarmonyPatch.ControlName!=null)
                {
                    bool Enabled = false;
                    foreach(var Control in @AnyHarmonyPatch.ControlName.Select(name=> ControlInstance.GetType().GetProperty(name))){
                        var DefaultValue = Control.GetCustomAttribute<DefaultValueAttribute>();
                        if (Control != null)
                        {   //任何一个控制启用 都执行patch
                            Enabled |= !Control.GetValue(ControlInstance).Equals(DefaultValue.Value);
                        }
                        else
                        {
                            throw new Exception("Not found {@AnyHarmonyPatch.ControlName} Property");
                        }
                    }
                    if (!Enabled) continue;
                }


                var prefix = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.Prefix) ?? GetNamedHarmonyMethod(type, "Prefix");
                var postfix = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.Postfix) ?? GetNamedHarmonyMethod(type, "Postfix");
                var transpiler = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.Transpiler) ?? GetNamedHarmonyMethod(type, "Transpiler");
                var finalizer = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.Finalizer) ?? GetNamedHarmonyMethod(type, "Finalizer");
                var replace = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.Replace) ?? GetNamedHarmonyMethod(type, "Replace");
                var ExecuteOnInit = GetNamedHarmonyMethod(type, @AnyHarmonyPatch.ExecuteOnInit);

                if (ExecuteOnInit != null)
                {
                    ExecuteOnInit.method.Invoke(null, null);
                    continue;
                }
                //ApplyPatch(@AnyHarmonyPatch.TargetType, @AnyHarmonyPatch.MethodName, prefix, postfix, transpiler, finalizer, replace);
                //continue;

                var PatchedMethods = @AnyHarmonyPatch.TargetType.GetMembers(AccessTools.all).Where(m=>m.Name== @AnyHarmonyPatch.MethodName).ToArray();
                if (PatchedMethods.Count() <= 0)
                {
                    throw new MissingMethodException($"Method {@AnyHarmonyPatch.MethodName} not found in {@AnyHarmonyPatch.TargetType.FullName}");
                }
                FieldInfo PatchClassProcessor_auxilaryMethods = typeof(PatchClassProcessor).GetField("auxilaryMethods", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo PatchClassProcessor_containerAttributes = typeof(PatchClassProcessor).GetField("containerAttributes", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo PatchClassProcessor_patchMethods = typeof(PatchClassProcessor).GetField("patchMethods", BindingFlags.NonPublic | BindingFlags.Instance);

                var AttributePatchType = typeof(HarmonyLib.Harmony).Assembly.GetType("HarmonyLib.AttributePatch");
                Func<dynamic> AttributePatch_Constructor = () => AttributePatchType.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).OfType<ConstructorInfo>().FirstOrDefault().Invoke(null);

                FieldInfo AttributePatch_info = AttributePatchType.GetField("info", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo AttributePatch_type = AttributePatchType.GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var PatchedMethod in PatchedMethods)
                {
                    if (@AnyHarmonyPatch.ArgumentTypes != null)
                    {
                        if (!FuncIsArgumentTypes(PatchedMethod, @AnyHarmonyPatch.ArgumentTypes))
                            continue;
                    }
                    PatchClassProcessor patchClassProcessor = new PatchClassProcessor(_harmony, type);
                    if (replace != null)
                    {
                        Memory.DetourMethod(PatchedMethod as MethodBase, replace.method);
                        Console.WriteLine($"Patched: {PatchedMethod.DeclaringType?.Name}.{PatchedMethod.Name}=>{replace.declaringType?.Name}.{replace.method?.Name}");

                    }
                    else if ((prefix?? postfix?? transpiler?? finalizer)!=null)
                    {
                        var patchMethods = Activator.CreateInstance(PatchClassProcessor_patchMethods.FieldType) as IList;
                        object AttributePatch;
                        if (prefix != null)
                        {
                            AttributePatch = AttributePatch_Constructor();
                            AttributePatch_info.SetValue(AttributePatch, prefix);
                            prefix.declaringType = PatchedMethod.DeclaringType;
                            prefix.methodName = PatchedMethod.Name;
                            prefix.methodType = GetMethodType(PatchedMethod);
                            prefix.argumentTypes = GetArgumentTypes(PatchedMethod);
                            AttributePatch_type.SetValue(AttributePatch, HarmonyPatchType.Prefix);
                            patchMethods.Add(AttributePatch);
                        }
                        if (postfix != null)
                        {
                            AttributePatch = AttributePatch_Constructor();
                            AttributePatch_info.SetValue(AttributePatch, postfix);
                            postfix.declaringType = PatchedMethod.DeclaringType;
                            postfix.methodName = PatchedMethod.Name;
                            postfix.methodType = GetMethodType(PatchedMethod);
                            postfix.argumentTypes = GetArgumentTypes(PatchedMethod);
                            AttributePatch_type.SetValue(AttributePatch, HarmonyPatchType.Postfix);
                            patchMethods.Add(AttributePatch);
                        }
                        if (transpiler != null)
                        {
                            AttributePatch = AttributePatch_Constructor();
                            AttributePatch_info.SetValue(AttributePatch, transpiler);
                            transpiler.declaringType = PatchedMethod.DeclaringType;
                            transpiler.methodName = PatchedMethod.Name;
                            transpiler.methodType = GetMethodType(PatchedMethod);
                            transpiler.argumentTypes = GetArgumentTypes(PatchedMethod);
                            AttributePatch_type.SetValue(AttributePatch, HarmonyPatchType.Transpiler);
                            patchMethods.Add(AttributePatch);
                        }
                        if (finalizer != null)
                        {
                            AttributePatch = AttributePatch_Constructor();
                            AttributePatch_info.SetValue(AttributePatch, finalizer);
                            finalizer.declaringType = PatchedMethod.DeclaringType;
                            finalizer.methodName = PatchedMethod.Name;
                            finalizer.methodType = GetMethodType(PatchedMethod);
                            finalizer.argumentTypes = GetArgumentTypes(PatchedMethod);
                            AttributePatch_type.SetValue(AttributePatch, HarmonyPatchType.Finalizer);
                            patchMethods.Add(AttributePatch);
                        }
                        PatchClassProcessor_patchMethods.SetValue(patchClassProcessor, patchMethods);

                        var containerAttributes = new HarmonyMethod();
                        containerAttributes.method = null;
                        containerAttributes.declaringType = PatchedMethod.DeclaringType;
                        containerAttributes.methodName = PatchedMethod.Name;
                        containerAttributes.methodType = GetMethodType(PatchedMethod);
                        PatchClassProcessor_containerAttributes.SetValue(patchClassProcessor, containerAttributes);
                        PatchClassProcessor_auxilaryMethods.SetValue(patchClassProcessor, new Dictionary<Type, MethodInfo>());
                        try
                        {
                            patchClassProcessor.Patch();
                            var PatchMethod = prefix ?? postfix ?? transpiler ?? finalizer;
                            foreach(var p in patchMethods)
                            {
                                Console.WriteLine($"Patched: {PatchedMethod.DeclaringType?.Name}.{PatchedMethod.Name}=>[{AttributePatch_type.GetValue(p)}|{((HarmonyMethod)AttributePatch_info.GetValue(p)).methodType}]:{PatchMethod.method?.DeclaringType.Name}.{PatchMethod.method?.Name}");

                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Harmony Error] Failed to patch {@AnyHarmonyPatch.TargetType.Name}.{@AnyHarmonyPatch.MethodName} with {ex}");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"至少需要提供一个补丁方法（prefix/postfix/transpiler/finalizer/reverse）for {@AnyHarmonyPatch.TargetType.Name}.{@AnyHarmonyPatch.MethodName}");
                    }


                }
            }
        }
    }

    private HarmonyMethod GetNamedHarmonyMethod(Type type, string methodName)
    {
        if (string.IsNullOrEmpty(methodName)) return null;
        var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == null) return null;

        return new HarmonyMethod(method);
    }

    private void ApplyPatch(
        Type targetType,
        string methodName,
        HarmonyMethod prefix,
        HarmonyMethod postfix,
        HarmonyMethod transpiler,
        HarmonyMethod finalizer,
        HarmonyMethod replace
        )
    {
        try
        {

            MethodBase[] methods = targetType.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
             .OfType<MethodBase>()
             .Where(m => m.Name == methodName)
             .ToArray();
            if (methods.Count() <= 0)
            {
                throw new MissingMethodException($"Method {methodName} not found in {targetType.FullName}");
            }



            foreach (var method in methods)
            {
                // 收集非空的补丁方法信息
                var PatchMethods = new List<string>();

                if (prefix != null)
                    PatchMethods.Add($"prefix:{prefix.method.DeclaringType?.Name}.{prefix.method.Name}");

                if (postfix != null)
                    PatchMethods.Add($"postfix:{postfix.method.DeclaringType?.Name}.{postfix.method.Name}");

                if (transpiler != null)
                    PatchMethods.Add($"transpiler:{transpiler.method.DeclaringType?.Name}.{transpiler.method.Name}");

                if (finalizer != null)
                    PatchMethods.Add($"finalizer:{finalizer.method.DeclaringType?.Name}.{finalizer.method.Name}");

                if (replace != null)
                    PatchMethods.Add($"reverse:{replace.method.DeclaringType?.Name}.{replace.method.Name}");

                // 检查是否有至少一个补丁方法
                if (PatchMethods.Count == 0)
                    throw new InvalidOperationException($"至少需要提供一个补丁方法（prefix/postfix/transpiler/finalizer/reverse）for {method.DeclaringType?.Name}.{method.Name}");
                if (replace != null)
                {
                    if (method == null)
                    {
                        throw new Exception("Patche method not found");

                    }
                    Memory.DetourMethod(method, replace.method);
                }
                else
                {
                    _harmony.Patch(
                        original: method,
                        prefix: prefix,
                        postfix: postfix,
                        transpiler: transpiler,
                        finalizer: finalizer
                    );
                }


                // 构建输出信息
                var output = new StringBuilder();
                output.Append($"Patched: {method.DeclaringType?.Name}.{method.Name}");
                output.Append($" | With: {string.Join(", ", PatchMethods)}");

                Console.WriteLine(output.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Harmony Error] Failed to patch {targetType.Name}.{methodName}: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _harmony?.UnpatchAll();
    }
}