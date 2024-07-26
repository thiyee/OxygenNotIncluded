using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace 中子湮灭发生器
{
	using System;
	using KSerialization;
	using UnityEngine;

	// Token: 0x02000C40 RID: 3136
	[SerializationConfig(MemberSerialization.OptIn)]
	public class UnobtaniumAnnihilationGenerator : StateMachineComponent<UnobtaniumAnnihilationGenerator.StatesInstance>, IHighEnergyParticleDirection
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x00025D47 File Offset: 0x00023F47
		// (set) Token: 0x06003E57 RID: 15959 RVA: 0x00169EDC File Offset: 0x001680DC
		public EightDirection Direction
		{
			get
			{
				return this._direction;
			}
			set
			{
				this._direction = value;
				if (this.directionController != null)
				{
					this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
					this.directionController.controller.enabled = false;
					this.directionController.controller.enabled = true;
				}
			}
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00025D4F File Offset: 0x00023F4F
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<UnobtaniumAnnihilationGenerator>(-905833192, UnobtaniumAnnihilationGenerator.OnCopySettingsDelegate);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00169F34 File Offset: 0x00168134
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.smi.StartSM();
			this.radiationEmitter.SetEmitting(false);
			this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
			this.Direction = this.Direction;
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00169F94 File Offset: 0x00168194
		private void OnCopySettings(object data)
		{
			UnobtaniumAnnihilationGenerator component = ((GameObject)data).GetComponent<UnobtaniumAnnihilationGenerator>();
			if (component != null)
			{
				this.Direction = component.Direction;
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00169FC4 File Offset: 0x001681C4
		public void LauncherUpdate()
		{
			if (this.particleStorage.Particles > 0f)
			{
				int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
				gameObject.SetActive(true);
				if (gameObject != null)
				{
					HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
					component.payload = this.particleStorage.ConsumeAndGet(this.particleStorage.Particles);
					component.SetDirection(this.Direction);
					this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
					this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
		}

		// Token: 0x04002AAB RID: 10923
		[MyCmpReq]
		private HighEnergyParticleStorage particleStorage;

		// Token: 0x04002AAC RID: 10924
		[MyCmpGet]
		private RadiationEmitter radiationEmitter;

		// Token: 0x04002AAD RID: 10925
		[Serialize]
		private EightDirection _direction;

		// Token: 0x04002AAE RID: 10926
		private EightDirectionController directionController;

		// Token: 0x04002AAF RID: 10927
		[MyCmpAdd]
		private CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04002AB0 RID: 10928
		private static readonly EventSystem.IntraObjectHandler<UnobtaniumAnnihilationGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<UnobtaniumAnnihilationGenerator>(delegate (UnobtaniumAnnihilationGenerator component, object data)
		{
			component.OnCopySettings(data);
		});

		// Token: 0x02000C41 RID: 3137
		public class StatesInstance : GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator, object>.GameInstance
		{
			// Token: 0x06003E5E RID: 15966 RVA: 0x00025D8C File Offset: 0x00023F8C
			public StatesInstance(UnobtaniumAnnihilationGenerator smi) : base(smi)
			{
			}

			// Token: 0x06003E5F RID: 15967 RVA: 0x00025D95 File Offset: 0x00023F95
			public bool IsComplexFabricatorWorkable(object data)
			{

				return data as ComplexFabricatorWorkable != null;
			}
		}

		// Token: 0x02000C42 RID: 3138
		public class States : GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator>
		{
			// Token: 0x06003E60 RID: 15968 RVA: 0x0016A088 File Offset: 0x00168288
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.inoperational;
				this.inoperational.Enter(delegate (UnobtaniumAnnihilationGenerator.StatesInstance smi)
				{
					smi.master.radiationEmitter.SetEmitting(false);
				}).TagTransition(GameTags.Operational, this.ready, false);
				this.ready.DefaultState(this.ready.idle).TagTransition(GameTags.Operational, this.inoperational, true).Update(
					delegate (UnobtaniumAnnihilationGenerator.StatesInstance smi, float dt){smi.master.LauncherUpdate();}, UpdateRate.SIM_200ms, false);
				this.ready.idle.EventHandlerTransition(GameHashes.WorkableStartWork, this.ready.working, (UnobtaniumAnnihilationGenerator.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data));
				this.ready.working.Enter(delegate (UnobtaniumAnnihilationGenerator.StatesInstance smi)
				{
					smi.master.radiationEmitter.SetEmitting(true);
				}).EventHandlerTransition(GameHashes.WorkableCompleteWork, this.ready.idle, (UnobtaniumAnnihilationGenerator.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).EventHandlerTransition(GameHashes.WorkableStopWork, this.ready.idle, (UnobtaniumAnnihilationGenerator.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).Exit(delegate (UnobtaniumAnnihilationGenerator.StatesInstance smi)
				{
					smi.master.radiationEmitter.SetEmitting(false);
				});
			}

			// Token: 0x04002AB1 RID: 10929
			public GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator, object>.State inoperational;

			// Token: 0x04002AB2 RID: 10930
			public UnobtaniumAnnihilationGenerator.States.ReadyStates ready;

			// Token: 0x02000C43 RID: 3139
			public class ReadyStates : GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator, object>.State
			{
				// Token: 0x04002AB3 RID: 10931
				public GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator, object>.State idle;

				// Token: 0x04002AB4 RID: 10932
				public GameStateMachine<UnobtaniumAnnihilationGenerator.States, UnobtaniumAnnihilationGenerator.StatesInstance, UnobtaniumAnnihilationGenerator, object>.State working;
			}
		}
	}

	public class UnobtaniumAnnihilationGeneratorConfig : IBuildingConfig
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x000351F4 File Offset: 0x000333F4
		public override string[] GetDlcIds()
		{
			return DlcManager.AVAILABLE_EXPANSION1_ONLY;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000351FC File Offset: 0x000333FC
		public override BuildingDef CreateBuildingDef()
		{
			string id = "UnobtaniumAnnihilationGenerator";
			int width = 1;
			int height = 2;
			string anim = "radiation_collector_kanim";
			int hitpoints = 30;
			float construction_time = 10f;
			float[] tier = new float[] { 400f };
			string[] raw_MINERALS = new string[] { "BuildableRaw" };
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = new EffectorValues { amount = 0, radius = 0 };
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints,
				construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues { amount = -10, radius = 2 }, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.Radiation.ID;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UseHighEnergyParticleOutputPort = true;
			buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.EnergyConsumptionWhenActive = 2000f;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "UnobtaniumAnnihilationGenerator");
			buildingDef.Deprecated = !Sim.IsRadiationEnabled();
			return buildingDef;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000352D4 File Offset: 0x000334D4
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
			Prioritizable.AddRef(go);
			go.AddOrGet<HighEnergyParticleStorage>();
			go.AddOrGet<LoopingSounds>();
			ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
			complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
			complexFabricator.duplicantOperated = false;
			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();
			ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
			BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
			complexFabricatorWorkable.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_manual_radbolt_generator_kanim") };
			complexFabricatorWorkable.workLayer = Grid.SceneLayer.BuildingUse;

			ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(SimHashes.EnrichedUranium.CreateTag(), 1000f) };
			ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[] { new ComplexRecipe.RecipeElement(ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL, 0.8f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false) };
			ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("HighEnergyParticleAnnihilation", array3, array4), array3, array4, 0, 10000000);
			complexRecipe2.time = 0f;
			complexRecipe2.description = "中子湮灭";
			complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient;
			complexRecipe2.fabricators = new List<Tag> { TagManager.Create("UnobtaniumAnnihilationGenerator") };

			go.AddOrGet<ManualHighEnergyParticleSpawner>();
			RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
			radiationEmitter.emissionOffset = new Vector3(0f, 1f);
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			radiationEmitter.emitRadiusX = 3;
			radiationEmitter.emitRadiusY = 3;
			radiationEmitter.emitRads = 120f;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00035341 File Offset: 0x00033541
		public override void DoPostConfigureComplete(GameObject go)
		{
		}

		// Token: 0x04000597 RID: 1431
		public const string ID = "UnobtaniumAnnihilationGenerator";
	}

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	class 添加建筑
	{
		public static void Prefix()
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.中子湮灭发生器)
			{
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.UNOBTANIUMANNIHILATIONGENERATOR.NAME", "中子湮灭发生器" });
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.UNOBTANIUMANNIHILATIONGENERATOR.EFFECT", "中子湮灭发生器" });
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.UNOBTANIUMANNIHILATIONGENERATOR.DESC", "发射大量辐射粒子 用于攻击中子物质或其他物质" });
				ModUtil.AddBuildingToPlanScreen("Base", "UnobtaniumAnnihilationGenerator");
			}
		}
	}
	[HarmonyPatch(typeof(HighEnergyParticleRedirectorConfig), "ConfigureBuildingTemplate")]
	class 辐射粒子变向器容量
	{
		private static void Postfix(ref HighEnergyParticleRedirectorConfig __instance, ref GameObject go, Tag prefab_tag)
		{
			if (PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.中子湮灭发生器)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
				highEnergyParticleStorage.capacity = 50000000f;
				go.AddOrGet<HighEnergyParticleRedirector>().directorDelay = 0f;
			}
		}
	}
}