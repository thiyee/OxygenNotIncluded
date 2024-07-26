using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TUNING;
namespace 真荧光棒
{

	public class 真荧光棒
    {
		private static void Postfix(ref GlowStick.StatesInstance __instance)
		{
			if( PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.真荧光棒){
				
				var _radiationEmitterField = typeof(GlowStick.StatesInstance).GetField("_radiationEmitter", BindingFlags.NonPublic | BindingFlags.Instance);
				
				if (_radiationEmitterField != null)
				{
					RadiationEmitter newEmitter = (RadiationEmitter)_radiationEmitterField.GetValue(__instance);
					newEmitter.emitRads = 100 * PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.荧光棒强度;
					newEmitter.emitRadiusX = (short)PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.荧光棒范围;
					newEmitter.emitRadiusY = (short)PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.荧光棒范围;
				}

			}
		}

        public 真荧光棒(){
			MemberInfo[] memberInfo = typeof(GlowStick.StatesInstance).GetMembers().Where(m => m.Name == ".ctor").ToArray();
			ConstructorInfo ctor = memberInfo[0] as ConstructorInfo;
			Harmony harmony = new Harmony("荧光棒属性");
			harmony.Patch(ctor, postfix: new HarmonyMethod(typeof(真荧光棒), nameof(Postfix)));
		}

	}



}
