using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(typeof(HighEnergyParticle.States), "InitializeStates", Postfix:nameof(InitializeStates), ControlName: new string[] { nameof(大一统.大一统控制台UI.中子湮灭) })]
    public class 中子湮灭
    {
        static void InitializeStates(HighEnergyParticle.States __instance)
        {
            __instance.destroying.explode.enterActions.Clear();
            __instance.destroying.explode.PlayAnim("explode").Enter((smi) =>
            {
                EmitRemainingPayload(smi);

                if (smi.master.payload > 0)
                {
                    smi.master.isCollideable = false;//已经撞击过一次的辐射粒子避免与其他辐射粒子碰撞
                    smi.master.collision = HighEnergyParticle.CollisionType.None;
                    smi.Schedule(1f, (_) => smi.GoTo(__instance.ready.pre), null);
                }
                else
                {
                    smi.Schedule(1f, (_) => UnityEngine.Object.Destroy(smi.master.gameObject), null);
                }
            });

        }
        static FieldInfo femitter = typeof(HighEnergyParticle).GetField("emitter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        private static long _globalVersionCounter = 0;
        static Dictionary<int, (float originalMass, float consumed, long version)> pendingEmissions = new Dictionary<int, (float originalMass, float consumed, long version)>();

        static void UpdateRadiationEmitter(HighEnergyParticle.StatesInstance smi)
        {
            RadiationEmitter emitter = femitter.GetValue(smi.master) as RadiationEmitter;
            emitter.emitRadiusX = 6;
            emitter.emitRadiusY = 6;
            emitter.emitRads = smi.master.payload * 0.5f * 600f / 9f;
            emitter.Refresh();
        }
        public static void EmitRemainingPayload(HighEnergyParticle.StatesInstance smi)
        {
             int cell = Grid.PosToCell(smi.master);
            if (!Grid.IsValidCell(cell)) return;

            UpdateRadiationEmitter(smi);//撞击产生大量辐射
            if (smi.master.collision == HighEnergyParticle.CollisionType.Solid)
            {
                float currentMass = Grid.Mass[cell];
                (float originalMass, float consumed, long version) pending;
                if (!pendingEmissions.TryGetValue(cell, out pending) || pending.originalMass != currentMass)
                {
                    pendingEmissions[cell]= pending = (currentMass, 0, Interlocked.Increment(ref _globalVersionCounter));
                    long currentverion = pending.version;
                    smi.Schedule(0.3f, (_) => { if (pendingEmissions[cell].version == currentverion) pendingEmissions.Remove(cell); }, null);
                }
                float remainingMass = pending.originalMass * 1000 - pending.consumed;//剩余质量
                float actualConsume = Mathf.Min(remainingMass, smi.master.payload);//实际消耗

                if (actualConsume >= 0)
                {
                    //Console.WriteLine($"碰撞:{smi.master.collision} 当前载荷:{smi.master.payload} 消耗载荷:{actualConsume} 湮灭质量:{actualConsume/1000f:F2}kg");

                    pending.consumed += actualConsume;
                    smi.master.payload -= actualConsume;
                    if(pending.originalMass * 1000 - pending.consumed == 0)
                    {
                        SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0);
                    }
                    else
                    {
                        SimMessages.EmitMass(cell, Grid.Element[cell].idx, -actualConsume / 1000f, Grid.Temperature[cell], 0, 0);
                    }
                }
                else
                {
                    //Console.WriteLine($"完全湮灭");
                    SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0);
                }
            }
            else
            {
                SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.gameObject), SimHashes.Fallout, CellEventLogger.Instance.ElementEmitted, smi.master.payload * 0.001f, 5000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(smi.master.payload * 0.5f / 0.01f), true, -1);
                smi.master.payload = 0;
            }
        }

    }
}
