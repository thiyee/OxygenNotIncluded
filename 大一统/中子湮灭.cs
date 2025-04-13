using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 大一统{
    [AnyHarmonyPatch(typeof(HighEnergyParticle.States), "EmitRemainingPayload", Replace:nameof(Replcae), ControlName: new string[] { nameof(大一统.大一统控制台UI.中子湮灭) })]
    public class 中子湮灭
    {
        static FieldInfo femitter = typeof(HighEnergyParticle).GetField("emitter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        public void Replcae(HighEnergyParticle.StatesInstance smi)
        {
                int cell = Grid.PosToCell(smi.master);

                if (smi.master.collision == HighEnergyParticle.CollisionType.Solid)
                {
                    float EmitParticle = Grid.Mass[cell] * 1000;
                    Element element = Grid.Element[cell];
                    if (EmitParticle > smi.master.payload)
                    {
                        SimMessages.EmitMass(cell, element.idx, -smi.master.payload/1000f, Grid.Temperature[cell], 0, 0);
                        smi.master.payload = 0;
                    }
                    else
                    {
                        smi.master.payload -= EmitParticle;
                        SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0);
                    }
                    
                }
                smi.master.GetComponent<KBatchedAnimController>().GetCurrentAnim();
                RadiationEmitter emitter = femitter.GetValue(smi.master) as RadiationEmitter;
                emitter.emitRadiusX = 6;
                emitter.emitRadiusY = 6;
                emitter.emitRads = smi.master.payload * 0.5f * 600f / 9f;
                emitter.Refresh();
                if (smi.master.payload > 0)
                {
                    SimMessages.AddRemoveSubstance(cell,
                        SimHashes.Fallout,
                        CellEventLogger.Instance.ElementEmitted,
                        smi.master.payload * 0.001f, 5000f,
                        Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id),
                        Mathf.FloorToInt(smi.master.payload * 0.5f / 0.01f),
                        true,
                        -1);
                }
                smi.Schedule(1f, delegate (object obj)
                {
                    UnityEngine.Object.Destroy(smi.master.gameObject);
                }, null);
            
        }
    }
}
