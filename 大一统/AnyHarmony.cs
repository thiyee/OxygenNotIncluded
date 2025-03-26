using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AnyHarmonyPatch : Attribute
{
    public Type TargetType { get; }
    public string MethodName { get; }

    // 新增补丁方法指定属性
    public string Prefix { get; set; }
    public string Postfix { get; set; }
    public string Transpiler { get; set; }
    public string Finalizer { get; set; }

    public AnyHarmonyPatch(Type targetType, string methodName, string Prefix= null, string Postfix = null, string Transpiler = null, string Finalizer = null)
    {
        TargetType = targetType;
        MethodName = methodName;
        this.Prefix = Prefix;
        this.Postfix = Postfix;
        this.Transpiler = Transpiler;
        this.Finalizer = Finalizer;
    }
}

public sealed class AnyHarmony : IDisposable
{
    private readonly Harmony _harmony;
    private readonly Assembly _assembly;

    public AnyHarmony(Assembly assembly)
    {
        _assembly = assembly;
        _harmony = new Harmony(assembly.FullName);
        PatchAll(_assembly);
    }

    public void PatchAll(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var patches = type.GetCustomAttributes<AnyHarmonyPatch>(inherit: false);
            if (!patches.Any()) continue;

            foreach (var patch in patches)
            {
                // 根据特性配置获取指定方法
                var prefix = GetNamedHarmonyMethod(type, patch.Prefix)?? GetNamedHarmonyMethod(type, "Prefix");
                var postfix = GetNamedHarmonyMethod(type, patch.Postfix) ?? GetNamedHarmonyMethod(type, "Postfix");
                var transpiler = GetNamedHarmonyMethod(type, patch.Transpiler) ?? GetNamedHarmonyMethod(type, "Transpiler");
                var finalizer = GetNamedHarmonyMethod(type, patch.Finalizer) ?? GetNamedHarmonyMethod(type, "Finalizer");

                ApplyPatch(
                    patch.TargetType,
                    patch.MethodName,
                    prefix,
                    postfix,
                    transpiler,
                    finalizer
                );
            }
        }
    }

    private HarmonyMethod GetNamedHarmonyMethod(Type type, string methodName)
    {
        if (string.IsNullOrEmpty(methodName)) return null;
        var method = type.GetMethod(methodName,BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == null) return null;

        return new HarmonyMethod(method);
    }

    private void ApplyPatch(
        Type targetType,
        string methodName,
        HarmonyMethod prefix,
        HarmonyMethod postfix,
        HarmonyMethod transpiler,
        HarmonyMethod finalizer)
    {
        try
        {
            MethodBase[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            methods= methods.Concat(targetType.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
            methods= methods.Where(m => m.Name == methodName).ToArray();

            if (methods.Count() <= 0)
            {
                throw new MissingMethodException($"Method {methodName} not found in {targetType.FullName}");
            }
            foreach (var method in methods)
            {
                _harmony.Patch(
                    original: method,
                    prefix: prefix,
                    postfix: postfix,
                    transpiler: transpiler,
                    finalizer: finalizer
                );
                Console.WriteLine($"Custom Patch original:{method.Name} prefix:{prefix?.method?.Name} postfix:{postfix?.method?.Name} transpiler:{transpiler?.method?.Name} finalizer:{finalizer?.method?.Name}");
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